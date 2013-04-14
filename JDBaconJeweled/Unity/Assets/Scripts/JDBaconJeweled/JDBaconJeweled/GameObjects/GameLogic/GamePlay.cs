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
    public GameObject HoverEffectObject;
    private JDMenu pausedMenu;
    private MenuNavigator navigate;

    private DynamicText LevelText;
    public override void Awake()
    {
        base.Awake();
        delay = 5;
        tick = 0;

        frame = Frame.Instance;
        timer = ZombieTimer.Instance;
        stats = GameStatistics.Instance;
        level = LevelManager.Instance;
        navigate = MenuNavigator.Instance;
        //[Player status injection site: load player stats here for which weapons they had saved]
    }
    public override void Start()
    {
        base.Start();
        GameObject level = GameObject.Find("LevelText");
        LevelText = DynamicText.GetTextMesh(level.transform.GetChild(0).gameObject);
        LevelText.SetText("");
        pausedMenu = navigate.GetMenu("Pause Menu");
    }
    public override void MenuEnter()
    {
        if (!this.IsPaused)
        {
            this.StartLevel();
        }
        else
        {
            this.UnPause();
        }

        base.MenuEnter();
    }
    public override void MenuUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            this.Pause();
            navigate.SwitchToMenu(pausedMenu);
        }
        if (timeToBeginFrame() && isFrameAble())
        {
            tick = 0;
            StepFrame();
        }

        ++tick;
    }
    public override void MenuLeave()
    {
        base.MenuLeave();
    }
    public override void RegisterTouchingEvents()
    {
        frame.ScriptUpdate += frame_ScriptUpdate;
        timer.ClearedZombies += timer_ClearedZombies;
        timer.OverrunByZombies += timer_OverrunByZombies;
        toucher.DropGameObject += toucher_DropGameObject;
        toucher.PickUpGameObject += toucher_PickUpGameObject;
        toucher.OverGameObject += toucher_OverGameObject;
    }
    public override void UnregisterTouchingEvents()
    {
        timer.ClearedZombies -= timer_ClearedZombies;
        timer.OverrunByZombies -= timer_OverrunByZombies;
        toucher.DropGameObject -= toucher_DropGameObject;
        toucher.PickUpGameObject -= toucher_PickUpGameObject;
        toucher.OverGameObject -= toucher_OverGameObject;
        frame.ScriptUpdate -= frame_ScriptUpdate;
    }
    public override void AssignButtonMenus()
    {
    }

    private void timer_OverrunByZombies(GenericStatusEventArgs eventArgs)
    {
        LoseLevel();
    }
    private void timer_ClearedZombies(GenericStatusEventArgs eventArgs)
    {
        BeatLevel();
    }
    private void toucher_PickUpGameObject(GameObjectTransferEventArgs eventArgs)
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
    private void toucher_DropGameObject(GameObjectTransferEventArgs eventArgs)
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
    private void frame_ScriptUpdate(MonoScriptEventArgs eventArgs)
    {
        frame.GridUpdateAction();
    }

    private IEnumerator startTimerWhenFrameStable()
    {
        WeaponBar bar = WeaponBar.GetWeaponBar(this);
        foreach (GameObject button in bar.ActiveWeaponButtons)
        {
            WeaponButton weapon = button.GetComponent<WeaponButton>();
            if (weapon != null)
            {
                weapon.StopFiring();
            }
        }

        level.ResetLevel();
        timer.ResizeBar();

        LevelText.SetText(level.CurrentLevelName());
        yield return new WaitForSeconds(2);
        LevelText.SetText("");

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

    public void StartLevel()
    {
        toucher.ClearHistory();
        frame.InitializeFrame();
        frame.SpawnFrame();
        this.StartCoroutine(startTimerWhenFrameStable());
    }
    private void BeatLevel()
    {
        this.EndLevel();
        this.StartCoroutine(BeatLevelCoroutine());
    }
    private IEnumerator BeatLevelCoroutine()
    {
        // Congradulations you beat this level!
        frame.ResetFrame();
        level.NextLevel();
        timer.ResizeBar();
        yield return new WaitForSeconds(0.5f);
        navigate.SwitchToMenu(navigate.GetMenu("Gunshop Menu"));
        yield return 0;
    }
    private void LoseLevel()
    {
        this.EndLevel();
        navigate.GoBack();
    }

    private void EndLevel()
    {
        frame.ResetFrame();
        toucher.ClearHistory();

        WeaponBar bar = WeaponBar.GetWeaponBar(this);
        foreach (GameObject button in bar.ActiveWeaponButtons)
        {
            WeaponButton weapon = button.GetComponent<WeaponButton>();
            if (weapon != null)
            {
                weapon.StopFiring();
            }
        }
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
}
