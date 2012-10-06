using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class JDShotgun : JDHeroWeapon
{
    public JDShotgun (JDHeroCharacter hero)
    {
        this.heroReference = hero;

        this.Range = 0;
        this.WeaponIdleAnimationType = HeroAnimationType.W_NONE;
        this.WeaponAttackAnimationType = HeroAnimationType.W_NONE;
        this.WeaponIconType = HeroWeaponIconType.SHOTGUN;
        this.DamageAmount = 10;
        this.CooldownTime = 1;
        this.IsActive = false;
    }
}
