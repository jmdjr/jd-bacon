using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

using Object = UnityEngine.Object;
using System.Collections.Generic;

public sealed class BulletFactory
{
    string BulletDefinitionFile = "./Definitions/BulletDefinitions.xml";

    private static BulletFactory instance;

    public static BulletFactory Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new BulletFactory();
            }
            return instance;
        }
    }

    public List<JDBullet> Bullets;

    private BulletFactory()
    {
        Bullets = (List<JDBullet>)JDGameUtilz.DeserializeObject(JDGameUtilz.LoadXML(BulletDefinitionFile),
            "bulletDefinitions", typeof(List<JDBullet>), JDGameUtilz.EncodingType.UTF8);

    }

}