﻿using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Linq;

using Object = UnityEngine.Object;
using Random = System.Random;
using System.Collections.Generic;

public class Frame10x10 : JDMonoGuiBehavior
{
    private GameObject[,] grid;

    private BulletMatrix frame;
    private Position2D dimension = new Position2D(10, 10);
    private List<BulletSpawner> bulletSpawners;
    private List<FallingBullet> bulletGroups;

    private Queue<Position2D> toMoveAndDestroy;

    public override void Awake()
    {
        base.Awake();
        toMoveAndDestroy = new Queue<Position2D>();

        frame = new BulletMatrix(dimension.Y, dimension.X);
        grid = new GameObject[dimension.Y, dimension.X];
        bulletSpawners = new List<BulletSpawner>();
        this.bulletGroups = new List<FallingBullet>();

        frame.Load(false);

        var childrenComps = this.gameObject.transform.GetComponentsInChildren(typeof(BulletSpawner));

        foreach (var comp in childrenComps)
        {
            BulletSpawner spawner = comp.gameObject.GetComponentInChildren<BulletSpawner>();
            spawner.SpawnedBulletGameObject += new GameObjectTransferEvent(spawner_SpawnedBulletGameObject);
            bulletSpawners.Add(spawner);
        }

        childrenComps = this.gameObject.transform.GetComponentsInChildren(typeof(FallingBullet));

        foreach (var comp in childrenComps)
        {
            FallingBullet bullet = comp.gameObject.GetComponentInChildren<FallingBullet>();
            bulletGroups.Add(bullet);
        }


        bulletSpawners = bulletSpawners.OrderBy(o => o.transform.position.x).ToList();

        frame.BulletDestroyed += new BulletActionEvent(frame_BulletDestroyed);
        frame.BulletSpawned += new BulletActionEvent(frame_BulletSpawned);

        DebugCommands.Instance.AddCommand(new ConsoleCommand("PrintFrame", "Prints an ascii interpretation of the grid.\n", Frame10x10_PrintGridCommand));
    }

    public override void Start()
    {
        base.Start();
        BulletGameGlobal.Instance.PreventBulletBouncing = true;
        frame.SpawnFullGrid();
    }

    #region Events
    private void spawner_SpawnedBulletGameObject(GameObjectTransferEventArgs eventArgs)
    {
        var pos = eventArgs.Position;
        var dim = this.dimension;
        if (pos.X < dim.X && pos.X >= 0 && pos.Y < dim.Y && pos.Y >= 0)
        {
            grid[eventArgs.Position.Y, eventArgs.Position.X] = eventArgs.GameObject;
        }
    }
    private void frame_BulletSpawned(BulletActionEventArgs eventArgs)
    {
        QueueBulletInSpawner(eventArgs.Bullet, eventArgs.Point);
    }
    private void frame_BulletDestroyed(BulletActionEventArgs eventArgs)
    {
        var x = eventArgs.Point.X;
        var y = eventArgs.Point.Y;
        toMoveAndDestroy.Enqueue(eventArgs.Point);
        BulletGameGlobal.Instance.PauseSpawners = true;
    }
    #endregion

    private void bubblePositionUp(int i, int j)
    {
        while (i > 0)
        {
            var first = new Position2D(j, i);
            var second = new Position2D(j, --i);

            SwapBullets(first, second);
            this.Debug_PrintGrid();
        }
    }
    private void SwapBullets(Position2D first, Position2D second)
    {
        GameObject temp = grid[first.Y, first.X];
        grid[first.Y, first.X] = grid[second.Y, second.X];
        grid[second.Y, second.X] = temp;
    }
    #region Checks

    public bool FrameStableCheck_FrameMissingBullets;
    public bool FrameStableCheck_FrameBulletsStillFalling;
    public bool IsFrameStable()
    {
        bool FrameStable = true;
        bool FrameMissingBullets = false;
        bool FrameBulletsStillFalling = false;

        foreach (GameObject bullet in grid)
        {
            if (bullet != null)
            {
                FallingBullet fBullet = bullet.GetComponent<FallingBullet>();

                if (fBullet != null)
                {
                    FrameStable &= !fBullet.IsFalling;
                    if (fBullet.IsFalling)
                    {
                        FrameBulletsStillFalling = true;
                    }
                }
            }
            else
            {
                FrameStable &= false;
                FrameMissingBullets = true;
            }
        }
        
        //Debug.Log("Frame10x10 - IsFrameStable(): " + FrameStable + " Cause? missing bullets: " + FrameMissingBullets + " still falling: " + FrameBulletsStillFalling );
        
        FrameStableCheck_FrameMissingBullets = FrameMissingBullets;
        FrameStableCheck_FrameBulletsStillFalling = FrameBulletsStillFalling;

        return FrameStable;
    }

    public int RemainningBulletsToSpawn()
    {
        int count = 0;
        foreach (BulletSpawner spawner in bulletSpawners)
        {
            count += spawner.RemainningBullet();
        }

        return count;
    }

    public bool HasMatches()
    {
        return frame.CollectMatchedBullets().Count > 0;
    }
    #endregion

    #region public Mechanics
    public List<Position2D> DropAnyMatches()
    {
        var matches = frame.CollectMatchedBullets();

        if (matches != null)
        {
            frame.DropMatchedBullets(matches);
        }

        return matches;
    }
    public void BubbleUpAndSpawn(List<Position2D> matches)
    {
        frame.ShiftItemsDown(matches);
    }
    #endregion

    private void QueueBulletInSpawner(JDBullet bullet, Position2D point)
    {
        BulletSpawner spawner = null;
        for (int i = 0; i < bulletSpawners.Count; ++i)
        {
            if (point.X == i)
            {
                spawner = bulletSpawners[i];
                break;
            }
        }

        if (spawner != null)
        {
            spawner.QueueBullet(bullet, point);
        }
    }

    private bool DropBullet(int i, int j)
    {
        var gridBullet = grid[i, j];
        if (gridBullet != null)
        {
            gridBullet.transform.position = new Vector3(40, 3, 1);
            gridBullet.rigidbody.collider.isTrigger = true;
            grid[i, j] = null;
            return true;
        }

        return false;
    }

    public override void Update()
    {
        base.Update();

        if (toMoveAndDestroy.Count > 0 && this.IsFrameStable())
        {
            BulletGameGlobal.Instance.PauseSpawners = true;
            
            for( var point = toMoveAndDestroy.GetEnumerator(); point.MoveNext(); )
            {
                Position2D spot = point.Current;

                this.Debug_PrintGrid();
                this.DropBullet(spot.Y, spot.X);
                this.bubblePositionUp(spot.Y, spot.X);
                this.Debug_PrintGrid();
            }

            BulletGameGlobal.Instance.PauseSpawners = false;
        }
    }

    #region Debug

    public void Debug_PrintGrid()
    {
        string gridString = "";

        for (int i = 0; i < dimension.Y; ++i)
        {
            for (int j = 0; j < dimension.X; ++j)
            {
                GameObject go = grid[i, j];

                if (go != null)
                {
                    FallingBullet goScript = go.GetComponent<FallingBullet>();

                    if (goScript != null)
                    {
                        gridString += goScript.BulletReference.bulletDebugChar;
                    }
                    else
                    {
                        gridString += "?";
                    }
                }
                else
                {
                    gridString += "_";
                }
            }
            gridString += ";" + '\n';
        }

        Debug.Log(gridString);
    }

    public void Frame10x10_PrintGridCommand(string[] Params)
    {
        this.Debug_PrintGrid();
    }
    #endregion
}
