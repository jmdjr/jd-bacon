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
    string LevelWavesDefinitionFile = "./Assets/Definitions/LevelWaves.xml";

    public string Name { get { return "LevelManager"; } set { } }
    public JDIObjectTypes JDType { get { return JDIObjectTypes.OBJECT; } }

    private List<JDLevel> levelProgression;
    private JDLevel currentLevel;

    private float zombiesKilled;
    private float currentZombieCount;

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
    public float CurrentZombieLimit()
    {
        return this.currentLevel.ZombieLimit;
    }
    public float CurrentZombieRate()
    {
        return this.currentLevel.ZombieCollectRate;
    }
    public string CurrentLevelName()
    {
        return this.currentLevel.Name;
    }
    public float CurrentZombieCount()
    {
        return this.currentZombieCount;
    }

    public void GotoLevel(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < levelProgression.Count)
        {
            this.currentLevel = levelProgression[levelIndex];
            ResetLevel();
        }
    }
    public void FirstLevel()
    {
        this.GotoLevel(0);
    }

    public void NextLevel()
    {
        this.GotoLevel(levelProgression.IndexOf(this.currentLevel) + 1);
    }
    public bool OnLastLevel()
    {
        return levelProgression.IndexOf(this.currentLevel) == levelProgression.Count - 1;
    }
    public void ResetLevel()
    {
        zombiesKilled = 0;
        currentZombieCount = 0;
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
    public bool HasKilledEnoughZombies()
    {
        return this.CurrentZombieCount() < 0;
    }
    public bool ReportStatistics(JDIStatTypes stat, int valueShift)
    {
        throw new NotImplementedException();
    }
}
