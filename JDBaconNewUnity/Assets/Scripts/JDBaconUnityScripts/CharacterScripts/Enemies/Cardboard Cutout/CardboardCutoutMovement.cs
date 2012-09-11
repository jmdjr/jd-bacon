using UnityEngine;
using System;
using System.Collections;

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
    public float RunningSpeed = 1f;
    public float perception = 1f;
    public ForceMode WalkingForceMode = ForceMode.Force;
    public GameObject player;

    #endregion

    #region Waiting
    private StateMachine WaitingSM;
    #region States
    private State IdleWaiting = new State("Idle Waiting");
    private State WalkingRight = new State("Walk Right");
    private State WalkingLeft = new State("Walk Left");
    private State AttackPlayer = new State("Attack Player");
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
    private bool ToAttackPlayerCondition()
    {
        return currentDirection == Direction.Player;
    }

    #endregion
    #endregion

    #region Jumping
    /*
	private StateMachine AttackingSM;
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
    */
    #endregion

    protected void UpdateTimer()
    {
        if (player.gameObject == null)
        {
            Debug.Log("Game Object has gone missing!");
        }
        float distance = Vector3.Distance(this.transform.position, player.transform.position);
        ElapsedTime += Time.deltaTime;
        if (WaitTime < ElapsedTime)
        {
            ElapsedTime = 0;
            if (perception > distance)
                currentDirection = Direction.Player;
            else if (currentDirection == Direction.Right)
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
        ExitStateCondition ToAttackPlayer = new ExitStateCondition(ToAttackPlayerCondition, AttackPlayer);

        IdleWaiting.Action = IdleWaitingAction;
        IdleWaiting.AddExitCondition(ToWalkingLeft);
        IdleWaiting.AddExitCondition(ToWalkingRight);
        IdleWaiting.AddExitCondition(ToAttackPlayer);

        WalkingLeft.Action = WalkingLeftAction;
        WalkingLeft.RepeatActionCount = 0;
        WalkingLeft.AddExitCondition(ToWalkingRight);
        WalkingLeft.AddExitCondition(ToIdleWait);
        IdleWaiting.AddExitCondition(ToAttackPlayer);

        WalkingRight.Action = WalkingRightAction;
        WalkingRight.RepeatActionCount = 0;
        WalkingRight.AddExitCondition(ToWalkingLeft);
        WalkingRight.AddExitCondition(ToIdleWait);
        IdleWaiting.AddExitCondition(ToAttackPlayer);

        WaitingSM = new StateMachine("Waiting", IdleWaiting);
    }

    protected override void InitializeStateManager()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        base.InitializeStateManager();

        InitializeWalkingSM();
        this.MachineList.Add(WaitingSM);
    }

}

