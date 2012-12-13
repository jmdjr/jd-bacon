using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

using Object = UnityEngine.Object;
using Random = System.Random;
using System.Collections.Generic;

public sealed class BulletFactory : JDIObject
{
    string BulletDefinitionFile = "../Unity/Assets/Definitions/BulletDefinitions.xml";

    private List<JDBullet> BulletReferences;
    private List<JDBullet> BulletCollection;

    public string Name
    {
        get
        {
            throw new NotImplementedException();
        }
        set
        {
            throw new NotImplementedException();
        }
    }
    public JDIObjectTypes JDType
    {
        get { return JDIObjectTypes.OBJECT; }
    }
    public static int NumberOfLoadedBullets { get { return Instance.BulletReferences.Count; } }

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

    public JDBullet SpawnBullet(int bulletIndex)
    {
        if (bulletIndex < 0 || bulletIndex > NumberOfLoadedBullets)
        {
            return null;
        }

        JDBullet bullet = BulletReferences[bulletIndex].SpawnCopy();
        bullet.ReportStatistics(JDIStatTypes.UNIQUES, 0);
        BulletCollection.Add(bullet);

        return bullet;
    }

    public void DestroyBullet(JDBullet bulletReference)
    {
        if (this.BulletCollection.Contains(bulletReference))
        {
            bulletReference.ReportStatistics(JDIStatTypes.INDIVIDUALS, 1);
            this.BulletCollection.Remove(bulletReference);
        }
    }

    public bool ReportStatistics(JDIStatTypes stat, int valueShift)
    {
        return true;
    }

    public void ResetStatistics()
    {
        foreach (JDBullet bullet in BulletReferences)
        {
            GameStatistics.Instance.CreateStatistic(bullet.bulletDebugChar, 0);
        }
    }

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
}