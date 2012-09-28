using UnityEngine;
using System;
using System.Collections;
using SmoothMoves;

[RequireComponent(typeof(Rigidbody))]
public class JDPlayerController : JDMonoBodyBehavior
{
    public HeroCharacter CharacterProperties = new HeroCharacter();
    public HeroPhysicsProperties PhysicsProperties = new HeroPhysicsProperties();
    public BoneAnimation Bone;
    private HeroAnimationProperties animateProperties;
    public HeroSMS HeroMachineSystem;

    public override void Awake()
    {
        animateProperties = new HeroAnimationProperties(new HeroAnimator(this.Bone));
        this.Body.renderer.enabled = false;
        HeroMachineSystem = new HeroSMS(this, this.CharacterProperties, this.PhysicsProperties, animateProperties);

        base.Awake();
    }

    public override void OnCollisionEnter(Collision other)
    {
        base.OnCollisionEnter(other);
    }
}