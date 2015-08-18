using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(PlayerController))]
public class Player : Character
{
    private Weapon mPlayerWeapon;
    public Weapon PlayerWeapon { get { return mPlayerWeapon; } }

    // On creation of this item
    void Start()
    {
        this.mHealth = 5;
        this.mMaxHealth = 5;
    }
    // Update is called once per frame
    void Update()
    {

    }

    // Display Character Health
    void OnGUI()
    {
        
    }

    //public void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.collider.transform.tag == "Enemy")
    //    {
    //        this.ChangeCurrentHealth(-1);
    //    }
    //}
}
