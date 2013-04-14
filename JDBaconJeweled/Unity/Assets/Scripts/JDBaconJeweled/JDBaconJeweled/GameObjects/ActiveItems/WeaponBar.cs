using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Linq;

using Object = UnityEngine.Object;
using Random = System.Random;
using System.Collections.Generic;

public class WeaponBar : JDMonoGuiBehavior
{
    private GameObject defaultWeapon = null;
    private GameObject emptyWeapon = null;
    private static List<WeaponButton> ActiveButtons = new List<WeaponButton>();
    private List<GameObject> attachedButtons = new List<GameObject>();
    private List<Vector3> Positions = new List<Vector3>();
    private bool isDirty = false;

    public override void Start()
    {
        // load from user save the progress/weapons assigned to what slots.
        base.Start();

        if (ActiveButtons.Count < 4)
        {
            defaultWeapon = WeaponButton.SpawnWeaponButton(0);
            emptyWeapon = WeaponButton.AttachWeaponButton((GameObject)Resources.Load("Game Prefabs/Buttons/Blank Weapon Square")).gameObject;


            //[Player status injection site: load player stats here for which weapons they had saved]
            ActiveButtons.Add(defaultWeapon.GetComponent<WeaponButton>());
            ActiveButtons.Add(emptyWeapon.GetComponent<WeaponButton>());
            ActiveButtons.Add(emptyWeapon.GetComponent<WeaponButton>());
            ActiveButtons.Add(emptyWeapon.GetComponent<WeaponButton>());
        }

        var child = this.transform.FindChild("Slot 1");
        Positions.Add(child.transform.position);
        GameObject.Destroy(child.gameObject);

        child = this.transform.FindChild("Slot 2");
        Positions.Add(child.transform.position);
        GameObject.Destroy(child.gameObject);

        child = this.transform.FindChild("Slot 3");
        Positions.Add(child.transform.position);
        GameObject.Destroy(child.gameObject);

        child = this.transform.FindChild("Slot 4");
        Positions.Add(child.transform.position);
        GameObject.Destroy(child.gameObject);

        this.isDirty = true;
    }

    // gets a weapon bar if it exists on this object.
    public static WeaponBar GetWeaponBar(JDMonoBehavior source)
    {
        var bar = source.GetComponentInChildren<WeaponBar>();
        return bar;
    }

    public List<GameObject> ActiveWeaponButtons { get { return attachedButtons.AsReadOnly().ToList(); } }

    public void SetWeaponButton(int slot, WeaponButton button)
    {
        WeaponButton prevButton = ActiveButtons[slot];

        if (button == null && prevButton != null)
        {
            ActiveButtons[slot] = emptyWeapon.GetComponent<WeaponButton>();
        }
        else
        {
            ActiveButtons[slot] = button;
        }

        if(prevButton == defaultWeapon || prevButton == emptyWeapon)
        {
            prevButton.transform.renderer.enabled = false;
        }
        else if(prevButton != null)
        {
            prevButton.Destroy();
        }

        this.isDirty = true;
    }

    public override void Update()
    {
        base.Update();

        if (this.isDirty)
        {
            this.isDirty = false;
            int minimumRun = this.Positions.Count < WeaponBar.ActiveButtons.Count ? this.Positions.Count : WeaponBar.ActiveButtons.Count;
            int idx = 0;

            while (idx < minimumRun)
            {
                if (WeaponBar.ActiveButtons[idx] != null)
                {
                    GameObject sittingGO = this.attachedButtons.FirstOrDefault(ab => ab.transform.position == Positions[idx]);
                    
                    if (sittingGO != null)
                    {
                        this.attachedButtons.Remove(sittingGO);
                        GameObject.Destroy(sittingGO);
                    }
                    WeaponButton NewButton = WeaponBar.ActiveButtons[idx];
                    GameObject instance = (GameObject)Instantiate(NewButton.gameObject, Positions[idx], this.transform.rotation);
                    instance.transform.parent = this.transform;
                    instance.GetComponent<WeaponButton>().SetWeapon(NewButton);

                    this.attachedButtons.Add(instance);
                }
                else
                {
                    Debug.LogError("Weapon bar ran into a null weapon button and/or position!");
                }

                ++idx;
            }
        }
    }

    // weapon bar will automatically update its visible weapons based on the actions taken by the player.  
}

