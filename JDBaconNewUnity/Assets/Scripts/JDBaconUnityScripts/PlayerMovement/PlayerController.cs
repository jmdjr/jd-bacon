using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : GameObjectStateMachine
{
    private GameObjectState Idle = new GameObjectState("Idle");
    private GameObjectState WalkingRight = new GameObjectState("Walking to the Right");
    private GameObjectState WalkingLeft = new GameObjectState("Walking to the Left");
    private GameObjectState JumpingUp = new GameObjectState("Jumping into the Air");
    private GameObjectState DoubleJumpingUp = new GameObjectState("2nd Jumping into the Air");

    public float WalkingSpeed = 1f;
    public float JumpingSpeed = 2.0f;
    public bool AllowDoubleJump = true;

    private bool airborne = false;
    private bool airjumped = false;

    protected override void InitializeStateManager()
    {
        SetInitialState(Idle);
        
        ExitStateCondition ToWalkingLeft = new ExitStateCondition(ToWalkingLeftCondition, WalkingLeft);
        ExitStateCondition ToWalkingRight = new ExitStateCondition(ToWalkingRightCondition, WalkingRight);
        ExitStateCondition ToIdle = new ExitStateCondition(ToIdleCondition, Idle);
        ExitStateCondition ToJumpingUp = new ExitStateCondition(ToJumpCondition, JumpingUp);
        ExitStateCondition ToDoubleJumpingUp = new ExitStateCondition(ToDoubleJumpCondition, DoubleJumpingUp);

        Idle.Action = IdleAction;
        Idle.AddExitCondition(ToWalkingLeft);
        Idle.AddExitCondition(ToWalkingRight);
        Idle.AddExitCondition(ToJumpingUp);

        WalkingRight.Action = WalkingRightAction;
        WalkingRight.RepeatActionCount = 0;
        WalkingRight.AddExitCondition(ToWalkingLeft);
        WalkingRight.AddExitCondition(ToIdle);
        WalkingLeft.AddExitCondition(ToJumpingUp);

        WalkingLeft.Action = WalkingLeftAction;
        WalkingLeft.RepeatActionCount = 0;
        WalkingLeft.AddExitCondition(ToWalkingRight);
        WalkingLeft.AddExitCondition(ToIdle);
        WalkingLeft.AddExitCondition(ToJumpingUp);
    }

    #region Actions
    private IEnumerator IdleAction()
    {
        this.airjumped = false;
        this.airborne = false;

        yield return 0;
    }
    private IEnumerator WalkingRightAction()
    {
        this.rigidbody.velocity = new Vector3(WalkingSpeed * -1, this.rigidbody.velocity.y);
        yield return 0;
    }
    private IEnumerator WalkingLeftAction()
    {
        this.rigidbody.velocity = new Vector3(WalkingSpeed * 1, this.rigidbody.velocity.y);
        yield return 0;
    }
    private IEnumerator JumpingUpEntered()
    {
        airborne = true;
        yield return 0;
    }
    private IEnumerator DoubleJumpingUpEntered()
    {
        airborne = true;
        yield return 0;
    }
    private IEnumerator JumpingUpAction()
    {
        this.rigidbody.AddRelativeForce(0.0f, 0.0f, JumpingSpeed * Input.GetAxis("Vertical"), ForceMode.Impulse); 
        yield return 0;
    }
    #endregion

    #region Conditions
    private bool ToWalkingRightCondition()
    {
        return Input.GetAxis("Horizontal") > 0;
    }
    private bool ToWalkingLeftCondition()
    {
        return Input.GetAxis("Horizontal") < 0;
    }
    private bool ToIdleCondition()
    {
        return Input.GetAxis("Horizontal") == 0;
    }
    private bool ToJumpCondition()
    {
        return Input.GetAxis("Vertical") > 0 && !this.airborne;
    }
    private bool ToDoubleJumpCondition()
    {
        return Input.GetAxis("Vertical") > 0 && !this.airjumped;
    }
    #endregion



    // it should still be fine for a state machine to have an update function, so long as the update function does not
    //  attempt to alter the flow of the statemachine directly.
    public void Update()
    {
        if (Input.GetAxis("Vertical") > 0 && !this.airborne)
        {
            this.airborne = true;
            this.rigidbody.AddRelativeForce(0.0f, 0.0f, JumpingSpeed * Input.GetAxis("Vertical"), ForceMode.Impulse);
        }
    }

}
