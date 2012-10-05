using UnityEngine;
using System;
using System.Collections;

using Object = UnityEngine.Object;
using System.Collections.Generic;

public class MainGameplayHUD : MonoBehaviour
{
    public JDHeroCharacter mPlayer;
    bool isPaused = false;
    float timescale;

    // Items to appear in the equiped Box
    // TODO: change this to Check the player for equipped weapon
    public Texture2D BaconSword;
    public Texture2D BaconShotgun;
    public Texture2D BaconMachineGun;
    public Texture2D BaconChainsaw;
    public Texture2D BaconHealth;
    public Texture2D WeaponSelected;
    public List<Texture2D> Weapons;
    public float previousTimeScale;
    // TODO: Change to correct iterator
    public int i = 0;
    
    // Items for shrinking the Bacon
    public float BaconWidth;
    public float BaconHeight;
    
    // Use this for initialization
    void Start()
    {
        mPlayer.HitPoints = 5;
        mPlayer.MaxHitPoints = 5;
        BaconWidth = BaconHealth.width / mPlayer.MaxHitPoints;
        BaconHeight = BaconHealth.height / mPlayer.MaxHitPoints;

        Weapons.Add(BaconSword);
        Weapons.Add(BaconShotgun);
        Weapons.Add(BaconMachineGun);
        Weapons.Add(BaconChainsaw);

        WeaponSelected = Weapons[i];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (i < Weapons.Count)
            {
                WeaponSelected = Weapons[i++];
            }
            else 
            { 
                i = 0;
                WeaponSelected = Weapons[i++];
            }
        }
        if (Input.GetButtonDown("Fire2"))
        {
            Debug.Log("Gameplay Paused");
            //PauseGameplay();
            if (Time.timeScale == 0)
            {
                Time.timeScale = previousTimeScale;
            }
            else
            {
                previousTimeScale = Time.timeScale;
                Time.timeScale = 0;
            }
        }
    }

    // Display Character Health
    void OnGUI()
    {
        GUI.Label(new Rect(50, 40, 100, 100), "Total Health: " + mPlayer.MaxHitPoints);
        GUI.Label(new Rect(50, 50, 100, 100), "Health: " + mPlayer.MaxHitPoints);
        // Draw Bacon Health Bar
        GUI.DrawTexture(new Rect((Screen.width - BaconHealth.width) / 2, 10, BaconWidth * (mPlayer.HitPoints), BaconHeight * (mPlayer.HitPoints)), BaconHealth);
        GUI.Box(new Rect(Screen.width / 20 * 2, Screen.height / 7 * 6, 100, 100), WeaponSelected);
    }

    public void PauseGameplay()
    {
        StateMachineSystem[] StateMachineRefrences = (StateMachineSystem[])FindObjectsOfType(typeof(StateMachineSystem));
        foreach (StateMachineSystem sms in StateMachineRefrences)
        {
            sms.PauseStateMachineSystem();
        }
    }
}
