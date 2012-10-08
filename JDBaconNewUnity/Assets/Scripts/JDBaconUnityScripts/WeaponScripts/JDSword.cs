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
        this.WeaponIdleAnimationType = HeroAnimationType.W_SWORD_IDLE;
        this.WeaponAttackAnimationType = HeroAnimationType.W_SWORD_ATTACK;
        this.WeaponIconType = HeroWeaponIconType.SWORD;
        this.DamageAmount = 10;
        this.CooldownTime = 0.4f;
        this.IsActive = false;
    }
}
