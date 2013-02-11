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
                    instance = go.GetComponent<MenuNavigator>();
                }
            }

            return instance;
        }
    }
    private List<JDMenu> menus = new List<JDMenu>();

    public override void Awake()
    {
        base.Awake();

        var children = this.gameObject.GetComponentsInChildren<JDMenu>();

        foreach (var child in children)
        {
            this.menus.Add(child);
        }

        this.menus.First().BringToTopLayer();
    }
}
