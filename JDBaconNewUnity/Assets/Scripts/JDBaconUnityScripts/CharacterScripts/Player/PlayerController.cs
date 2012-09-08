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
    public float WaitTimeForJump = 0.25f;
    public bool AllowDoubleJump = true;
    public ForceMode JumpingForceMode = ForceMode.Impulse;
    private bool facingLeft = false;

    private bool airborne = false;
    private bool hasReleasedJump = false;
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
        this.BoneAnimation.Play("Walk");
        yield return 0;
    }
    private IEnumerator StartWalkingRightAction()
    {
        if (facingLeft)
        {
            this.transform.Rotate(Vector3.up, 180.0f);
            facingLeft = false;
        }
        this.BoneAnimation.CrossFade("Walk");
        yield return 0;
    }
    private IEnumerator StartIdleWalkAction()
    {
        this.BoneAnimation.Stop();
        this.BoneAnimation.CrossFade("Stand");
        yield return 0;
    }
    #endregion
    #region Actions
    private IEnumerator IdleWalkingAction()
    {
        yield return 0;
    }
    private IEnumerator WalkingLeftAction()
    {
        Vector3 NewMotion = WalkingSpeed * Vector3.left;
        this.rigidbody.AddForce(NewMotion, WalkingForceMode);
        float yComp = this.rigidbody.velocity.y;
        Vector3 horizontalMotion = Vector3.Cross(this.rigidbody.velocity, Vector3.left) + Vector3.Cross(this.rigidbody.velocity, Vector3.right);
        horizontalMotion = Vector3.ClampMagnitude(horizontalMotion, this.MaxWalkSpeed);

        this.rigidbody.velocity = new Vector3(horizontalMotion.x, yComp, 0);
        yield return 0;
    }
    private IEnumerator WalkingRightAction()
    {
        Vector3 NewMotion = WalkingSpeed * Vector3.right;
        this.rigidbody.AddForce(NewMotion, WalkingForceMode);
        float yComp = this.rigidbody.velocity.y;
        Vector3 horizontalMotion = Vector3.Cross(this.rigidbody.velocity, Vector3.left) + Vector3.Cross(this.rigidbody.velocity, Vector3.right);
        horizontalMotion = Vector3.ClampMagnitude(horizontalMotion, this.MaxWalkSpeed);

        this.rigidbody.velocity = new Vector3(horizontalMotion.x, yComp, 0);
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
    private IEnumerator JumpingUpAction()
    {
        Vector3 NewMotion = this.rigidbody.mass * Physics.gravity * AntiGravityJumpFactor * JumpStrength;
        this.rigidbody.AddForce(NewMotion, JumpingForceMode);
        this.airborne = true;
        hasReleasedJump = false;
        yield return new WaitForSeconds(WaitTimeForJump);
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
        JumpingUp.RepeatActionCount = 1;
        JumpingUp.AddExitCondition(ToIdleJump);
        JumpingUp.AddExitCondition(ToDoubleJumpingUp);

        DoubleJumpingUp.Action = JumpingUpAction;
        DoubleJumpingUp.RepeatActionCount = 1;
        DoubleJumpingUp.AddExitCondition(ToIdleJump);
    }

    protected override void InitializeStateManager()
    {
        base.InitializeStateManager();

        this.renderer.enabled = false;

        InitializeWalkingSM();
        InitializeJumpingSM();

        this.MachineList.Add(WalkingSM);
        this.MachineList.Add(JumpingSM);
    }

    public void Update()
    {
        if (!hasReleasedJump)
        {
            hasReleasedJump = Input.GetAxis("Jump") == 0;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.transform.tag == "LevelTerrain")
        {
            this.airborne = false;
        }
		else if (collision.collider.transform.tag == "Enemy")
		{
			PlayerHealth PlayerHealth = this.gameObject.GetComponent<PlayerHealth>();
			PlayerHealth.ChangeCurrentHealth(-3);
			if(!PlayerHealth.IsAlive())
			{
				PlayerHealth.Dead();
			}
		}
    }
}
