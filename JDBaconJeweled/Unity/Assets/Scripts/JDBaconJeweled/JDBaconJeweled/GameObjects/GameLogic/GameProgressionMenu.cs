using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Object = UnityEngine.Object;
using Random = System.Random;

public class GameProgressionMenu : JDMenu
{
    private int delay;
    private int tick;

    public SwapTypes SwapType = SwapTypes.CLICK;
    private Frame frame;
    private ZombieTimer timer;
    public GameObject HoverEffectObject;

    public override void Awake()
    {
        base.Awake();
        delay = 5;
        tick = 0;

        frame = Frame.Instance;
        timer = ZombieTimer.Instance;

        GameStatistics.Instance.AllowedBulletStat = JDIStatTypes.INDIVIDUALS;
    }

    public override void Start()
    {
        base.Start();
        StartLevel();
    }

    void toucher_PickUpGameObject(GameObjectTransferEventArgs eventArgs)
    {
        var go = eventArgs.GameObject;

        // touching falling bullets.
        if (go.GetComponent<FallingBullet>() != null)
        {
            if (SwapType == SwapTypes.CLICK)
            {
                var last = toucher.LastPickedUpGameObject;

                if (last != null && go != last)
                {
                    frame.SwapBullets(go, last);
                    toucher.ClearHistory();
                }
            }
        }
        if (go.GetComponentInChildren<WeaponButton>() != null)
        {
            var weapon = go.GetComponentInChildren<WeaponButton>();

            // for now use the dummy weapon, when button is added to bar, it will already have weapon set.
            weapon.SetWeapon(1);
            weapon.FireWeapon();
        }
    }
    void toucher_DropGameObject(GameObjectTransferEventArgs eventArgs)
    {
        var go = eventArgs.GameObject;
        //Debug.Log("DropEvent");
        // touching falling bullets.
        if (go.GetComponent<FallingBullet>() != null)
        {
            if (SwapType == SwapTypes.DRAG_DROP)
            {
                var last = toucher.LastPickedUpGameObject;

                if (last != null && go != last)
                {
                    frame.SwapBullets(go, last);
                    toucher.ClearHistory();
                }
            }
        }
    }

    public void toucher_OverGameObject(GameObjectTransferEventArgs eventArgs)
    {
        var go = eventArgs.GameObject;
        var gos = go.GetComponent<FallingBullet>();
        //Debug.Log("HoverOver Event");
        // touching falling bullets.
        if (gos != null)
        {
            this.HoverEffectObject.gameObject.transform.position = gos.gameObject.transform.position;
        }
        else
        {
            this.HoverEffectObject.gameObject.transform.position = Vector3.zero;
        }
    }

    private void StartLevel()
    {
        this.StartCoroutine(startTimerWhenFrameStable());
    }

    IEnumerator startTimerWhenFrameStable()
    {
        yield return new WaitForSeconds(1);
        while (!frame.IsFrameStable())
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(0.5f);
        timer.StartTimerCycle();
        yield return 0;
    }

    private void EndLevel()
    {
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

    public override void MenuUpdate()
    {
        ScoreBar.Instance.SetScoreText(Time.frameCount.ToString());
        // replace this with whatever we deem the starting gun for this game.

        if (timeToBeginFrame() && isFrameAble())
        {
            tick = 0;
            StepFrame();
        }

        ++tick;
    }

    void frame_ScriptUpdate(MonoScriptEventArgs eventArgs)
    {
        frame.GridUpdateAction();
    }

    public override void RegisterTouchingEvents()
    {
        Debug.Log(toucher);
        frame.InitializeFrame();
        frame.ScriptUpdate += frame_ScriptUpdate;
        toucher.DropGameObject += toucher_DropGameObject;
        toucher.PickUpGameObject += toucher_PickUpGameObject;
        toucher.OverGameObject += toucher_OverGameObject;
    }


    public override void UnregisterTouchingEvents()
    {
        toucher.DropGameObject -= toucher_DropGameObject;
        toucher.PickUpGameObject -= toucher_PickUpGameObject;
        toucher.OverGameObject -= toucher_OverGameObject;
        frame.ScriptUpdate -= frame_ScriptUpdate;
    }
}
