using UnityEngine;
using System;
using System.Collections;
using SmoothMoves;

[RequireComponent(typeof(Rigidbody))]
public class JDPlayerController : JDMonoBodyBehavior
{
    public BoneAnimation Bone;
    public JDHeroCharacter Hero;

    [NonSerialized]
    public HeroSMS HeroMachineSystem;

    public override void Awake()
    {
        Hero = new JDHeroCharacter(new HeroAnimationProperties(new JDHeroAnimator()), new HeroPhysicsProperties()); 

        this.Body.renderer.enabled = false;
        HeroMachineSystem = new HeroSMS(this, this.Hero);

        this.JDCollection.Add(this.Hero);

        base.Awake();
    }

    public override void OnCollisionEnter(Collision other)
    {
        base.OnCollisionEnter(other);
    }
}