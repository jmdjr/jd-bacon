using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Object = UnityEngine.Object;
using Random = System.Random;

public class MenuNavigator : JDMonoGuiBehavior
{
    static MenuNavigator instance;
    public static MenuNavigator Instance
    {
        get
        {
            if (instance == null)
            {
                var go = GameObject.Find("Menus");
                
                if (go != null)
                {
                    go.GetComponent<MenuNavigator>();
                }
            }

            return instance;
        }
    }
}
