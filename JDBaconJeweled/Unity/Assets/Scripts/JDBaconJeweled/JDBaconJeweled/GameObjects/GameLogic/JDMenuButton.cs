using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Object = UnityEngine.Object;
using Random = System.Random;

[RequireComponent(typeof(BoxCollider))]
// JD Menu Buttons are buttons which are housed under "Buttons" game object groups.  menu buttons sole purpose is to navigate to other menus.
public class JDMenuButton : JDMonoGuiBehavior
{
    JDMenu ToMenu = null;
    
    public void AssignMenu(JDMenu menuToNavigate)
    {
        ToMenu = menuToNavigate;
    }

    public void GoToAssignedMenu()
    {
        if (this.gameObject.name == "Back Button")
        {
            MenuNavigator.Instance.GoBack();
        }
        else if (ToMenu != null)
        {
            MenuNavigator.Instance.SwitchToMenu(this.ToMenu);
        }
    }
}
