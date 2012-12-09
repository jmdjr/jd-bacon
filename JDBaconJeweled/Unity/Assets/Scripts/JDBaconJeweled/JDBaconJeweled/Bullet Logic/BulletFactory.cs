using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

using Object = UnityEngine.Object;
using Random = System.Random;
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

    public static int NumberOfLoadedBullets { get { return Instance.BulletReferences.Count; } }

    private List<JDBullet> BulletReferences;
    private List<JDBullet> BulletCollection;
    private BulletFactory()
    {
        BulletCollection = new List<JDBullet>();

        BulletReferences = (List<JDBullet>)JDGameUtilz.DeserializeObject(JDGameUtilz.LoadXML(BulletDefinitionFile),
            "bulletDefinitions", typeof(List<JDBullet>), JDGameUtilz.EncodingType.UTF8);

        Random r = new Random(0);

        foreach (JDBullet bullet in BulletReferences)
        {
            bullet.Debug_Color = (ConsoleColor)(r.Next(1, 15));
        }
    }

    public JDBullet SpawnBullet(int bulletIndex)
    {
        if (bulletIndex < 0 || bulletIndex > NumberOfLoadedBullets)
        {
            return null;
        }
        
        JDBullet bullet = BulletReferences[bulletIndex].SpawnCopy();
        BulletCollection.Add(bullet);

        return bullet;
    }

    public void DestroyBullet(JDBullet bulletReference)
    {
        if(this.BulletCollection.Contains(bulletReference))
        {
            this.BulletCollection.Remove(bulletReference);
        }
    }
}