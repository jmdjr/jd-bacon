using UnityEngine;
using System;
using System.Collections;

using Object = UnityEngine.Object;
using System.Collections.Generic;


public delegate void BulletActionEvent(BulletActionEventArgs eventArgs);
public class BulletActionEventArgs
{
    private Position2D point;
    private JDBullet bulletSpawned;

    public Position2D Point { get { return point; } }
    public JDBullet Bullet { get { return bulletSpawned; } }

    public BulletActionEventArgs(Position2D point, JDBullet bulletSpawned) 
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

public delegate void PositionTransferEvent(PositionTransferEventArgs eventArgs);
public class PositionTransferEventArgs
{
    private Position2D origin;
    private Position2D next;

    public Position2D Origin { get { return origin; } }
    public Position2D Next { get { return next; } }

    public PositionTransferEventArgs(Position2D origin, Position2D next)
    {
        this.origin = origin;
        this.next = next;
    }
}

public delegate void GenericStatusEvent(GenericStatusEventArgs eventArgs);
public class GenericStatusEventArgs
{
    private GenericStatusFlags flag;

    public GenericStatusFlags Flag { get { return flag; } }

    public GenericStatusEventArgs(GenericStatusFlags flag)
    {
        this.flag = flag;
    }
}
