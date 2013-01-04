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
    private Vector3 Position;
    private Quaternion Rotation;

    Frame10x10 gameFrame;
    Frame10x10 GameFrame
    {
        get
        {
            if (gameFrame == null)
            {
                var g = GameObject.Find("Frame");

                if (g != null)
                {
                    gameFrame = g.GetComponent<Frame10x10>();
                }
            }

            return gameFrame;
        }
    }
    public event GameObjectTransferEvent SpawnedBulletGameObject;

    public override void Awake()
    {
        toSpawn = new Stack<JDBullet>();
        toSpawnPosition = new Stack<Position2D>();

        this.Position = this.gameObject.transform.position;
        this.Rotation = this.gameObject.transform.rotation;

        base.Awake();
    }

    private GameObject SpawnBullet(JDBullet bullet)
    {
        return (GameObject)Instantiate(Resources.Load(bullet.ResourceName), this.Position, this.Rotation);
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
            if (!BulletGameGlobal.Instance.PauseSpawners && SpawnedBulletGameObject != null && toSpawn.Count > 0 && toSpawn.Count == toSpawnPosition.Count)
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

    public int RemainningBullet()
    {
        return toSpawn.Count;
    }

    public bool HasBulletsToSpawn()
    {
        return toSpawn.Count > 0;
    }
}