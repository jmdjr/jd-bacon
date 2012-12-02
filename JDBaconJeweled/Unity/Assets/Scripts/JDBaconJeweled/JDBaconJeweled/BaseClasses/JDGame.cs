using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

using Object = UnityEngine.Object;
using System.Collections.Generic;

public class JDGame
{
    private static GameObject gameMaster = null;
    public static GameObject GameMaster
    {
        get
        {
            if (gameMaster == null)
            {
                gameMaster = GameObject.Find("__GameMasterObject");
            }

            return gameMaster;
        }
    }

    private static GameObject levelMaster = null;
    public static GameObject LevelMaster
    {
        get 
        {
            if (levelMaster == null)
            {
                levelMaster = GameObject.Find("__LevelMasterObject");
            }

            return levelMaster;
        }
    }

    public static JDIObject GetJDIObject(JDMonoBodyBehavior script, JDIObjectTypes JDType)
    {
        return (JDIObject)script.JDCollection.Find(match => { return (match.JDType == JDType && match != (JDIObject)script); });
    }

}

