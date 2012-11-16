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
        Hero = new JDHeroCharacter(new HeroAnimationProperties(new JDHeroAnimator(this.Bone)), new HeroPhysicsProperties()); 

        this.Body.renderer.enabled = false;
        HeroMachineSystem = new HeroSMS(this, this.Hero);

        this.JDCollection.Add(this.Hero);

        this.ScriptCollisionEnter += new MonoBodyScriptEventHanlder(JDPlayerController_ScriptCollisionEnter);

        base.Awake();
    }

    void JDPlayerController_ScriptCollisionEnter(CollisionEventArgs eventArgs)
    {
        if (eventArgs.JdOther.ObjectTagType == TagTypes.ENEMY)
        {
            JDCharacter ch = JDGame.GetCharacterFromCollider(eventArgs.Other.collider, this.Hero);
            if (ch != null)
            {
                this.Hero.UpdateHealth(ch.InflictingDamage());
            }
        }
    }

    public override void Update()
    {
        base.Update();

        if (this.Hero.IsDead)
        {
            this.gameObject.rigidbody.useGravity = false;
            this.gameObject.renderer.enabled = false;
            this.gameObject.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            Application.LoadLevel(Application.loadedLevel); // Resets level...
        }
    }
}