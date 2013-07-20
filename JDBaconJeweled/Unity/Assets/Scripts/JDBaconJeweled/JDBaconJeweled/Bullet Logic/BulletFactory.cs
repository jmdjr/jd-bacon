using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Object = UnityEngine.Object;
using Random = System.Random;

public sealed class BulletFactory : JDISavableObject
{
    string BulletDefinitionFile = "./Assets/Definitions/BulletDefinitions.xml";

    private List<JDBullet> BulletReferences;
    private List<JDBullet> BulletCollection;

    public string Name { get; set; }
    public JDIObjectTypes JDType { get { return JDIObjectTypes.OBJECT; } }
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

    public bool CanSpawnBullet(int bulletIndex)
    {
        if (bulletIndex < 0 || bulletIndex > NumberOfLoadedBullets)
        {
            return false;
        }

        return BulletReferences[bulletIndex].IsUnlocked();
    }
    public JDBullet SpawnBullet(int bulletIndex)
    {
        if (!CanSpawnBullet(bulletIndex))
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
        return false;
    }

    public void ResetStatistics()
    {
        foreach (JDBullet bullet in BulletReferences)
        {
            GameStatistics.Instance.SetStatistic(bullet.bulletDebugChar, 0);
        }
    }

    private string debugChars = "";
    public string Debug_DebugChars 
    {
        get 
        {
            if (debugChars == "")
            {
                foreach (var bullet in BulletReferences)
                {
                    debugChars += "\\" + bullet.bulletDebugChar;
                }
            }

            return debugChars;
        }
    }

    public JDBullet GetReference(int Id)
    {
        JDBullet reference = null;

        if (Id > 0 && Id <= this.BulletReferences.Max(w => w.Id))
        {
            reference = this.BulletReferences.FirstOrDefault(b => b.Id == Id);
        }

        return reference;
    }

    public string SaveData()
    {
        return JDGameUtilz.SerializeObject(BulletReferences, "bulletDefinitions", typeof(List<JDBullet>));
    }

    public void LoadData(string savefiletext)
    {
        int rootStart = savefiletext.IndexOf("<bulletDefinitions>");
        int rootEnd = savefiletext.IndexOf("</bulletDefinitions>") + "</bulletDefinitions>".Length;
        string partialText = savefiletext.Substring(rootStart, rootEnd - rootStart);
        BulletReferences = (List<JDBullet>)JDGameUtilz.DeserializeObject(partialText, "bulletDefinitions", typeof(List<JDBullet>), JDGameUtilz.EncodingType.UTF8);
    }
}