using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class JDShotgun : JDHeroWeapon
{
    public JDShotgun ()
    {
        this.Range = 0;
        this.WeaponIdleAnimationType = HeroAnimationType.W_SWORD_IDLE;
        this.WeaponAttackAnimationType = HeroAnimationType.W_SWORD_ATTACK;
        this.WeaponIconType = HeroWeaponIconType.SHOTGUN;
        this.DamageAmount = 10;
        this.CooldownTime = 1;
        this.IsActive = false;
    }
}
