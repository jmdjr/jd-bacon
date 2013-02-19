using UnityEngine;
using System;
using System.Collections;

using Object = UnityEngine.Object;
using System.Collections.Generic;

public class BulletGameGlobal
{

    private static BulletGameGlobal instance;
    public static BulletGameGlobal Instance
    {
        get 
        {
            if (instance == null)
            {
                instance = new BulletGameGlobal();
            }
            
            return instance;
        }
    }

    private BulletGameGlobal()
    {
        DebugCommands.Instance.AddCommand(new ConsoleCommand("GlobalSetFlag", "Sets global variables from the BulletGameGlobal context", BulletGameGlobal_Debug_GlobalSet));
    }

    public bool PreventBulletBouncing { get; set; }
    public bool PauseSpawners { get; set; }
    public bool PauseFrame { get; set; }

    private void BulletGameGlobal_Debug_GlobalSet(string[] Params)
    {
        //Debug.Log(Params.Length);
        if(Params.Length == 0 || DebugCommands.IsHelpSwitch(Params[0]))
        {
            Debug.Log(
                "GlobalSetFlag [/?][/x:PropertySwitch] T|F \n"
                + "Sets the value of some global flags that control the game mechanics.\n"
                + "Property Switches: \n"
                + "/B  Prevent Bullets from bouncing. \n"
                + "/S  Pauses the Spawners, essentially preventing the game from continuing.\n"
                );
            return;
        }

        if (Params.Length > 2 || (Params[1] != "T" && Params[1] != "F"))
        {
            Debug.LogWarning("GlobalSetFlag command improperly formatted.");
            return;
        }

        string switchCommand = Params[0];
        bool switchValue = Params[1] == "T";
        

        switch (switchCommand)
        {
            case "/B":
            case "/b":
                this.PreventBulletBouncing = switchValue;
                break;
            case "/S":
            case "/s":
                this.PauseSpawners = switchValue;
                break;
        }
    }
}
