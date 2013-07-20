using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Linq;

using Object = UnityEngine.Object;
using Random = System.Random;
using System.Collections.Generic;

public class LevelManager : JDISavableObject
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


        levelProgression = levelProgression.OrderBy(level => level.Id).ToList();
    }

    public bool IsCurrentLevelDefined { get { return this.currentLevel != null; } }

    public int CurrentLevel()
    {
        if (this.IsCurrentLevelDefined)
        {
            return this.currentLevel.Id;
        }
        
        return 0;
    }
    public float CurrentZombieLimit()
    {
        if (this.IsCurrentLevelDefined)
        {
            return this.currentLevel.ZombieLimit;
        }

        return 0;
    }
    public float CurrentZombieRate()
    {
        if (this.IsCurrentLevelDefined)
        {
            return this.currentLevel.ZombieCollectRate;
        }

        return 0;
    }
    public string CurrentLevelName()
    {
        if (this.IsCurrentLevelDefined)
        {
            return this.currentLevel.Name;
        }

        return "";
    }
    public float CurrentZombieCount()
    {
        return this.currentZombieCount;
    }
    public int NumberOfLevels() 
    {
        return this.levelProgression.Count;
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

    public string SaveData()
    {
        return JDGameUtilz.SerializeObject(levelProgression, "LevelWaves", typeof(List<JDLevel>));
    }

    public void LoadData(string savefiletext)
    {
        int rootStart = savefiletext.IndexOf("<LevelWaves>");
        int rootEnd = savefiletext.IndexOf("</LevelWaves>") + "</LevelWaves>".Length;
        string partialText = savefiletext.Substring(rootStart, rootEnd - rootStart);
        levelProgression = (List<JDLevel>)JDGameUtilz.DeserializeObject(partialText, "LevelWaves", typeof(List<JDLevel>), JDGameUtilz.EncodingType.UTF8);
    }
}
