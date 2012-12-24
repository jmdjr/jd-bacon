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

    public override void Awake()
    {
        base.Awake();
        GameStatistics.Instance.AllowedBulletStat = JDIStatTypes.INDIVIDUALS;
    }

    public override void Update()
    {
        base.Update();

        if (GameFrame != null)
        {
            if (GameFrame.IsFrameStable() && GameFrame.HasMatches())
            {
                var matches = GameFrame.DropAnyMatches();
                GameFrame.BubbleUpAndSpawn(matches);
            }
        }
    }

}
