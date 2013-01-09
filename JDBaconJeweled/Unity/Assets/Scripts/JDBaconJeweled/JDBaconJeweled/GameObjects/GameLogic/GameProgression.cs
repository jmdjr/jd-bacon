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
    private int delay;
    private int tick;

    public override void Awake()
    {
        base.Awake();
        delay = 5;
        tick = 0;
        GameStatistics.Instance.AllowedBulletStat = JDIStatTypes.INDIVIDUALS;
        GameObjectGrabber.Instance.DroppedGameObject += new GameObjectTransferEvent(GameObjectGrabber_DroppedGameObject);
    }

    private void GameObjectGrabber_DroppedGameObject(GameObjectTransferEventArgs eventArgs)
    {
        GameObject held = GameObjectGrabber.Instance.HeldGameObject;
        Frame10x10.Instance.SwapBullets(held, eventArgs.GameObject);
    }
    public override void Update()
    {
        base.Update();

        if (Time.timeScale > 0f && tick >= delay && Frame10x10.Instance != null && Frame10x10.Instance.IsFrameStable() && Frame10x10.Instance.HasMatches())
        {
            tick = 0;

            var matches = Frame10x10.Instance.DropAnyMatches();
            Frame10x10.Instance.BubbleUpAndSpawn(matches);
        }

        ++tick;
    }

}
