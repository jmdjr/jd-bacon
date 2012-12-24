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
    private Stack<JDBullet> toSpawn;
    private Stack<Position2D> toSpawnPosition;
    private int delay = 30;
    private int tick = 0;

    public event GameObjectTransferEvent SpawnedBulletGameObject;

    public override void Awake()
    {
        toSpawn = new Stack<JDBullet>();
        toSpawnPosition = new Stack<Position2D>();
        base.Awake();
    }

    private GameObject SpawnBullet(JDBullet bullet)
    {
        var position = this.gameObject.transform.position;
        var rotation = this.gameObject.transform.rotation;
        Debug.Log("Spawned: " + bullet.bulletDebugChar + " Position: " + position);
        return (GameObject)Instantiate(Resources.Load(bullet.ResourceName), position, rotation);
    }

    public void QueueBullet(JDBullet bullet, Position2D position) 
    {
        this.toSpawn.Push(bullet);
        this.toSpawnPosition.Push(position);
    }

    public override void Update()
    {
        ++tick;
        if (tick >= delay)
        {
            tick = 0;
            if (SpawnedBulletGameObject != null && toSpawn.Count > 0 && toSpawn.Count == toSpawnPosition.Count)
            {
                JDBullet bullet = toSpawn.Pop();
                Position2D point = toSpawnPosition.Pop();
                GameObject spawned = SpawnBullet(bullet);
                FallingBullet spawnedScript = spawned.GetComponent<FallingBullet>();

                if (spawnedScript != null)
                {
                    spawnedScript.BulletReference = bullet;
                }

                GameObjectTransferEventArgs args = new GameObjectTransferEventArgs(spawned, point);
                SpawnedBulletGameObject(args);
            }
        }

        base.Update();
    }

    public bool HasBulletsToSpawn()
    {
        return toSpawn.Count > 0;
    }
}