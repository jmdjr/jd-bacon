using UnityEngine;
using System;
using System.Collections;
using SmoothMoves;

[Serializable]
public class JDWeapon : JDIWeapon
{
    [SerializeField]
    private int damageAmount;
    [SerializeField]
    private bool isActive;
    [SerializeField]
    private int cooldownTime;
    [SerializeField]
    private string name;

    public int DamageAmount { get { return this.damageAmount; } set { this.damageAmount = value; } }
    public bool IsActive { get { return this.isActive; } set { this.isActive = value; } }
    public int CooldownTime { get { return this.cooldownTime; } set { this.cooldownTime = value; } }
    public string Name { get { return this.name; } set { this.name = value; } }
    public JDIObjectTypes JDType { get { return JDIObjectTypes.WEAPON; } }

    public bool ReportStatistics(JDIStatTypes stat, int valueShift)
    {
        return true;
    }
}