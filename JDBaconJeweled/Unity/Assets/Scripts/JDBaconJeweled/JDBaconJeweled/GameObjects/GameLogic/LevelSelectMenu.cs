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
        foreach (JDMenuButton button in this.menuButtons.Values)
        {
            int levelIndex = 0;

            try
            {
                levelIndex = int.Parse(button.name);
            }
            catch (Exception)
            {
            }

            if (levelIndex > 0)
            {
                if (levelIndex > level.NumberOfLevels())
                {
                    button.renderer.enabled = false;
                }
                else
                {
                    button.renderer.enabled = true;
                    button.OnClick += new EventHandler((o, e) =>
                    {
                        this.StartCoroutine(ButtonClickToLevel(levelIndex));
                    });
                }
            }
        }
    }

    public IEnumerator ButtonClickToLevel(int idx)
    {
        level.GotoLevel(idx - 1);
        
        MenuNavigator.Instance.SwitchToMenu(MenuNavigator.Instance.GetMenu("GamePlay"));
        yield return 0;
    }
}