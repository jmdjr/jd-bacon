using UnityEngine;
using System;
using System.Collections;

using Object = UnityEngine.Object;
using System.Collections.Generic;

public class JDGame
{
    private static GrimReaper reaper;
    public static GrimReaper GrimReaper
    {
        get
        {
            if(reaper == null)
            {
                GameObject LevelMaster = GameObject.Find("__LevelMasterObject");
                reaper = ((GrimReaper)LevelMaster.GetComponent<GrimReaper>());
            }

            return reaper;
        }
    }
}

