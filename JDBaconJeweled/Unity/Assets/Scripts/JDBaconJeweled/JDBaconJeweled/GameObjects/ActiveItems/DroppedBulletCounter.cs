﻿using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Object = UnityEngine.Object;
using Random = System.Random;

public class DroppedBulletCounter : JDMonoGuiBehavior
{
    public static DroppedBulletCounter Instance
    {
        get
        {
            if (instance == null)
            {
                instance = JDGame.GameMaster.GetComponent<DroppedBulletCounter>();
            }

            return instance;
        }
    }
    static DroppedBulletCounter instance;

    private List<FallingBullet> droppedBullets = new List<FallingBullet>();

    // bullets to be counted then dropped.
    public void AddBullets(List<FallingBullet> bullets)
    {
        this.droppedBullets.AddRange(bullets);
        var bulletBags = from bullet in bullets
                            group bullet by bullet.BulletReference.Name into bb
                            let count = bb.Count()
                            select new { 
                                Name = bb.Key,
                                Count = (int)Math.Floor(count / 3d) + (count % 3)
                            };

        foreach (var bulletBag in bulletBags)
        {
            if (GameStatistics.Instance.GetStatistic(bulletBag.Name) == -1)
            {
                GameStatistics.Instance.CreateStatistic(bulletBag.Name, 0);
            }

            GameStatistics.Instance.UpdateStatistic(bulletBag.Name, bulletBag.Count);
        }
    }

    public override void Awake()
    {
        base.Awake();

        DebugCommands.Instance.AddCommand(new ConsoleCommand("PrintStats", "Prints the game statistics", Debug_PrintStatistics));
    }

    public override void Update()
    {
        base.Update();

        if (this.droppedBullets.Count > 0)
        {
            for (int count = 0; count < this.droppedBullets.Count(); ++count)
            {
                FallingBullet bullet = this.droppedBullets[count];
                bullet.gameObject.transform.position = bullet.gameObject.transform.parent.transform.position;
                bullet.gameObject.layer = 9;
            }

            this.droppedBullets.Clear();
        }

    }

    public void Debug_PrintStatistics(string[] Params)
    {
        int index = 0, value = 0;
        string statisticName = "";
        string debugOut = "";
        statisticName = GameStatistics.Instance.GetStatisticNameByIndex(index);
        
        while (statisticName != "")
        {
            value = GameStatistics.Instance.GetStatisticValueByIndex(index);
            debugOut += statisticName + ": " + value + "\n";

            ++index;
            statisticName = GameStatistics.Instance.GetStatisticNameByIndex(index);
        }

        Debug.Log(debugOut);
    }
}