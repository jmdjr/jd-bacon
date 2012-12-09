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

    public static JDIBulletTypes ToBulletTagType(string bulletString)
    {
        switch (bulletString)
        {
            default:
                return JDIBulletTypes.UNKOWN;

            case "BULLET_1":
                return JDIBulletTypes.BULLET_1;
            case "BULLET_2":
                return JDIBulletTypes.BULLET_2;
            case "BULLET_3":
                return JDIBulletTypes.BULLET_3;
            case "BULLET_4":
                return JDIBulletTypes.BULLET_4;
            case "BULLET_5":
                return JDIBulletTypes.BULLET_5;
            case "BULLET_6":
                return JDIBulletTypes.BULLET_6;
            case "BULLET_7":
                return JDIBulletTypes.BULLET_7;
            case "BULLET_8":
                return JDIBulletTypes.BULLET_8;
        }
    }
}