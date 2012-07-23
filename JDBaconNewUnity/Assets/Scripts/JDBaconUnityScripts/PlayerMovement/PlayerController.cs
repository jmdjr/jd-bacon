﻿using UnityEngine;
using System;
using System.Collections;
using SmoothMoves;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : StateMachineSystem
{
    #region Variables
    public BoneAnimation BoneAnimation;
    public float WalkingSpeed = 1f;
    public float MaxWalkSpeed = 10.0f;
    public ForceMode WalkingForceMode = ForceMode.Acceleration;
    public float JumpStrength = 1.0f;
    public float AntiGravityJumpFactor = -0.4f;
    public float WaitTimeForJump = 0.5f;
    public bool AllowDoubleJump = true;
    public ForceMode JumpingForceMode = ForceMode.Impulse;

    private bool airborne = false;
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
        this.BoneAnimation.CrossFade("Walk");
        yield return 0;
    }
    private IEnumerator StartWalkingRightAction()
    {
        this.BoneAnimation.CrossFade("Walk");
        yield return 0;
    }
    private IEnumerator StartIdleWalkAction()
    {
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
        Vector3 ResultingVelocity = this.rigidbody.velocity + NewMotion;
        if (ResultingVelocity.magnitude >= this.MaxWalkSpeed)
        {
            NewMotion.Normalize();
            NewMotion *= this.MaxWalkSpeed;
        }
            this.rigidbody.AddForce(NewMotion, WalkingForceMode);
        yield return 0;
    }
    private IEnumerator WalkingRightAction()
    {
        Vector3 NewMotion = WalkingSpeed * Vector3.right;
        Vector3 ResultingVelocity = this.rigidbody.velocity + NewMotion;
        if (ResultingVelocity.magnitude >= this.MaxWalkSpeed)
        {
            NewMotion.Normalize();
            NewMotion *= this.MaxWalkSpeed;
        }
        this.rigidbody.AddForce(NewMotion, WalkingForceMode);
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
        yield return new WaitForSeconds(WaitTimeForJump);
    }
    #endregion
    #region Conditions
    private bool ToIdleJumpingCondition()
    {
        //ERROR: This is bad logic, the transition condition needs to be changed to 
        //        properly detect when jumping is no longer occuring.
        return !this.airborne;
    }
    private bool ToJumpCondition()
    {
        return Input.GetAxis("Jump") > 0 && !this.airborne;
    }
    private bool ToDoubleJumpCondition()
    {
        return Input.GetAxis("Jump") > 0 && this.airborne && this.AllowDoubleJump;
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

        InitializeWalkingSM();
        InitializeJumpingSM();

        this.MachineList.Add(WalkingSM);
        this.MachineList.Add(JumpingSM);
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
