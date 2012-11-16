using UnityEngine;
using System;
using System.Collections;

using Object = UnityEngine.Object;
using System.Collections.Generic;

[Serializable]
public class MainGameplayHUD : MonoBehaviour
{
    public JDPlayerController player;
    protected JDHeroCharacter PlayerReference;
    bool isPaused = false;
    float timescale;

    // Items to appear in the equiped Box
    // TODO: change this to Check the player for equipped weapon
    public Texture2D BaconHealth;
    protected List<Texture2D> Weapons;
    public float previousTimeScale;
    private Texture2D WeaponSelected;
    // TODO: Change to correct iterator
    public int i = 0;
    
    // Items for shrinking the Bacon
    public float BaconWidth;
    public float BaconHeight;
    // Use this for initialization
    void Start()
    {
        PlayerReference = player.Hero;
        PlayerReference.HitPoints = 5;
        PlayerReference.MaxHitPoints = 5;
        BaconWidth = BaconHealth.width / PlayerReference.MaxHitPoints;
        BaconHeight = BaconHealth.height / PlayerReference.MaxHitPoints;
        Debug.Log("BaconHeight:" + BaconHeight);
        Weapons = new List<Texture2D>(Enum.GetValues(typeof(HeroWeaponIconType)).Length);
        Weapons.AddRange(PlayerReference.WeaponManager.WeaponsList.ConvertAll<Texture2D>(image => image.WeaponIconType.ToIconImageFile()));
    }

    // Update is called once per frame
    void Update()
    {
        WeaponSelected = Weapons[this.PlayerReference.WeaponManager.GetCurrentWeaponIndex()];
        if (Input.GetButtonDown("Pause"))
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
        GUI.Label(new Rect(50, 40, 100, 100), "Total Health: " + PlayerReference.MaxHitPoints);
        GUI.Label(new Rect(50, 50, 100, 100), "Health: " + PlayerReference.HitPoints);
        Rect rect = new Rect((Screen.width - BaconHealth.width) /2 , 10, BaconWidth * (PlayerReference.HitPoints), BaconHeight * (PlayerReference.HitPoints));
        GUI.DrawTexture(rect, BaconHealth);
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
