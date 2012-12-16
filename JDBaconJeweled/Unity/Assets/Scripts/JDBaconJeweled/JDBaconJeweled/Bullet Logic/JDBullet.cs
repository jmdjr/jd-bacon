﻿using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

using Object = UnityEngine.Object;
using System.Collections.Generic;

[Serializable]
public class JDBullet : JDIObject
{
    #region Properties
    public bool Unlocked { get; set; }
    public string Name { get; set; }
    public int Id { get; set; }
    public string bulletDebugChar { get; set; }

    public JDIObjectTypes JDType
    {
        get { return JDIObjectTypes.OBJECT; }
    }

    public ConsoleColor Debug_Color { get; set; }

    #endregion Properties

    public bool ReportStatistics(JDIStatTypes stat, int valueShift)
    {
        if (stat == GameStatistics.Instance.AllowedBulletStat)
        {
            if (GameStatistics.Instance.GetStatistic(this.bulletDebugChar) == -1)
            {
                GameStatistics.Instance.CreateStatistic(this.bulletDebugChar, valueShift);
            }
            else
            {
                GameStatistics.Instance.UpdateStatistic(this.bulletDebugChar, valueShift);
            }
        }

        return true;
    }

    public JDBullet SpawnCopy() 
    {
        return new JDBullet()
        {
            Name = this.Name
            , Id = this.Id
            , bulletDebugChar = this.bulletDebugChar
            , Debug_Color = this.Debug_Color
            , Unlocked = this.Unlocked
            //, PointCost = this.PointCost
            //, ZombieDamage = this.ZombieDamage
        };
    }
}