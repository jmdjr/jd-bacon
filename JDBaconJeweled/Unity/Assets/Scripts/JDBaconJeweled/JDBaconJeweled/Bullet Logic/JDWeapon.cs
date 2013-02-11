using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

using Object = UnityEngine.Object;
using System.Collections.Generic;

public class JDWeapon : JDIObject
{
    public bool Unlocked { get; set; }
    public string Name { get; set; }
    public int Id { get; set; }
    public int AccessedLevel { get; set; }
    public int ClipSize { get; set; }
    public float ShotDelay { get; set; }
    public float ReloadDelay { get; set; }
    public string ResourceName { get; set; }

    public JDIObjectTypes JDType { get { return JDIObjectTypes.WEAPON; } }

    public bool ReportStatistics(JDIStatTypes stat, int valueShift)
    {
        return false;
    }
}
