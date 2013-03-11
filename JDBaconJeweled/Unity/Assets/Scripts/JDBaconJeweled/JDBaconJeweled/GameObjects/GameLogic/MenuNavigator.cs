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
    private Stack<JDMenu> history = new Stack<JDMenu>();

    public JDMenu GetMenu(string menuName)
    {
        return menus.FirstOrDefault(m => { return m.gameObject.name == menuName; });
    }

    private void SendAllMenusToBack()
    {
        foreach (var menu in menus)
        {
            menu.SendToBackLayer();
        }
    }
    public override void Awake()
    {
        base.Awake();

        var children = this.gameObject.GetComponentsInChildren<JDMenu>();

        foreach (var child in children)
        {
            Debug.Log("Menus: Added " + child.gameObject.name);
            this.menus.Add(child);
        }

    }

    public override void Start()
    {
        base.Start();

        SwitchToMenu(GetMenu("Main Menu"));
    }

    public void SwitchToMenu(JDMenu menu, bool saveMenu = true)
    {
        if (menu != null)
        {
            JDMenu topMenu = menus.FirstOrDefault(m => m.IsTopLevel);
            
            if (topMenu != null)
            {
                topMenu.SendToBackLayer();
                if (saveMenu)
                {
                    history.Push(topMenu);
                }
            }

            menu.BringToTopLayer();
        }
    }

    public void GoBack()
    {
        SwitchToMenu(history.Pop(), false);
    }
}
