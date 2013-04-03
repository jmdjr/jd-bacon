using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Object = UnityEngine.Object;
using Random = System.Random;
public class LevelSelectMenu : JDMenu
{
    LevelManager level;
    public override void RegisterTouchingEvents()
    {
        base.RegisterTouchingEvents();
    }

    public override void UnregisterTouchingEvents()
    {
        base.UnregisterTouchingEvents();
    }
    public override void Awake()
    {
        base.Awake();

        level = LevelManager.Instance;
    }
    public override void MenuUpdate()
    {
    }

    public override void AssignButtonMenus()
    {
        for(int idx=0; idx < this.menuButtons.Count; ++idx)
        {

            JDMenuButton button = this.menuButtons.FirstOrDefault(pair => pair.Key.Contains(idx.ToString())).Value;

            if (idx > level.NumberOfLevels())
            {
                button.renderer.enabled = false;
            }
            else
            {
                button.renderer.enabled = true;
                button.OnClick += new EventHandler((o, e) =>
                {
                    level.GotoLevel(idx);
                });

                button.AssignMenu(MenuNavigator.Instance.GetMenu("GamePlay"));
            }
        }
    }
}