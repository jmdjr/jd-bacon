using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Object = UnityEngine.Object;
using Random = System.Random;

public class WeaponButton : JDMonoGuiBehavior
{
    JDWeapon weaponReference = null;
    LevelManager level = LevelManager.Instance;
    WeaponFactory weaponfactory = WeaponFactory.Instance;

    private bool isFiring = false;
    public bool IsFiring
    {
        get
        {
            return isFiring;
        }

        private set
        {
            isFiring = value;
        }
    }
    private bool isActive = false;
    public bool IsActive
    {
        get
        {
            return isActive;
        }

        private set
        {
            isActive = value;
        }
    }

    public void SetWeapon(int Id)
    {
        Debug.Log(this.weaponReference);
        this.weaponReference = WeaponFactory.Instance.GetReference(Id);
        Debug.Log(this.weaponReference);
        this.IsActive = true;
    }

    public void FireWeapon()
    {
        if (!IsFiring && IsActive)
        {
            this.StartCoroutine(FireWeaponAction());
        }
    }

    public override void Update()
    {
        base.Update();
    }

    private IEnumerator FireWeaponAction()
    {
        // set is firing to true
        this.IsFiring = true;
        Debug.Log(this.weaponReference);
        int clip = this.weaponReference.ClipSize;


        while (clip > 0)
        {
            // fire shot
            // wait for shot delay
            // if clip is not empty repeat
            --clip;
            level.KillZombies(this.weaponReference.bulletReference.ZombieKillNumber);
            yield return new WaitForSeconds(this.weaponReference.ShotDelay);
        }

        // if clip is empty, wait for reload delay to finish
        // when reload delay is finished, set is firing to false
        yield return new WaitForSeconds(this.weaponReference.ReloadDelay);
        this.IsFiring = false;
        yield return 0;
    }

    public static GameObject SpawnWeaponButton(JDWeapon weapon)
    {
        return (GameObject)Instantiate(Resources.Load(weapon.ResourceName));
    }
}
