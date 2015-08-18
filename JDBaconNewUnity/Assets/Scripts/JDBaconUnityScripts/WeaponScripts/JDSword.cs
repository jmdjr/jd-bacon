using UnityEngine;
using System;
using System.Collections;
using SmoothMoves;

public class JDSword : JDHeroWeapon
{
    public JDSword(JDHeroCharacter hero)
    {
        this.heroReference = hero;

        this.Range = 0;
        this.WeaponIdleAnimationType = HeroAnimationType.W_NONE;
        this.WeaponAttackAnimationType = HeroAnimationType.W_SWORD_ATTACK;
        this.WeaponIconType = HeroWeaponIconType.SWORD;
        this.DamageAmount = 5;
        this.CooldownTime = 0.28f;
        this.IsActive = false;
    }
}
