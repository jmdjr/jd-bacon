using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PauseMenu : JDMenu
{
    public override void RegisterTouchingEvents()
    {
        base.RegisterTouchingEvents();
    }

    public override void UnregisterTouchingEvents()
    {
        base.UnregisterTouchingEvents();
    }

    public override void MenuUpdate()
    {
    }

    public override void AssignButtonMenus()
    {
        if (this.menuButtons["Exit Game"] != null)
        {
            this.menuButtons["Exit Game"].OnClick += ExitGame;
            this.menuButtons["Exit Game"].AssignMenu(navigator.GetMenu("Main Menu"));
        }
    }

    public void ExitGame(object sender, EventArgs args)
    {
        GamePlay gameplay = (GamePlay)navigator.GetMenu("GamePlay");
        gameplay.QuitLevel();
    }
}