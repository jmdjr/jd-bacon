using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

using Object = UnityEngine.Object;
using System.Collections.Generic;

[Serializable]
public class JDBullet : IBullet
{
    [SerializeField]
    private int id;

    [SerializeField]
    private string name;

    [SerializeField]
    private string bulletType;

    public int Id { get { return this.id; } set { this.id = value; } }

    public string Name { get { return this.name; } set { this.name = value; } }

    public JDIObjectTypes JDType
    {
        get { return JDIObjectTypes.OBJECT; }
    }

    public string BulletTypeString { get { return this.bulletType; } set { this.bulletType = value; } }

    public JDIBulletTypes BulletType
    {
        get { return TagTypeExtension.ToBulletTagType(BulletTypeString); }
    }

    public bool ReportStatistics(JDIStatTypes stat, int valueShift)
    {
        throw new NotImplementedException();
    }

}