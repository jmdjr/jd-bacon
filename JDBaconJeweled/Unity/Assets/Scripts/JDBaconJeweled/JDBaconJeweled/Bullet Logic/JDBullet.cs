using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

using Object = UnityEngine.Object;
using System.Collections.Generic;

[Serializable]
public class JDBullet : JDIObject
{
    #region Properties
    [SerializeField]
    private int id;
    [SerializeField]
    private string name;
    [SerializeField]
    private string bulletType;

    public string Name { get { return this.name; } set { this.name = value; } }
    public string BulletTypeString { get { return this.bulletType; } set { this.bulletType = value; } }
    public int Id { get { return this.id; } set { this.id = value; } }
    public string bulletDebugChar { get; set; }

    //public bool Unlocked { get; set; }
    //public int PointCost { get; set; }
    //public int ZombieDamage { get; set; }

    public JDIObjectTypes JDType
    {
        get { return JDIObjectTypes.OBJECT; }
    }

    public ConsoleColor Debug_Color { get; set; }

    public JDIBulletTypes BulletType
    {
        get { return TagTypeExtension.ToBulletTagType(BulletTypeString); }
    }

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
            name = this.name
            , id = this.id
            , bulletType = this.bulletType
            , bulletDebugChar = this.bulletDebugChar
            , Debug_Color = this.Debug_Color
            //, Unlocked = this.Unlocked
            //, PointCost = this.PointCost
            //, ZombieDamage = this.ZombieDamage
        };
    }
}