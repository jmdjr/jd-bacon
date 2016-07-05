using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

public static class TagTypeExtension 
{
    public static string ToTypeString(this TagTypes tag)
    {
        switch (tag)
        {
            case TagTypes.COLLECTABLE:
                return "Collectable";
            case TagTypes.ENEMY:
                return "Enemy";
            case TagTypes.EVENTTRIGGER:
                return "EventTrigger";
            case TagTypes.LEVELTERRAIN:
                return "LevelTerrain";
            case TagTypes.PLAYER:
                return "Player";
            default:
            case TagTypes.UNTAGGED:
                return "Untagged";
        }
    }
    public static TagTypes ToTagType(string tagString)
    {
        switch (tagString)
        {
            case "Collectable":
                return  TagTypes.COLLECTABLE;
            case "Enemy":
                return TagTypes.ENEMY;
            case "EventTrigger":
                return TagTypes.EVENTTRIGGER;
            case "LevelTerrain":
                return TagTypes.LEVELTERRAIN;
            case "Player":
                return TagTypes.PLAYER;
            default:
            case "Untagged":
                return TagTypes.UNTAGGED;
        }
    }
}