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

public class GameObjectStateMachine : MonoBehaviour
{
    private GameObjectState currentState = null;

    // Implement this function like a constructor. specifically, piece together the States and ExitConditions.
    protected virtual void InitializeStateManager(){}

    protected void SetInitialState(GameObjectState state)
    {
        currentState = state;
    }

    private IEnumerator Run()
    {
        if (currentState != null)
        {
            uint timesActionPerformed = 0;
            do
            {
                Debug.Log(currentState.ToString());
                GameObjectState ToState = currentState.RunTests();

                if (ToState != null)
                {
                    if (currentState.Exiting != null)
                    {
                        yield return StartCoroutine(currentState.Exiting());
                    }
                    currentState = ToState;

                    if (currentState.Entering != null)
                    {
                        yield return StartCoroutine(currentState.Entering());
                    }

                    timesActionPerformed = 0;
                }

                if (currentState.RepeatActionCount == 0 || currentState.RepeatActionCount >= ++timesActionPerformed)
                {
                    yield return StartCoroutine(currentState.Action());
                }
                else
                {
                    yield return 0;
                }

            } while (!currentState.DeadState || currentState.RepeateIfDead);
        }
    }

    public void Awake()
    {
        InitializeStateManager();
        StartCoroutine(Run());
    }
}

public sealed class GameObjectState
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

public class ExitStateCondition
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