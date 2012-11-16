
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using SmoothMoves;

using Object = UnityEngine.Object;

[Serializable]
public class JDHeroCharacter : JDCharacter
{
    public HeroAnimationProperties AnimationProperties;
    public HeroPhysicsProperties PhysicsProperties;

    public JDWeaponManager WeaponManager;

    public JDHeroCharacter(HeroAnimationProperties animationProps, HeroPhysicsProperties physicsProperties)
    {
        this.PhysicsProperties = physicsProperties;
        this.AnimationProperties = animationProps;

        this.WeaponManager = new JDWeaponManager(this);
    }

    public override int InflictingDamage()
    {
        return -1 * this.WeaponManager.CurrentWeapon.DamageAmount;
    }
}

