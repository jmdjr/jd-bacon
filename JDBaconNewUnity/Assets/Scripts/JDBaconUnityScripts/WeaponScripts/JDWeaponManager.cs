﻿using UnityEngine;
using System;
using System.Collections;
using SmoothMoves;
using System.Collections.Generic;
using System.Xml;
using System.IO;

public class JDWeaponManager
{
    protected int CurrentWeaponIndex;
    protected List<JDHeroWeapon> Weapons;
    public List<JDHeroWeapon> WeaponsList
    {
        get { return Weapons; }
    }

    public JDHeroWeapon CurrentWeapon { get { return this.Weapons[CurrentWeaponIndex]; } }
    public JDWeaponManager(JDHeroCharacter heroReference)
    {
        this.CurrentWeaponIndex = 1;

        // will convert this to reading a document later.
//        String xmlString = @"
//<Weapons>
//    <Name>NoWeapon</Name>
//    <Range>0</Range>
//    <WeaponIdleAnimationType>W_NONE</WeaponIdleAnimationType>
//    <WeaponAttackAnimationType>W_NONE</WeaponAttackAnimationType>
//    <WeaponIconType>NONE</WeaponIconType>
//    <DamageAmount>0</DamageAmount>
//    <CooldownTime>1</CooldownTime>
//    <IsActive>true</IsActive>
//</Weapons>
//";
//        JDHeroWeapon weapons = (JDHeroWeapon)JDGameUtilz.DeserializeObject(xmlString, "Weapons", typeof(JDHeroWeapon), JDGameUtilz.EncodingType.UTF8);

//        Debug.Log(weapons);
        
        Weapons = new List<JDHeroWeapon>()
        {
            new JDNoWeapon(heroReference),
            new JDSword(heroReference),
            new JDShotgun(heroReference)
        };
    }

    public void SwitchToWeapon (int weaponIndex)
    {
        this.CurrentWeaponIndex = weaponIndex;
        if (this.CurrentWeaponIndex >= Weapons.Count)
        {
            this.CurrentWeaponIndex = Weapons.Count - 1;
        }
    }

    public void GotoNextWeapon()
    {
        ++this.CurrentWeaponIndex;

        if (this.CurrentWeaponIndex >= Weapons.Count)
        {
            this.CurrentWeaponIndex = 0;
        }
    }
    public void GotoPreviousWeapon()
    {
        --this.CurrentWeaponIndex;

        if (this.CurrentWeaponIndex < 0)
        {
            this.CurrentWeaponIndex = Weapons.Count - 1;
        }
    }
    public int GetCurrentWeaponIndex()
    {
        return this.CurrentWeaponIndex;
    }
    public HeroAnimationType GetWeaponAttack()
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
