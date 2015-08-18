using UnityEngine;
using System;
using System.Collections;
using SmoothMoves;

public class HeroJumpingSM : JDStateMachine
{
    private HeroPhysicsProperties PhysicsProperties;
    private HeroAnimationProperties AnimationProperties;
    private JDMonoBodyBehavior ScriptReference;

    public HeroJumpingSM(JDHeroCharacter HeroReference, JDMonoBodyBehavior scriptReference)
    :base("Jumping")
    {
        this.PhysicsProperties = HeroReference.PhysicsProperties;
        this.AnimationProperties = HeroReference.AnimationProperties;
        this.ScriptReference = scriptReference;

        this.ScriptReference.ScriptUpdate += new MonoScriptEventHandler(ScriptReference_ScriptUpdate);
    }

    void ScriptReference_ScriptUpdate(MonoScriptEventArgs eventArgs)
    {
        if (!PhysicsProperties.HasReleasedJump)
        {
            PhysicsProperties.HasReleasedJump = Input.GetAxis("Jump") == 0;
        }

        RaycastHit info;
        if (Physics.Raycast(this.ScriptReference.ColliderCenter, Vector3.down, out info, 0.5f))
        {
            if (PhysicsProperties.IsAirborne)
            {
                AnimationProperties.UpdateStandardAnimation(HeroAnimationType.S_IDLE);
                PhysicsProperties.IsAirborne = false;
            }
        }
        else
        {
            if (!PhysicsProperties.IsAirborne)
            {
                AnimationProperties.UpdateStandardAnimation(HeroAnimationType.S_JUMP);
                PhysicsProperties.IsAirborne = true;
            }
        }
    }

    #region States
    private State IdleJumping = new State("Idle Jumping");
    private State JumpingUp = new State("Jumping into the Air");
    private State DoubleJumpingUp = new State("2nd Jumping into the Air");
    #endregion

    #region Actions
    private IEnumerator IdleJumpingAction()
    {
        yield return 0;
    }
    private IEnumerator JumpingUpEnterAction()
    {
        yield return 0;
    }
    private IEnumerator JumpingUpAction()
    {
        AnimationProperties.UpdateStandardAnimation(HeroAnimationType.S_JUMP);
        PhysicsProperties.HasReleasedJump = false;
        PhysicsProperties.ApplyJumpingPhysics(this.ScriptReference.rigidbody);
        yield return new WaitForSeconds(PhysicsProperties.WaitTimeForJump);
    }
    private IEnumerator JumpingUpExitAction()
    {
        yield return 0;
    }
    private IEnumerator JumpingUpAgainAction()
    {
        AnimationProperties.UpdateStandardAnimation(HeroAnimationType.S_JUMP);
        PhysicsProperties.HasReleasedJump = false;
        PhysicsProperties.ApplyJumpingPhysics(this.ScriptReference.rigidbody);
        yield return 0;
    }
    #endregion

    #region Conditions
    private bool ToIdleJumpingCondition()
    {
        return !PhysicsProperties.IsAirborne;
    }
    private bool ToJumpCondition()
    {
        return PhysicsProperties.HasReleasedJump && Input.GetAxis("Jump") > 0 && !PhysicsProperties.IsAirborne;
    }
    private bool ToDoubleJumpCondition()
    {
        return PhysicsProperties.HasReleasedJump && Input.GetAxis("Jump") > 0 && PhysicsProperties.IsAirborne && PhysicsProperties.AllowDoubleJump;
    }
    #endregion

    public override void  InitializeStateMachine()
    {
        this.SetInitialState(IdleJumping);
        ExitStateCondition ToIdleJump = new ExitStateCondition(ToIdleJumpingCondition, IdleJumping);
        ExitStateCondition ToJumpingUp = new ExitStateCondition(ToJumpCondition, JumpingUp);
        ExitStateCondition ToDoubleJumpingUp = new ExitStateCondition(ToDoubleJumpCondition, DoubleJumpingUp);

        IdleJumping.Action = IdleJumpingAction;
        IdleJumping.AddExitCondition(ToJumpingUp);

        JumpingUp.Action = JumpingUpAction;
        JumpingUp.Entering = JumpingUpEnterAction;
        JumpingUp.Exiting = JumpingUpExitAction;
        JumpingUp.RepeatActionCount = 1;
        JumpingUp.AddExitCondition(ToIdleJump);
        JumpingUp.AddExitCondition(ToDoubleJumpingUp);

        DoubleJumpingUp.Action = JumpingUpAgainAction;
        DoubleJumpingUp.Entering = JumpingUpEnterAction;
        DoubleJumpingUp.Exiting = JumpingUpExitAction;
        DoubleJumpingUp.RepeatActionCount = 1;
        DoubleJumpingUp.AddExitCondition(ToIdleJump);
    }
}