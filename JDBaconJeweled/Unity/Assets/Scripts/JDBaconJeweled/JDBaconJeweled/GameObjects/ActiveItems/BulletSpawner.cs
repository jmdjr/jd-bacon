using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Object = UnityEngine.Object;
using Random = System.Random;

[RequireComponent(typeof(Rigidbody))]
public class BulletSpawner : JDMonoBodyBehavior
{
    private Stack<GameObject> toSpawn;
    private Vector3 Position;
    private Quaternion Rotation;
    private bool pauseThisSpawner;
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
        toSpawn = new Stack<GameObject>();
        delay = 15;
        tick = 0;

        BulletGameGlobal.Instance.PreventBulletBouncing = true;
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
        toSpawn.Push(spawnedBullet);
    }

    public override void Update()
    {
        if (tick >= delay)
        {
            tick = 0;
            if (!pauseThisSpawner && !BulletGameGlobal.Instance.PauseSpawners && toSpawn.Count > 0)
            {
                GameObject bullet = toSpawn.Pop();
                BoxCollider boxCollider = bullet.GetComponent<BoxCollider>();

                if (boxCollider != null && !boxCollider.enabled)
                {
                    boxCollider.enabled = true;
                }

                MeshRenderer renderer = bullet.GetComponent<MeshRenderer>();

                if (renderer != null && !renderer.enabled)
                {
                    renderer.enabled = true;
                }

                Rigidbody body = bullet.GetComponent<Rigidbody>();

                if (body != null)
                {
                    body.useGravity = true;
                }

                this.pauseThisSpawner = true;
            }
        }

        ++tick;
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

    public override void OnCollisionEnter(Collision other)
    {
        FallingBullet bullet = other.gameObject.GetComponent<FallingBullet>();
        if (bullet != null)
        {
            pauseThisSpawner = true;
        }
    }

    public override void OnCollisionExit(Collision other)
    {
        FallingBullet bullet = other.gameObject.GetComponent<FallingBullet>();
        if (bullet != null)
        {
            pauseThisSpawner = false;
        }
    }
}