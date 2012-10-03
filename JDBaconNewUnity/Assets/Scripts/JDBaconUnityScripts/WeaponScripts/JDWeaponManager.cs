using UnityEngine;
using System;
using System.Collections;
using SmoothMoves;

public class JDWeaponManager
{
    protected int CurrentWeaponIndex;
    protected JDHeroWeapon[] Weapons = 
    {
        new JDSword(),
        new JDShotgun(),
    };

    public JDWeaponManager()
    {
        this.CurrentWeaponIndex = 0;
    }

    public void SwitchToWeapon (int weaponIndex)
    {
        this.CurrentWeaponIndex = weaponIndex;
        if (this.CurrentWeaponIndex >= Weapons.Length)
        {
            this.CurrentWeaponIndex = Weapons.Length - 1;
        }
    }
}
