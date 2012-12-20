using UnityEngine;
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
    private Queue<Position2D> toMoveAndDestroy;
    public override void Awake()
    {
        base.Awake();
        toMoveAndDestroy = new Queue<Position2D>();

        frame = new BulletMatrix(dimension.Y, dimension.X);
        grid = new GameObject[dimension.Y, dimension.X];
        bulletSpawners = new List<BulletSpawner>();

        frame.Load(false);

        var childrenComps = this.gameObject.transform.GetComponentsInChildren(typeof(BulletSpawner));

        foreach (var comp in childrenComps)
        {
            BulletSpawner spawner = comp.gameObject.GetComponentInChildren<BulletSpawner>();
            spawner.SpawnedBulletGameObject += new GameObjectTransferEvent(Frame10x10_SpawnedBulletGameObject);
            bulletSpawners.Add(spawner);
        }

        bulletSpawners = bulletSpawners.OrderBy(o => o.transform.position.x).ToList();
        foreach (var b in bulletSpawners)
        {
            Debug.Log(b.transform.position.ToString());
        }

        frame.BulletSpawned += new BulletActionEvent(frame_BulletSpawned);
        frame.BulletDestroyed += new BulletActionEvent(frame_BulletDestroyed);
    }

    public override void Start()
    {
        base.Start();
        frame.SpawnFullGrid();
        Debug.Log(frame.ToString());
    }
    private void Frame10x10_SpawnedBulletGameObject(GameObjectTransferEventArgs eventArgs)
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
    }

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

    public override void Update()
    {
        base.Update();

        if (toMoveAndDestroy.Count > 0)
        {
            var point = toMoveAndDestroy.Peek();
            if (grid[point.Y, point.X] != null)
            {
                point = toMoveAndDestroy.Dequeue();

                grid[point.Y, point.X].transform.position.Set(0, 0, 100);
            }
        }
    }
}
