using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Linq;

using Object = UnityEngine.Object;
using Random = System.Random;
using System.Collections.Generic;

public class LevelManager : JDIObject
{
    //string LevelWavesDefinitionFile = "../Unity/Assets/Definitions/LevelWaves.xml";
    string LevelWavesDefinitionFile = "./Assets/Definitions/LevelWaves.xml";

    public string Name { get { return "LevelManager"; } set { } }
    public JDIObjectTypes JDType { get { return JDIObjectTypes.OBJECT; } }

    private List<JDLevel> levelProgression;
    private JDLevel currentLevel;

    private int zombiesKilled;
    private int currentZombieCount;

    private static LevelManager instance;
    public static LevelManager Instance 
    {
        get 
        {
            if (instance == null)
            {
                instance = new LevelManager();
            }

            return instance;
        }
    }

    private LevelManager()
    {
        levelProgression = new List<JDLevel>();

        levelProgression = (List<JDLevel>)JDGameUtilz.DeserializeObject(JDGameUtilz.LoadXML(LevelWavesDefinitionFile),
            "LevelWaves", typeof(List<JDLevel>), JDGameUtilz.EncodingType.UTF8);

        //string Serial = JDGameUtilz.SerializeObject(levelProgression, "LevelWaves", typeof(List<JDLevel>));

        levelProgression = levelProgression.OrderBy(level => level.Id).ToList();
        GotoLevel(0);
    }
    
    public int CurrentLevel()
    {
        return this.currentLevel.Id;
    }
    public int CurrentZombieLimit()
    {
        return this.currentLevel.ZombieLimit;
    }
    public int CurrentZombieRate()
    {
        return this.currentLevel.ZombieCollectRate;
    }
    public string CurrentLevelName()
    {
        return this.currentLevel.Name;
    }
    public int CurrentZombieCount()
    {
        return this.currentZombieCount;
    }


    public void GotoLevel(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < levelProgression.Count)
        {
            this.currentLevel = levelProgression[levelIndex];
            zombiesKilled = 0;
            currentZombieCount = 0;
        }
    }

    public void KillZombies(int amountToKill = 0)
    {
        if (amountToKill < 0)
        {
            return;
        }

        zombiesKilled += amountToKill;
        currentZombieCount -= amountToKill;
    }

    public void StepZombieCount()
    {
        currentZombieCount += this.CurrentZombieRate();
    }

    public bool ReportStatistics(JDIStatTypes stat, int valueShift)
    {
        throw new NotImplementedException();
    }
}
