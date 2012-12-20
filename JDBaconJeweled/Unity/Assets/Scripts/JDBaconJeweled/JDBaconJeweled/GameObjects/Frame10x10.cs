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
    private List<GameObject> bulletSpawners;

    public override void Awake()
    {
        frame = new BulletMatrix(dimension.Y, dimension.X);
        grid = new GameObject[dimension.Y, dimension.X];
        bulletSpawners = new List<GameObject>();

        frame.BulletSpawned += new BulletSpawnedEvent(frame_BulletSpawned);
        frame.Load(false);

        int count = this.gameObject.transform.GetChildCount();
        var childrenComps = this.gameObject.transform.GetComponentsInChildren(typeof(BulletSpawner));

        foreach (var comp in childrenComps)
        {
            BulletSpawner spawner = comp.gameObject.GetComponent<BulletSpawner>();
            spawner.SpawnedBulletGameObject += new GameObjectTransferEvent(Frame10x10_SpawnedBulletGameObject);
            bulletSpawners.Add(comp.gameObject);
        }

        bulletSpawners = bulletSpawners.OrderBy(o => o.transform.position.x).ToList();

        base.Awake();
    }

    private void Frame10x10_SpawnedBulletGameObject(GameObjectTransferEventArgs eventArgs)
    {
        if (eventArgs.Position.X < this.dimension.X 
            && eventArgs.Position.Y < this.dimension.Y
            && 0 <= this.dimension.Y 
            && 0 <= this.dimension.X)
        {
            grid[eventArgs.Position.Y, eventArgs.Position.X] = eventArgs.GameObject;
        }
    }

    private void frame_BulletSpawned(BulletSpawnedEventArgs eventArgs)
    {
        Debug.Log("event bullet spawned fired...");
        QueueBulletInSpawner(eventArgs.Bullet, eventArgs.Point);
    }

    private void QueueBulletInSpawner(JDBullet bullet, Position2D point)
    {
        BulletSpawner spawner = null;
 
        for (int i = 0; i < bulletSpawners.Count; ++i)
        {
            if (point.X == i)
            {
                Debug.Log("Queueing bullet in spawner...");
                spawner = bulletSpawners[i].GetComponentInChildren<BulletSpawner>();
                break;
            }
        }

        if (spawner != null)
        {
            spawner.QueueBullet(bullet, point);
        }
    }
}
