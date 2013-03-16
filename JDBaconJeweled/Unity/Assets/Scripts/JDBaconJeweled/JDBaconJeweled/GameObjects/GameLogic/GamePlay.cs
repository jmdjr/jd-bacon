using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Object = UnityEngine.Object;
using Random = System.Random;

public class GamePlay : JDMenu
{
    private int delay;
    private int tick;

    public SwapTypes SwapType = SwapTypes.CLICK;
    private Frame frame;
    private ZombieTimer timer;
    private LevelManager level;
    private GameStatistics stats;
    private ScoreBar score;
    public GameObject HoverEffectObject;

    public override void Awake()
    {
        base.Awake();
        delay = 5;
        tick = 0;

        frame = Frame.Instance;
        timer = ZombieTimer.Instance;
        stats = GameStatistics.Instance;
        level = LevelManager.Instance;
        score = ScoreBar.Instance;
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
            weapon.FireWeapon();
        }
    }
    void toucher_DropGameObject(GameObjectTransferEventArgs eventArgs)
    {
        var go = eventArgs.GameObject;
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

    public void StartLevel()
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

        List<string> statNames = new List<string>();
        foreach (var bullet in frame.BulletGroups)
        {
            statNames.Add(bullet.ManualName);
        }
        statNames.Add(StatisticsEnum.Cash.ToString());
        statNames.Add(StatisticsEnum.Score.ToString());

        stats.EstablishGroup(level.CurrentLevelName(), statNames.ToArray());

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

    void frame_ScriptUpdate(MonoScriptEventArgs eventArgs)
    {
        frame.GridUpdateAction();
    }
    public override void MenuEnter()
    {
        this.StartLevel();
        base.MenuEnter();
    }
    public override void MenuUpdate()
    {
        score.SetScoreText(stats.SubGroup(level.CurrentLevelName(), "Score"));
        // replace this with whatever we deem the starting gun for this game.
        
        if (timeToBeginFrame() && isFrameAble())
        {
            tick = 0;
            StepFrame();
        }

        ++tick;
    }

    public override void RegisterTouchingEvents()
    {
        frame.InitializeFrame();
        frame.SpawnFrame();
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

    public override void AssignButtonMenus()
    {

    }
}
