using UnityEngine;
using System;
using System.Collections;
using SmoothMoves;

public class JDNoWeapon: JDHeroWeapon
{
    public JDNoWeapon(JDHeroCharacter hero)
    {
        this.heroReference = hero;
        this.Range = 0;
        this.WeaponIdleAnimationType = HeroAnimationType.W_NONE;
        this.WeaponAttackAnimationType = HeroAnimationType.W_NONE;
        this.WeaponIconType = HeroWeaponIconType.NONE;
        this.DamageAmount = 1;
        this.CooldownTime = 1;
        this.IsActive = true;
    }
}
