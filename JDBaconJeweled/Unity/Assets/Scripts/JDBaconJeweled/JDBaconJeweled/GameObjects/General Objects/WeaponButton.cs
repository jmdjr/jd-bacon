using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Object = UnityEngine.Object;
using Random = System.Random;

[RequireComponent(typeof(BoxCollider))]
public class WeaponButton : JDMonoGuiBehavior
{
    JDWeapon weaponReference = null;
    LevelManager level = null;
    GameStatistics stats = null;
    public bool IsFiring
    {
        get;

        private set;
    }
    public bool IsActive
    {
        get;

        private set;
    }

    private int availableClips
    {
        get 
        {
            int clips = 0;
            if (stats != null && weaponReference != null && weaponReference.bulletReference != null)
            {
                var bullet = weaponReference.bulletReference;
                clips = stats.GetStatistic(stats.SubGroup(level.CurrentLevelName(), bullet.Name));
            }

            return clips >= 0 ? clips : 0;
        }
    }

    public override void Awake()
    {
        base.Awake();

        level = LevelManager.Instance;
        stats = GameStatistics.Instance;
    }
    public void StopFiring()
    {
        this.IsFiring = false;
        this.StopCoroutine("FireWeaponAction");
    }

    private bool canFire()
    {
        return this.availableClips > 0;
    }
    private bool consumedClip()
    {
        if (canFire())
        {
            var bullet = weaponReference.bulletReference;
            stats.UpdateStatistic(stats.SubGroup(level.CurrentLevelName(), bullet.Name), -1);
            return true;
        }

        return false;
    }
    public void FireWeapon()
    {
        bool loadedGun = consumedClip();

        if (!loadedGun)
        {
            //pulse the clip or something.
        }

        if (!IsFiring && IsActive && loadedGun)
        {
            this.StartCoroutine(FireWeaponAction());
        }
    }
    private IEnumerator FireWeaponAction()
    {
        // set is firing to true
        this.IsFiring = true;
        int clip = this.weaponReference.ClipSize;
        while (clip > 0 && this.IsFiring)
        {
            // fire shot
            // wait for shot delay
            // if clip is not empty repeat
            --clip;
            level.KillZombies(this.weaponReference.bulletReference.ZombieKillNumber);
            yield return new WaitForSeconds(this.weaponReference.ShotDelay);
            yield return new WaitForEndOfFrame();
        }

        // if clip is empty, wait for reload delay to finish
        // when reload delay is finished, set is firing to false
        yield return new WaitForSeconds(this.weaponReference.ReloadDelay);
        this.IsFiring = false;
        yield return 0;
    }

    public static GameObject SpawnWeaponButton(int WeaponId)
    {
        JDWeapon weapon = WeaponFactory.Instance.AvailableWeapons()[WeaponId];
        return SpawnWeaponButton(weapon);
    }
    public static GameObject SpawnWeaponButton(JDWeapon weapon)
    {
        GameObject weaponButton = (GameObject)Resources.Load(weapon.ResourceName);
        AttachWeaponButton(weaponButton).weaponReference = weapon;
        return weaponButton;
    }
    public void SetWeapon(WeaponButton weaponReference)
    {
        this.weaponReference = weaponReference.weaponReference;
        this.IsActive = true;
    }
    public static WeaponButton AttachWeaponButton(GameObject weaponButton)
    {
        WeaponButton buttonScript = weaponButton.GetComponent<WeaponButton>();

        if (buttonScript == null)
        {
            buttonScript = weaponButton.AddComponent<WeaponButton>();
        }

        return buttonScript;
    }
}
