using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(PlayerController))]
public class PlayerHealth : Health
{
    void Start()
    {
        this.SetTotalHealth(5);
    }

    // Display Character Health
    void OnGUI()
    {
        GUI.Label(new Rect(50, 50, 100, 100), "Health: " + this.currentHealth);
    }
}
