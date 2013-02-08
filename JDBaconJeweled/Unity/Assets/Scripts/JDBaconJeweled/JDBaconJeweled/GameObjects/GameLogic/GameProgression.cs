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
    public SwapTypes SwapType = SwapTypes.CLICK;
    public GameObject FrameSceneOrientation;
    public GameObject ShopSceneOrientation;
    private GameObjectToucher toucher;
    private Frame10x10 frame;
    
    // game starts unpaused, while everything else is paused.  this way, it is known that 
    //  the game has just started.  after this point, all control pausers will be kept in sync with 
    //  this variable.
    public bool GameIsPaused { get { return Time.timeScale == 0; } }

    private GameObject cameraGO;
    private GameObject Camera
    {
        get
        {
            if (this.cameraGO == null)
            {
                this.cameraGO = GameObject.Find("Main Camera");
            }

            return this.cameraGO;
        }
    }

    public override void Awake()
    {
        base.Awake();
        delay = 5;
        tick = 0;

        toucher = GameObjectToucher.Instance;
        frame = Frame10x10.Instance;

        GameStatistics.Instance.AllowedBulletStat = JDIStatTypes.INDIVIDUALS;
        toucher.DropGameObject += Instance_DropGameObject;
        toucher.PickUpGameObject += Instance_PickUpGameObject;
        //TransitionToScene(GameScenes.GUN_SHOP);
    }
    public override void Start()
    {
        base.Start();

        StartLevel();
    }


    void Instance_PickUpGameObject(GameObjectTransferEventArgs eventArgs)
    {
        if (/*we are in gameplay and */ SwapType == SwapTypes.CLICK)
        {
            var go = eventArgs.GameObject;
            var last = toucher.LastPickedUpGameObject;

            if (last != null && go != last)
            {
                frame.SwapBullets(go, last);
                toucher.ClearHistory();
            }
        }
    }
    void Instance_DropGameObject(GameObjectTransferEventArgs eventArgs)
    {
        if (/*we are in gameplay and */ SwapType == SwapTypes.DRAG_DROP)
        {
            var go = eventArgs.GameObject;
            var last = toucher.LastPickedUpGameObject;

            if (last != null && go != last)
            {
                frame.SwapBullets(go, last);
                toucher.ClearHistory();
            }
        }
    }

    private void TransitionToScene(GameScenes scene)
    {
        if (scene != this.currentScene)
        {
            this.currentScene = scene;
            this.Camera.transform.position = ShopSceneOrientation.transform.position;
            this.Camera.transform.rotation = ShopSceneOrientation.transform.rotation;
        }
    }

    private void StartLevel()
    {
        this.StartCoroutine(startTimerWhenFrameStable());
    }

    IEnumerator startTimerWhenFrameStable()
    {
        yield return new WaitForSeconds(1);
        while (!Frame10x10.Instance.IsFrameStable())
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(0.5f);
        ZombieTimer.Instance.StartTimerCycle();
        yield return 0;
    }

    private void EndLevel()
    {
    }

    private void SwitchToShop()
    {
        this.TransitionToScene(GameScenes.GUN_SHOP);
    }

    private void StepFrame()
    {
        var matches = frame.DropAnyMatches();
        frame.BubbleUpAndSpawn(matches);
    }

    private bool isFrameAble()
    {
        return frame != null && frame.IsFrameStable() && frame.HasMatches();
    }
    private bool timeToBeginFrame()
    {
        return !this.IsPaused && tick >= delay;
    }
    public override void Update()
    {
        base.Update();

        // replace this with whatever we deem the starting gun for this game.

        if (timeToBeginFrame() && isFrameAble())
        {
            tick = 0;
            StepFrame();
        }

        ++tick;
    }
}
