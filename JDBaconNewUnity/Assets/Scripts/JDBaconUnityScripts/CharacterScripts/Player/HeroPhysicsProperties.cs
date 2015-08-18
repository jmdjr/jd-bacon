using UnityEngine;
using System;
using System.Collections;
using SmoothMoves;

[Serializable]
public class HeroPhysicsProperties
{
    public float WalkingSpeed = 1.5f;
    public float MaxWalkSpeed = 1.5f;
    public ForceMode WalkingForceMode = ForceMode.Acceleration;
    public float JumpStrength = 3.0f;
    public float AntiGravityJumpFactor = -0.12f;
    public float WaitTimeForJump = 0.3f;
    public bool AllowDoubleJump = true;
    public ForceMode JumpingForceMode = ForceMode.Impulse;

    private bool airborne = false;
    private bool hasReleasedJump = false;

    public bool IsAirborne { get { return this.airborne; } set { this.airborne = value; } }
    public bool HasReleasedJump { get { return this.hasReleasedJump; } set { this.hasReleasedJump = value; } }

    public void ApplyWalkingPhysics(Rigidbody body, Vector3 Direction)
    {
        Vector3 NewMotion = WalkingSpeed * Direction;

        Vector3 myVelocity = body.velocity;
        Vector3 horizontalMotion = Vector3.ClampMagnitude(NewMotion, this.MaxWalkSpeed);
        
        body.velocity = new Vector3(horizontalMotion.x, myVelocity.y, 0);
    }

    public void ApplyJumpingPhysics(Rigidbody body)
    {
        Vector3 myVelocity = body.velocity;
        Vector3 horizontalMotion = Vector3.Cross(myVelocity, Vector3.left) + Vector3.Cross(myVelocity, Vector3.right);
        horizontalMotion = Vector3.ClampMagnitude(horizontalMotion, this.MaxWalkSpeed);
        body.velocity = new Vector3(horizontalMotion.x, JumpStrength, 0);
    }
}
