using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Object = UnityEngine.Object;
using Random = System.Random;
using System.IO;

public class MainMenu : JDMenu
{
    public override void MenuUpdate()
    {
    }

    public override void AssignButtonMenus()
    {
        if (this.menuButtons["Exit Game"] != null)
        {
            // should have a confirmation menu to ask if they really want to quit, maybe save too.
        }

        if (this.menuButtons["Options"] != null)
        {
            this.menuButtons["Options"].AssignMenu(navigator.GetMenu("Options Menu"));
        }
        if (this.menuButtons["Start Game"] != null)
        {
            this.menuButtons["Start Game"].AssignMenu(navigator.GetMenu("Level Select Menu"));
        }
        if (this.menuButtons["Load Game"] != null)
        {
            this.menuButtons["Load Game"].AssignMenu(navigator.GetMenu("Load Game Menu"));
        }
    }
}
