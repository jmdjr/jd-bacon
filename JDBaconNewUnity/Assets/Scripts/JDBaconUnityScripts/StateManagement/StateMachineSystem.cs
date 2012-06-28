using UnityEngine;
using System;
using System.Collections;

using Object = UnityEngine.Object;
using System.Collections.Generic;

// Use [RequireComponent(ComponentType)] for any external components you need on this Game object for reference.

// use as base class for all StateManagers in-game.  implement your own GameObjectStateManager for a specific AI,
// and attach it to the entity it will control.  your GOSM should inherit from this, add any new variables which 
// are needed for ExitTests, implement the Action for a state, along with all ExitStateConditions which apply.
// 

public class StateMachineSystem : MonoBehaviour
{
    protected List<StateMachine> MachineList = new List<StateMachine>();

    // Override in child class and load MachineList with machines.
    // The State Machine System will take care of running the machines
    //  and switching the states.
    protected virtual void InitializeStateManager()
    {
        StateMachine.CoroutineDelegate = this.StartCoroutine;
    }

    public void Awake()
    {
        Debug.Log("StateMachineSystem Boot Up.");
        InitializeStateManager();
        foreach (StateMachine machine in MachineList)
        {
            Debug.Log("starting Machine: " + machine.Name);
            machine.Start();
        }
    }

    protected sealed class StateMachine
    {
        // borrowing the 
        public delegate Coroutine StartCoroutine(IEnumerator func);

        public bool IsAlive { get { return isAlive; } }
        private GameObjectState currentState = null;
        private bool isAlive = false;
        private uint timesActionPerformed = 0;
        public string Name { get; private set; }

        public StateMachine(string Name, GameObjectState state = null)
        {
            this.Name = Name;
            this.SetInitialState(state);
        }

        public static StartCoroutine CoroutineDelegate = null;

        public void SetInitialState(GameObjectState state)
        {
            currentState = state;
        }

        private IEnumerator Run()
        {
            Debug.Log(this.Name + ": Running Machine");
            if (currentState != null)
            {
                Debug.Log(this.Name + ": Current State = " + currentState.Name);
                do
                {
                    GameObjectState ToState = currentState.RunTests();

                    if (ToState != null)
                    {
                        Debug.Log(this.Name + ": State Changed From..." + currentState.Name);

                        if (currentState.Exiting != null)
                        {
                            Debug.Log(this.Name + ": Exiting State = " + currentState.Name);
                            yield return CoroutineDelegate(currentState.Exiting());
                        }
                        currentState = ToState;

                        Debug.Log(this.Name + ": State Changed To..." + currentState.Name);
                        if (currentState.Entering != null)
                        {
                            Debug.Log(this.Name + ": Entering State = " + currentState.Name);
                            yield return CoroutineDelegate(currentState.Entering());
                        }

                        timesActionPerformed = 0;
                    }

                    if (currentState.RepeatActionCount == 0 || currentState.RepeatActionCount >= ++timesActionPerformed)
                    {
                        yield return CoroutineDelegate(currentState.Action());
                    }
                    else
                    {
                        yield return 0;
                    }

                } while (!currentState.DeadState || currentState.RepeateIfDead);
            }
            else
            {
                Debug.Log(this.Name + ": Current State Not Set");
            }
        }

        public void Start()
        {
            isAlive = true;
            Debug.Log(this.Name + ": Start Entered");
            CoroutineDelegate(Run());
        }
    }
    protected sealed class GameObjectState
    {
        public delegate IEnumerator ActionDelegate();

        // Name of this State, for debugging purposes.
        public string Name { get { return name; } }
        private string name;

        // This state is dead if it doesn't have any exit conditions, meanning it doesn't go anywhere past this action.
        public bool DeadState { get { return exitConditionList.Count == 0; } }
        public bool RepeateIfDead { get; set; }
        public uint RepeatActionCount { get; set; }

        /// <summary>
        /// Action Performed immidiately after transition into the state. <br/>
        /// 
        /// </summary>
        public ActionDelegate Entering = null;
        public ActionDelegate Action = null;
        public ActionDelegate Exiting = null;
        public IList ExitConditionList { get { return exitConditionList.AsReadOnly(); } }
        private List<ExitStateCondition> exitConditionList = new List<ExitStateCondition>();

        public GameObjectState(string name)
        {
            this.name = name;
            this.RepeateIfDead = false;
        }

        public bool AddExitCondition(ExitStateCondition condition)
        {
            // Cannot have duplicate ExitStateConditions in the same list.
            if (!exitConditionList.Contains(condition))
            {
                exitConditionList.Add(condition);
                return true;
            }

            return false;
        }
        public bool RemoveExitCondition(ExitStateCondition condition)
        {
            if (exitConditionList.Contains(condition))
            {
                exitConditionList.Remove(condition);
                return true;
            }

            return false;
        }

        public GameObjectState RunTests()
        {
            foreach (ExitStateCondition condition in exitConditionList)
            {
                if (condition.ExitTest())
                {
                    return condition.ToState;
                }
            }

            return null;
        }

        public override string ToString()
        {
            return Name;
        }
    }
    protected sealed class ExitStateCondition
    {
        public delegate bool ExitCondition();
        public ExitCondition ExitTest;
        public GameObjectState ToState;

        public ExitStateCondition(ExitCondition test, GameObjectState state)
        {
            ExitTest = test;
            ToState = state;
        }
    }

}

