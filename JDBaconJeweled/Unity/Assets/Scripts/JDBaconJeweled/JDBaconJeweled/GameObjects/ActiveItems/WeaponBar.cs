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
    static WeaponBar instance;
    public static WeaponBar Instance
    {
        get
        {
            if (instance == null)
            {
                if (GameObject.Find("WeaponBar") != null)
                {
                    instance = GameObject.Find("WeaponBar").GetComponent<WeaponBar>();
                }
            }

            return instance;
        }
    }

    public List<Vector3> ButtonPositionReferences = new List<Vector3>(4);


}
