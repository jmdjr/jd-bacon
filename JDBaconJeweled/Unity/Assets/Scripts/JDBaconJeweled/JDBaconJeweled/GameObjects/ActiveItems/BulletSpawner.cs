using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Object = UnityEngine.Object;
using Random = System.Random;

public class BulletSpawner : JDMonoBehavior
{
    private Queue<JDBullet> toSpawn;
    private Queue<Position2D> toSpawnPosition;
    private int delay = 500;
    private int tick = 0;

    public event GameObjectTransferEvent SpawnedBulletGameObject;

    public override void Awake()
    {
        toSpawn = new Queue<JDBullet>();
        toSpawnPosition = new Queue<Position2D>();
        base.Awake();
    }

    private GameObject SpawnBullet(JDBullet bullet)
    {
        var position = this.gameObject.transform.position;
        var rotation = this.gameObject.transform.rotation;
        return (GameObject)Instantiate(Resources.Load(bullet.ResourceName), position, rotation);
    }

    public void QueueBullet(JDBullet bullet, Position2D position) 
    {
        this.toSpawn.Enqueue(bullet);
        this.toSpawnPosition.Enqueue(position);
    }

    public override void Update()
    {
        ++tick;
        if (tick >= delay)
        {
            tick = 0;
            if (SpawnedBulletGameObject != null)
            {
                JDBullet bullet = toSpawn.Dequeue();
                Position2D point = toSpawnPosition.Dequeue();

                GameObjectTransferEventArgs args = new GameObjectTransferEventArgs(SpawnBullet(bullet), point);
                SpawnedBulletGameObject(args);
            }
        }

        base.Update();
    }
}