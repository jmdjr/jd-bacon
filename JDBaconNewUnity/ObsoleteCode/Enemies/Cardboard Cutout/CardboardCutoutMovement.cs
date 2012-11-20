
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using SmoothMoves;

using Object = UnityEngine.Object;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MeshCollider))]
public class CardboardCutoutMovement : StateMachineSystem
{
    public enum Direction
    {
        Left,
        Right,
        Wait,
        Player,
    }

    #region Variables
    public float WaitTime = 3f;
    public float ElapsedTime = 0f;
    public float DamagedTime = 1f;
    public Direction currentDirection = Direction.Left;
    public float WalkingSpeed = .5f;
    public ForceMode WalkingForceMode = ForceMode.Force;

    #endregion

    #region Waiting
    private StateMachine WaitingSM;
    #region States
    private State IdleWaiting = new State("Idle Waiting");
    private State WalkingRight = new State("Walk Right");
    private State WalkingLeft = new State("Walk Left");
    #endregion
    #region Actions
    private IEnumerator IdleWaitingAction()
    {
        this.rigidbody.velocity = Vector3.zero;
        yield return 0;
    }
    private IEnumerator WalkingLeftAction()
    {
        Vector3 NewMotion = WalkingSpeed * Vector3.left;
        this.rigidbody.velocity = NewMotion;
        yield return 0;
    }
    private IEnumerator WalkingRightAction()
    {
        Vector3 NewMotion = WalkingSpeed * Vector3.right;
        this.rigidbody.velocity = NewMotion;
        yield return 0;
    }
    #endregion
    #region Conditions
    private bool ToWalkingRightCondition()
    {
        UpdateTimer();
        return currentDirection == Direction.Right;
    }
    private bool ToWalkingLeftCondition()
    {
        UpdateTimer();
        return currentDirection == Direction.Left;
    }
    private bool ToIdleWaitingCondition()
    {
        UpdateTimer();
        return currentDirection == Direction.Wait;
    }
    #endregion
    #endregion

    protected void UpdateTimer()
    {
        ElapsedTime += Time.deltaTime;
        if (WaitTime < ElapsedTime)
        {
            ElapsedTime = 0;
            if (currentDirection == Direction.Right)
                currentDirection = Direction.Left;
            else if (currentDirection == Direction.Left)
                currentDirection = Direction.Wait;
            else
                currentDirection = Direction.Right;
        }
    }
    protected void InitializeWalkingSM()
    {
        ExitStateCondition ToIdleWait = new ExitStateCondition(ToIdleWaitingCondition, IdleWaiting);
        ExitStateCondition ToWalkingLeft = new ExitStateCondition(ToWalkingLeftCondition, WalkingLeft);
        ExitStateCondition ToWalkingRight = new ExitStateCondition(ToWalkingRightCondition, WalkingRight);

        IdleWaiting.Action = IdleWaitingAction;
        IdleWaiting.AddExitCondition(ToWalkingLeft);
        IdleWaiting.AddExitCondition(ToWalkingRight);

        WalkingLeft.Action = WalkingLeftAction;
        WalkingLeft.RepeatActionCount = 0;
        WalkingLeft.AddExitCondition(ToWalkingRight);
        WalkingLeft.AddExitCondition(ToIdleWait);

        WalkingRight.Action = WalkingRightAction;
        WalkingRight.RepeatActionCount = 0;
        WalkingRight.AddExitCondition(ToWalkingLeft);
        WalkingRight.AddExitCondition(ToIdleWait);

        WaitingSM = new StateMachine("Waiting", IdleWaiting);
    }

    protected override void InitializeStateManager()
    {
        base.InitializeStateManager();

        InitializeWalkingSM();
        this.MachineList.Add(WaitingSM);
    }

}

