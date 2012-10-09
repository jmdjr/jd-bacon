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

    private static GrimReaper reaper;
    public static GrimReaper GrimReaper
    {
        get
        {
            if(reaper == null)
            {
                reaper = ((GrimReaper)LevelMaster.GetComponent<GrimReaper>());
            }

            return reaper;
        }
    }

    public static JDCharacter GetCharacterFromCollider(Collider collider)
    {
        try
        {
            GameObject go = collider.gameObject;

            JDMonoBodyBehavior registeredScript = go.GetComponent<JDMonoBodyBehavior>();
            JDCharacter character = (JDCharacter)registeredScript.JDCollection.Find(match => { return match.JDType == JDIObjectTypes.CHARACTER; });

            return character;
        }
        catch (Exception)
        {
            return null;
        }
    }
    
}

