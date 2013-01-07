using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Object = UnityEngine.Object;
using Random = System.Random;

public class GameProgression : JDMonoGuiBehavior
{
    Frame10x10 gameFrame;
    Frame10x10 GameFrame
    {
        get
        {
            if (gameFrame == null)
            {
                var g = GameObject.Find("Frame");

                if (g != null)
                {
                    gameFrame = g.GetComponent<Frame10x10>();
                }
            }

            return gameFrame;
        }
    }
    private int delay;
    private int tick;

    public override void Awake()
    {
        base.Awake();
        delay = 5;
        tick = 0;
        GameStatistics.Instance.AllowedBulletStat = JDIStatTypes.INDIVIDUALS;
    }

    public override void Update()
    {
        base.Update();

        if (Time.timeScale > 0f && tick >= delay && GameFrame != null && GameFrame.IsFrameStable() && GameFrame.HasMatches())
        {
            tick = 0;

            var matches = GameFrame.DropAnyMatches();
            GameFrame.BubbleUpAndSpawn(matches);
        }

        ++tick;
    }

}
