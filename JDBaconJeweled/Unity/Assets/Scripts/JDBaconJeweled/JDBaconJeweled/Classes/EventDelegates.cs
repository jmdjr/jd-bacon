using UnityEngine;
using System;
using System.Collections;

using Object = UnityEngine.Object;
using System.Collections.Generic;


public delegate void BulletSpawned(BulletSpawnedEventArgs eventArgs);
public class BulletSpawnedEventArgs
{
    private Position2D point;
    private JDBullet bulletSpawned;

    public BulletSpawnedEventArgs(Position2D point, JDBullet bulletSpawned) 
    {
        this.point = point;
        this.bulletSpawned = bulletSpawned;
    }
}
