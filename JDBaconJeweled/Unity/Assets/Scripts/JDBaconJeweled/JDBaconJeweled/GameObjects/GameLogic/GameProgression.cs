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

    private GameScenes currentScene = GameScenes.BULLET_FRAME;
    
    public GameObject FrameSceneOrientation;
    public GameObject ShopSceneOrientation;

    private GameObject camera;
    private GameObject Camera
    {
        get
        {
            if (camera == null)
            {
                camera = GameObject.Find("Main Camera");
            }

            return camera;
        }
    }

    public override void Awake()
    {
        base.Awake();
        delay = 5;
        tick = 0;
        GameStatistics.Instance.AllowedBulletStat = JDIStatTypes.INDIVIDUALS;
        GameObjectGrabber.Instance.DroppedGameObject += new GameObjectTransferEvent(GameObjectGrabber_DroppedGameObject);
        //TransitionToScene(GameScenes.GUN_SHOP);
    }

    private void TransitionToScene(GameScenes scene)
    {
        if (scene != this.currentScene)
        {
            this.currentScene = scene;
            this.Camera.camera.enabled = false;

        }
    }

    private void StartLevel()
    {
    }

    private void StartTimer()
    {
    }

    private void EndLevel()
    {

    }

    private void StepFrame()
    {
        var matches = Frame10x10.Instance.DropAnyMatches();
        Frame10x10.Instance.BubbleUpAndSpawn(matches);
    }

    private void GameObjectGrabber_DroppedGameObject(GameObjectTransferEventArgs eventArgs)
    {
        GameObject held = GameObjectGrabber.Instance.HeldGameObject;
        Frame10x10.Instance.SwapBullets(held, eventArgs.GameObject);
    }

    private bool isFrameAble()
    {
        return Frame10x10.Instance != null && Frame10x10.Instance.IsFrameStable() && Frame10x10.Instance.HasMatches();
    }

    public override void Update()
    {
        base.Update();

        if (Time.timeScale > 0f && tick >= delay && isFrameAble())
        {
            tick = 0;
            StepFrame();
        }

        ++tick;
    }

}
