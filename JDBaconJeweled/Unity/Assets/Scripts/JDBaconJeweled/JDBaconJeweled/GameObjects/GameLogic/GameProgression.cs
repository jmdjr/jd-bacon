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
    
    // game starts unpaused, while everything else is paused.  this way, it is known that 
    //  the game has just started.  after this point, all control pausers will be kept in sync with 
    //  this variable.
    public bool GameIsPaused = false;

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
        GameStatistics.Instance.AllowedBulletStat = JDIStatTypes.INDIVIDUALS;
        GameObjectGrabber.Instance.DroppedGameObject += new GameObjectTransferEvent(GameObjectGrabber_DroppedGameObject);
        //TransitionToScene(GameScenes.GUN_SHOP);
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
        BulletGameGlobal.Instance.PauseFrame = false;
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

    private void PauseGameplay()
    {
        GameIsPaused = true;
        BulletGameGlobal.Instance.PauseFrame = true;
        ZombieTimer.Instance.PauseTimer();
        // add pausers for anything else in this game.
    }

    private void ResumeGameplay()
    {
        GameIsPaused = false;
        BulletGameGlobal.Instance.PauseFrame = false;
        ZombieTimer.Instance.ResumeTimer();
    }

    private bool IsGameStart()
    {
        // only test with one for certainty
        return !GameIsPaused && BulletGameGlobal.Instance.PauseFrame;
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
    private bool timeToBeginFrame()
    {
        return Time.timeScale > 0f && tick >= delay;
    }
    public override void Update()
    {
        base.Update();

        // replace this with whatever we deem the starting gun for this game.
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (IsGameStart())
            {
                StartLevel();
            }
            else if (!GameIsPaused)
            {
                this.PauseGameplay();
            }
            else if (GameIsPaused)
            {
                this.ResumeGameplay();
            }
        }

        if (timeToBeginFrame() && isFrameAble())
        {
            tick = 0;
            StepFrame();
        }

        ++tick;
    }
}
