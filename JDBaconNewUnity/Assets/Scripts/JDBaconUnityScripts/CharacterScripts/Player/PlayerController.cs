using UnityEngine;
using System;
using System.Collections;
using SmoothMoves;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : StateMachineSystem
{
    #region Variables
    public BoneAnimation BoneAnimation;
    public float WalkingSpeed = 50f;
    public float MaxWalkSpeed = 100.0f;
    public ForceMode WalkingForceMode = ForceMode.Acceleration;
    public float JumpStrength = 3.0f;
    public float AntiGravityJumpFactor = -0.12f;
    public float WaitTimeForJump = 0.0f;
    public bool AllowDoubleJump = true;
    public ForceMode JumpingForceMode = ForceMode.Impulse;
    private bool facingLeft = false;

    private bool airborne = false;
    private bool hasReleasedJump = false;
    private CharacterAnimationHelper Animator = null;

    private HeroAnimationType currentStandard = HeroAnimationType.S_STAND;
    private HeroAnimationType currentWeapon = HeroAnimationType.W_NONE;
    private HeroAnimationType currentDirection = HeroAnimationType.D_STRAIT;
    #endregion

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
        if (!facingLeft)
        {
            this.transform.Rotate(Vector3.up, 180.0f);
            facingLeft = true;
        }
        yield return 0;
    }
    private IEnumerator StartWalkingRightAction()
    {
        if (facingLeft)
        {
            this.transform.Rotate(Vector3.up, 180.0f);
            facingLeft = false;
        }
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
        if (!this.airborne)
        {
            this.UpdateStandardAnimation(HeroAnimationType.S_STAND);
        }
        yield return 0;
    }
    private IEnumerator WalkingLeftAction()
    {
        if (!this.airborne)
        {
            this.UpdateStandardAnimation(HeroAnimationType.S_WALK);
        }

        ApplyWalkingPhysics(Vector3.left);
        yield return 0;
    }
    private IEnumerator WalkingRightAction()
    {
        if (!this.airborne)
        {
            this.UpdateStandardAnimation(HeroAnimationType.S_WALK);
        } 
        
        ApplyWalkingPhysics(Vector3.right);

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
        this.UpdateStandardAnimation(HeroAnimationType.S_JUMP);
        yield return 0;
    }
    private IEnumerator JumpingUpAction()
    {
        hasReleasedJump = false;
        ApplyJumpingPhysics();
        yield return new WaitForSeconds(WaitTimeForJump);
    }
    private IEnumerator JumpingUpExitAction()
    {
        yield return 0;
    }
    private IEnumerator JumpingUpAgainAction()
    {
        hasReleasedJump = false;
        ApplyJumpingPhysics();
        yield return 0;
    }
    #endregion

    #region Conditions
    private bool ToIdleJumpingCondition()
    {
        return !this.airborne;
    }
    private bool ToJumpCondition()
    {
        return Input.GetAxis("Jump") > 0 && !this.airborne;
    }
    private bool ToDoubleJumpCondition()
    {
        return hasReleasedJump && Input.GetAxis("Jump") > 0 && this.airborne && this.AllowDoubleJump;
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
        if (!this.airborne)
        {
            this.UpdateStandardAnimation(HeroAnimationType.S_STAND);
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

    #region Entering Action
    #endregion

    #region Action
    #endregion

    #region Exit Action

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
        this.Animator = new CharacterAnimationHelper(this.BoneAnimation);
        this.renderer.enabled = false;
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
        if (!hasReleasedJump)
        {
            hasReleasedJump = Input.GetAxis("Jump") == 0;
        }

        RaycastHit info;
        if (Physics.Raycast(this.gameObject.collider.bounds.center, Vector3.down, out info, 0.4f))
        {
            if(this.airborne)
            {
                this.UpdateStandardAnimation(HeroAnimationType.S_STAND);
                this.airborne = false;
            }
        }
        else
        {
            if (!this.airborne)
            {
                this.UpdateStandardAnimation(HeroAnimationType.S_JUMP);
                this.airborne = true;
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
    
    #region Helper Functions
    private void UpdateWeaponAnimation(HeroAnimationType weaponType)
    {
        this.currentWeapon = weaponType.TypeToWeapon();
        this.UpdateAnimation();
    }
    private void UpdateStandardAnimation(HeroAnimationType standardType)
    {
        this.currentStandard = standardType.TypeToStandard();
        this.UpdateAnimation();
    }
    private void UpdateDirectionAnimation(HeroAnimationType directionType)
    {
        this.currentDirection = directionType.TypeToDirection();
        this.UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        this.Animator.PlayAnimation(this.currentStandard | this.currentWeapon | this.currentDirection);
    }
    private void ApplyWalkingPhysics(Vector3 Direction)
    {
        Vector3 NewMotion = WalkingSpeed * Direction;

        this.rigidbody.AddForce(NewMotion, WalkingForceMode);
        
        Vector3 myVelocity = this.rigidbody.velocity;
        Vector3 horizontalMotion = Vector3.Cross(myVelocity, Vector3.left) + Vector3.Cross(myVelocity, Vector3.right);
        horizontalMotion = Vector3.ClampMagnitude(horizontalMotion, this.MaxWalkSpeed);

        this.rigidbody.velocity = new Vector3(horizontalMotion.x, myVelocity.y, 0);
    }
    private void ApplyJumpingPhysics()
    {
        Vector3 NewMotion = this.rigidbody.mass * Physics.gravity * AntiGravityJumpFactor * JumpStrength;
        Vector3 originalV = this.rigidbody.velocity;
        this.rigidbody.velocity = new Vector3(originalV.x, 0, 0);
        this.rigidbody.AddForce(NewMotion, JumpingForceMode);
    }
    #endregion
}
