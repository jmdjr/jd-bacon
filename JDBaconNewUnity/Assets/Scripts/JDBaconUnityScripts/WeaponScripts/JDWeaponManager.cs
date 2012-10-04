using UnityEngine;
using System;
using System.Collections;
using SmoothMoves;

public class JDWeaponManager
{
    protected int CurrentWeaponIndex;
    protected JDHeroWeapon[] Weapons; 

    public JDWeaponManager(JDHeroCharacter heroReference)
    {
        this.CurrentWeaponIndex = 0;

        Weapons = new JDHeroWeapon[]
        {
            new JDSword(heroReference),
            new JDShotgun(heroReference),
        };
    }

    public void SwitchToWeapon (int weaponIndex)
    {
        this.CurrentWeaponIndex = weaponIndex;
        if (this.CurrentWeaponIndex >= Weapons.Length)
        {
            this.CurrentWeaponIndex = Weapons.Length - 1;
        }
    }

    public HeroAnimationType GetWeaponAttack ()
    {
        return Weapons[this.CurrentWeaponIndex].WeaponAttackAnimationType.TypeToWeapon();
    }
    
    public HeroAnimationType GetWeaponIdle()
    {
        return Weapons[this.CurrentWeaponIndex].WeaponIdleAnimationType.TypeToWeapon();
    }

    public HeroWeaponIconType GetWeaponIcon()
    {
        return Weapons[this.CurrentWeaponIndex].WeaponIconType;
    }
}
