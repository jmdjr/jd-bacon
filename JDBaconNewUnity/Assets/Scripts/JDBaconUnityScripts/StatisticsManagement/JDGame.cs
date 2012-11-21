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
                reaper = ((GrimReaper)GameMaster.GetComponent<GrimReaper>());
            }

            return reaper;
        }
    }

    public static JDCharacter GetCharacterFromCollider(Collider collider, JDCharacter Original)
    {
        try
        {
            GameObject go = collider.gameObject;

            JDMonoBodyBehavior registeredScript = go.GetComponent<JDMonoBodyBehavior>();
            JDCharacter character = (JDCharacter)registeredScript.JDCollection.Find(match => { return (match.JDType == JDIObjectTypes.CHARACTER && match != Original); });

            return character;
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// Returns the First Instance of a JDIObject of the type provided.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="gameObject"></param>
    /// <returns></returns>
    //public static JDIObject GetJDIObjectFromSelf<T>(GameObject gameObject) where T: JDIObject
    //{
    //}
}

