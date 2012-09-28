using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System;
using System.Collections;

using Object = UnityEngine.Object;
using System.Collections.Generic;

public class JDMeleeWeapon : JDIWeapon
{
    [SerializeField]
    private int damageAmount;
    public int DamageAmount
    {
        get
        {
            return damageAmount;
        }
        set
        {
            damageAmount = value;
        }
    }

    [SerializeField]
    bool isActive;
    public bool IsActive
    {
        get
        {
            return isActive;
        }
        set
        {
            isActive = value;
        }
    }

    [SerializeField]
    private int cooldownTime;
    public int CooldownTime
    {
        get
        {
            return cooldownTime;
        }
        set
        {
            this.cooldownTime = value;
        }
    }

    [SerializeField]
    private string name;
    public string Name
    {
        get
        {
            return name;
        }
        set
        {
            name = value;
        }
    }

    public JDIObjectTypes JDType
    {
        get { return JDIObjectTypes.WEAPON; }
    }

    public bool ReportStatistics(JDIStatTypes stat, int valueShift)
    {
        return true;
    }
}