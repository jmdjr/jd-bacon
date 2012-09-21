using UnityEngine;
using System;
using System.Collections;

using Object = UnityEngine.Object;
using System.Collections.Generic;

public class MainGameplayHUD : MonoBehaviour
{
    public Player mPlayer;
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
    // TODO: Change to correct iterator
    public int i = 0;
    
    // Items for shrinking the Bacon
    public float BaconWidth;
    public float BaconHeight;
    
    // Use this for initialization
    void Start()
    {
        mPlayer.Health = 5;
        mPlayer.MaxHealth = 5;
        BaconWidth = BaconHealth.width / mPlayer.Health;
        BaconHeight = BaconHealth.height / mPlayer.Health;

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
    }

    // Display Character Health
    void OnGUI()
    {
        GUI.Label(new Rect(50, 40, 100, 100), "Total Health: " + mPlayer.MaxHealth);
        GUI.Label(new Rect(50, 50, 100, 100), "Health: " + mPlayer.Health);
        // Draw Bacon Health Bar
        GUI.DrawTexture(new Rect((Screen.width - BaconHealth.width) / 2, 10, BaconWidth * (mPlayer.Health), BaconHeight * (mPlayer.Health)), BaconHealth);
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
