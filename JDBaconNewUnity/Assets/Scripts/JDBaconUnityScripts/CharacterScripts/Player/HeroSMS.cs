using UnityEngine;
using System;
using System.Collections;
using SmoothMoves;
public class HeroSMS : JDStateMachineSystem
{
    #region Variables
    public HeroPhysicsProperties PhysicsProperties;
    public HeroAnimationProperties AnimationProperties;
    public HeroCharacter HeroReference;
    #endregion

    public HeroSMS(JDMonoBehavior scriptReference, HeroCharacter heroProps, HeroPhysicsProperties physProps, HeroAnimationProperties animProps)
        : base(scriptReference)
    {
        HeroReference = heroProps;
        PhysicsProperties = physProps;
        AnimationProperties = animProps;

        scriptReference.ScriptUpdate += new MonoScriptEventHandler(scriptReference_ScriptUpdate);
    }

    private void scriptReference_ScriptUpdate(MonoScriptEventArgs eventArgs)
    {
        this.Update();
    }

    #region Walking
    private StateMachine WalkingSM;
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
            AnimationProperties.UpdateStandardAnimation(HeroAnimationType.S_STAND);
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
    #endregion

    #region Jumping
    private StateMachine JumpingSM;

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
        AnimationProperties.UpdateStandardAnimation(HeroAnimationType.S_JUMP);
        yield return 0;
    }
    private IEnumerator JumpingUpAction()
    {
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
        return Input.GetAxis("Jump") > 0 && !PhysicsProperties.IsAirborne;
    }
    private bool ToDoubleJumpCondition()
    {
        return PhysicsProperties.HasReleasedJump && Input.GetAxis("Jump") > 0 && PhysicsProperties.IsAirborne && PhysicsProperties.AllowDoubleJump;
    }
    #endregion

    #endregion

    #region Attacking
    private StateMachine AttackingSM;

    #region States
    State IdleCombatState = new State("No Combat");
    State AttackingState = new State("Attacking");
    State DamageState = new State("Taking Damage");
    State SwitchingWeaponsState = new State("Switching between Weapons");
    #endregion

    #region Actions
    private IEnumerator SwitchingWeaponsAction()
    {
        if (!PhysicsProperties.IsAirborne)
        {
            AnimationProperties.UpdateStandardAnimation(HeroAnimationType.S_STAND);
        }
        yield return 0;
    }
    private IEnumerator IdleCombatAction()
    {
        yield return 0;
    }
    private IEnumerator AttackingAction()
    {
        //if (mWeapon != null)
        //{
        //    mWeapon.Attack();
        //}
        yield return 0;
    }
    private IEnumerator DamageAction()
    {
        yield return 0;
    }
    #endregion

    #region Conditions
    private bool SwitchingWeaponsCondition()
    {
        // You have pressed the button to attack.
        return Input.GetAxis("Fire2") > 0;
    }
    private bool IdleCombatCondition()
    {
        return true;
    }
    private bool AttackingCondition()
    {
        // You have pressed the button to attack.
        return Input.GetAxis("Fire1") > 0;
    }
    private bool DamageCondition()
    {
        // You have been hit by an enemy.
        return true;
    }
    #endregion

    #endregion

    protected void InitializeWalkingSM()
    {
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

        WalkingSM = new StateMachine("Walking", IdleWalking);
    }
    protected void InitializeJumpingSM()
    {
        JumpingSM = new StateMachine("Jumping", IdleJumping);
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

    protected void InitializeAttackingSM()
    {
        ExitStateCondition ToSwitchingWeapons = new ExitStateCondition(SwitchingWeaponsCondition, SwitchingWeaponsState);
        ExitStateCondition ToIdleCombatState = new ExitStateCondition(IdleCombatCondition, IdleCombatState);
        ExitStateCondition ToAttackingState = new ExitStateCondition(AttackingCondition, AttackingState);

        SwitchingWeaponsState.Action = SwitchingWeaponsAction;
        SwitchingWeaponsState.AddExitCondition(ToIdleCombatState);
        SwitchingWeaponsState.AddExitCondition(ToAttackingState);

        IdleCombatState.Action = IdleCombatAction;
        IdleCombatState.AddExitCondition(ToSwitchingWeapons);
        IdleCombatState.AddExitCondition(ToAttackingState);

        AttackingState.Action = AttackingAction;
        AttackingState.AddExitCondition(ToIdleCombatState);
        AttackingState.AddExitCondition(ToAttackingState);


        AttackingSM = new StateMachine("Attacking", IdleCombatState);
    }

    protected override void InitializeStateManager()
    {
        base.InitializeStateManager();
        //mWeapon = new MachineGun();

        InitializeWalkingSM();
        InitializeJumpingSM();
        InitializeAttackingSM();

        this.MachineList.Add(WalkingSM);
        this.MachineList.Add(JumpingSM);
        this.MachineList.Add(AttackingSM);
    }
    public void Update()
    {
        if (!PhysicsProperties.HasReleasedJump)
        {
            PhysicsProperties.HasReleasedJump = Input.GetAxis("Jump") == 0;
        }

        RaycastHit info;
        if (Physics.Raycast(this.ScriptReference.ColliderCenter, Vector3.down, out info, 0.4f))
        {
            if (PhysicsProperties.IsAirborne)
            {
                AnimationProperties.UpdateStandardAnimation(HeroAnimationType.S_STAND);
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
    public void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Enemy":
                //Player PlayerHealth = this.gameObject.GetComponent<Player>();
                //PlayerHealth.ChangeCurrentHealth(-1);
                //if (!PlayerHealth.IsAlive())
                //{
                //    PlayerHealth.Dead();
                //}
                break;
        }
    }
}
