using UnityEngine;
using System;
using System.Collections;

using Object = UnityEngine.Object;
using System.Collections.Generic;


public delegate void BulletSpawnedEvent(BulletSpawnedEventArgs eventArgs);
public class BulletSpawnedEventArgs
{
    private Position2D point;
    private JDBullet bulletSpawned;

    public Position2D Point { get { return point; } }
    public JDBullet Bullet { get { return bulletSpawned; } }

    public BulletSpawnedEventArgs(Position2D point, JDBullet bulletSpawned) 
    {
        this.point = point;
        this.bulletSpawned = bulletSpawned;
    }
}

public delegate void GameObjectTransferEvent(GameObjectTransferEventArgs eventArgs);
public class GameObjectTransferEventArgs
{
    private GameObject gameObject;
    private Position2D position;

    public GameObject GameObject { get { return gameObject; } }
    public Position2D Position { get { return position; } }

    public GameObjectTransferEventArgs(GameObject gameObject, Position2D position)
    {
        this.gameObject = gameObject;
        this.position = position;
    }
}