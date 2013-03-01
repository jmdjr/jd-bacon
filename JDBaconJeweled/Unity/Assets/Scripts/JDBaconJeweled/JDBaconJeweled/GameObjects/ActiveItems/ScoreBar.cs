using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Object = UnityEngine.Object;
using Random = System.Random;

public class ScoreBar : JDMonoBehavior
{
    public int NumberOfLeadingZeros = 7;
    static ScoreBar instance;
    public static ScoreBar Instance
    {
        get
        {
            if (instance == null)
            {
                if (GameObject.Find("Score Bar") != null)
                {
                    instance = GameObject.Find("Score Bar").GetComponent<ScoreBar>();
                }
                else
                {
                    Debug.Log("Can't find Score Bar GameObject!");
                }
            }

            return instance;
        }
    }

    DynamicText dText = null;
    GameStatistics stats = null;
    LevelManager level = null;

    public override void Awake()
    {
        base.Awake();

        dText = DynamicText.GetTextMesh(this);
        stats = GameStatistics.Instance;
        level = LevelManager.Instance;
    }

    public override void Update()
    {
        base.Update();
        string currentLevelScoreStat = GameStatistics.SubGroup(level.CurrentLevelName(), "Score");
        int score = stats.GetStatistic(currentLevelScoreStat);
        SetScore(score == -1 ? 0 : score);
    }

    public void SetScore(int score)
    {
        int length = score.ToString("D").Length + NumberOfLeadingZeros;
        SetScoreText( score.ToString("D" + length.ToString()) );
    }

    public void SetScoreText(string text)
    {
        this.dText.SetText(text);
    }
}
