using UnityEngine;
using System;
using System.Collections;
using SmoothMoves;

public class JDSword : JDHeroWeapon
{
    public JDSword ()
    {
        this.Range = 0;
        this.WeaponIdleAnimationType = HeroAnimationType.W_SWORD_IDLE;
        this.WeaponAttackAnimationType = HeroAnimationType.W_SWORD_ATTACK;
        this.WeaponIconType = HeroWeaponIconType.SWORD;
        this.DamageAmount = 10;
        this.CooldownTime = 1;
        this.IsActive = false;
    }
}
