using UnityEngine;
using System;
using System.Collections;
using SmoothMoves;

public class HeroHorizontalMovementSM : JDStateMachine
{
    private HeroPhysicsProperties PhysicsProperties;
    private HeroAnimationProperties AnimationProperties;
    private JDMonoBodyBehavior ScriptReference;

    public HeroHorizontalMovementSM(JDHeroCharacter HeroReference, JDMonoBodyBehavior scriptReference)
    :base("Walking")
    {
        this.PhysicsProperties = HeroReference.PhysicsProperties;
        this.AnimationProperties = HeroReference.AnimationProperties;
        this.ScriptReference = scriptReference;
    }

    #region States
    private State IdleWalking = new State("Idle Walking");
    private State WalkingRight = new State("Walking to the Right");
    private State WalkingLeft = new State("Walking to the Left");
    #endregion
    #region Start Action
    private IEnumerator StartWalkingLeftAction()
    {
        AnimationProperties.FaceLeft();
        yield return 0;
    }
    private IEnumerator StartWalkingRightAction()
    {
        AnimationProperties.FaceRight();
        yield return 0;
    }
    private IEnumerator StartIdleWalkAction()
    {
        yield return 0;
    }
    #endregion
    #region Actions
    private IEnumerator IdleWalkingAction()
    {
        if (!PhysicsProperties.IsAirborne)
        {
            AnimationProperties.UpdateStandardAnimation(HeroAnimationType.S_IDLE);
        }
        yield return 0;
    }
    private IEnumerator WalkingLeftAction()
    {
        if (!PhysicsProperties.IsAirborne)
        {
            AnimationProperties.UpdateStandardAnimation(HeroAnimationType.S_WALK);
        }

        PhysicsProperties.ApplyWalkingPhysics(this.ScriptReference.Body, Vector3.left);
        yield return 0;
    }
    private IEnumerator WalkingRightAction()
    {
        if (!PhysicsProperties.IsAirborne)
        {
            AnimationProperties.UpdateStandardAnimation(HeroAnimationType.S_WALK);
        }

        PhysicsProperties.ApplyWalkingPhysics(this.ScriptReference.Body, Vector3.right);

        yield return 0;
    }
    #endregion
    #region Conditions
    private bool ToWalkingRightCondition()
    {
        return Input.GetAxis("Horizontal") < 0;
    }
    private bool ToWalkingLeftCondition()
    {
        return Input.GetAxis("Horizontal") > 0;
    }
    private bool ToIdleWalkingCondition()
    {
        return Input.GetAxis("Horizontal") == 0;
    }
    #endregion

    public override void  InitializeStateMachine()
    {
        this.SetInitialState(this.IdleWalking);
        ExitStateCondition ToIdleWalk = new ExitStateCondition(ToIdleWalkingCondition, IdleWalking);
        ExitStateCondition ToWalkingLeft = new ExitStateCondition(ToWalkingLeftCondition, WalkingLeft);
        ExitStateCondition ToWalkingRight = new ExitStateCondition(ToWalkingRightCondition, WalkingRight);

        IdleWalking.Entering = StartIdleWalkAction;
        IdleWalking.Action = IdleWalkingAction;
        IdleWalking.AddExitCondition(ToWalkingLeft);
        IdleWalking.AddExitCondition(ToWalkingRight);

        WalkingLeft.Entering = StartWalkingLeftAction;
        WalkingLeft.Action = WalkingLeftAction;
        WalkingLeft.RepeatActionCount = 0;
        WalkingLeft.AddExitCondition(ToWalkingRight);
        WalkingLeft.AddExitCondition(ToIdleWalk);

        WalkingRight.Entering = StartWalkingRightAction;
        WalkingRight.Action = WalkingRightAction;
        WalkingRight.RepeatActionCount = 0;
        WalkingRight.AddExitCondition(ToWalkingLeft);
        WalkingRight.AddExitCondition(ToIdleWalk);
    }
}