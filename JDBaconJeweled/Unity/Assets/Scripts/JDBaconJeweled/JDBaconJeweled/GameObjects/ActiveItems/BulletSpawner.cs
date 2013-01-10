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
    private Queue<GameObject> toSpawn;

    private Vector3 Position;
    private Quaternion Rotation;
    
    private int delay;
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

    private int tick;
    public override void Awake()
    {
        toSpawn = new Queue<GameObject>();
        delay = 15;
        tick = 0;

        BulletGameGlobal.Instance.PreventBulletBouncing = false;
        this.Position = this.gameObject.transform.position - new Vector3(0, 1, 0);
        this.Rotation = this.gameObject.transform.rotation;

        base.Awake();
    }

    public GameObject SpawnBullet(JDBullet bullet)
    {
        GameObject loadedBullet = (GameObject)Instantiate(Resources.Load(bullet.ResourceName), this.Position, this.Rotation);
        FallingBullet loadedBulletScript = loadedBullet.GetComponent<FallingBullet>();

        if (loadedBulletScript != null)
        {
            loadedBulletScript.BulletReference = bullet;
        }

        return loadedBullet;
    }

    public void QueueBullet(GameObject spawnedBullet)
    {
        if (spawnedBullet.transform.parent != null)
        {
            spawnedBullet.transform.rotation = spawnedBullet.transform.parent.rotation;
        }
        toSpawn.Enqueue(spawnedBullet);
    }

    public override void Update()
    {
        if (Time.timeScale > 0)
        {
            if (tick >= delay)
            {
                tick = 0;
                if (!BulletGameGlobal.Instance.PauseSpawners && toSpawn.Count > 0)
                {
                    GameObject bullet = toSpawn.Dequeue();
                    BoxCollider boxCollider = bullet.GetComponent<BoxCollider>();

                    if (boxCollider != null)
                    {
                        boxCollider.enabled = true;
                    }

                    Rigidbody body = bullet.GetComponent<Rigidbody>();

                    if (body != null)
                    {
                        body.useGravity = true;
                    }
                }
            }

            ++tick;
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