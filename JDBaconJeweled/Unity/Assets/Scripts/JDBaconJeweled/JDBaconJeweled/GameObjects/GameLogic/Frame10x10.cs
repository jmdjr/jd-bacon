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
    static Frame10x10 instance;
    public static Frame10x10 Instance
    {
        get
        {
            if (instance == null)
            {
                var g = GameObject.Find("Frame");

                if (g != null)
                {
                    instance = g.GetComponent<Frame10x10>();
                }
            }

            return instance;
        }
    }

    private struct SpawningBullet
    {
        public Position2D position;
        public JDBullet bullet;

        public SpawningBullet(JDBullet bullet, Position2D position)
        {
            this.bullet = bullet;
            this.position = position;
        }
    }

    private BulletMatrix frame;
    private Position2D dimension = new Position2D(10, 10);
    private List<BulletSpawner> bulletSpawners;
    private List<FallingBullet> bulletGroups;
    private List<FallingBullet> AllBullets;
    private Queue<GameObject> toSpawn;

    public override void Awake()
    {
        base.Awake();
        frame = new BulletMatrix(dimension.Y, dimension.X);
        bulletSpawners = new List<BulletSpawner>();
        this.bulletGroups = new List<FallingBullet>();
        toSpawn = new Queue<GameObject>();
        AllBullets = new List<FallingBullet>();

        frame.Load(false);

        var childrenComps = this.gameObject.transform.GetComponentsInChildren(typeof(BulletSpawner));

        foreach (var comp in childrenComps)
        {
            BulletSpawner spawner = comp.gameObject.GetComponentInChildren<BulletSpawner>();
            bulletSpawners.Add(spawner);
        }

        childrenComps = this.gameObject.transform.GetComponentsInChildren(typeof(FallingBullet));

        foreach (var comp in childrenComps)
        {
            FallingBullet bullet = comp.gameObject.GetComponentInChildren<FallingBullet>();
            bulletGroups.Add(bullet);
        }
        bulletSpawners = bulletSpawners.OrderBy(o => o.transform.position.x).ToList();

        frame.BulletDestroyed += new BulletActionEvent(frame_BulletDestroyed);
        frame.BulletSpawned += new BulletActionEvent(frame_BulletSpawned);

        DebugCommands.Instance.AddCommand(new ConsoleCommand("PrintFrame", "stuffs", Debug_PrintGrid));
        DebugCommands.Instance.AddCommand(new ConsoleCommand("TogglePause", "stuffs", Debug_PauseGameplay));
    }

    public override void Start()
    {
        base.Start();
        BulletGameGlobal.Instance.PreventBulletBouncing = true;
        frame.SpawnFullGrid();
    }

    #region Events
    private void frame_BulletSpawned(BulletActionEventArgs eventArgs)
    {
        QueueBulletInSpawner(eventArgs.Bullet, eventArgs.Point);
    }
    private void frame_BulletDestroyed(BulletActionEventArgs eventArgs)
    {
        DropBullet(eventArgs.Bullet);
    }
    #endregion

    private BulletSpawner GetBulletSpawner(int xPos)
    {
        BulletSpawner spawner = null;
        for (int i = 0; i < bulletSpawners.Count; ++i)
        {
            if (xPos == i)
            {
                spawner = bulletSpawners[i];
                break;
            }
        }

        return spawner;
    }
    private FallingBullet GetBulletGroup(JDBullet bulletType)
    {
        FallingBullet group = null;
        for (int i = 0; i < bulletGroups.Count; ++i)
        {
            if ((bulletGroups[i].BulletReference != null && bulletGroups[i].BulletReference.Name == bulletType.Name) || bulletGroups[i].ManualName == bulletType.Name)
            {
                group = bulletGroups[i];
                break;
            }
        }

        return group;
    }

    #region Checks

    public bool IsFrameStable()
    {
        bool FrameStable = true;

        foreach (FallingBullet bullet in AllBullets)
        {
            if (bullet != null)
            {
                FrameStable &= !bullet.IsFalling;
            }
            else
            {
                FrameStable &= false;
            }
        }
        if (RemainningBulletsToSpawn() > 0)
        {
            FrameStable &= false;
        }

        if (this.AllBullets.Count < dimension.X * dimension.Y)
        {
            FrameStable = false;
        }


        return FrameStable;
    }

    public int RemainningBulletsToSpawn()
    {
        int count = 0;
        foreach (BulletSpawner spawner in bulletSpawners)
        {
            count += spawner.RemainningBullet();
        }

        return count;
    }

    public bool HasMatches()
    {
        return frame.CollectMatchedBullets().Count > 0;
    }
    #endregion

    #region public Mechanics
    private bool DropBullet(JDBullet bullet)
    {
        FallingBullet b = this.AllBullets.Find(fb => bullet == fb.BulletReference);

        if (b != null)
        {
            b.transform.position = b.transform.parent.transform.position;
            b.rigidbody.collider.isTrigger = true;
            this.AllBullets.Remove(b);
            return true;
        }

        return false;
    }
    public List<Position2D> DropAnyMatches()
    {
        var matches = frame.CollectMatchedBullets();

        if (matches != null)
        {
            frame.DropMatchedBullets(matches);
        }

        return matches;
    }
    public void BubbleUpAndSpawn(List<Position2D> matches)
    {
        frame.ShiftItemsDown(matches);
    }
    #endregion
    
    private void QueueBulletInSpawner(JDBullet bullet, Position2D point)
    {
        BulletSpawner spawner = GetBulletSpawner(point.X);
        GameObject fallingBullet = spawner.SpawnBullet(bullet);
        FallingBullet group = this.GetBulletGroup(bullet);

        if(group != null)
        {
            fallingBullet.transform.parent = group.gameObject.transform;
        }
        
        FallingBullet fallingBulletScript = fallingBullet.GetComponent<FallingBullet>();
        
        if(fallingBulletScript != null)
        {
            fallingBulletScript.MySpawner = spawner;
            this.AllBullets.Add(fallingBulletScript);
        }

        toSpawn.Enqueue(fallingBullet);
    }

    public override void Update()
    {
        base.Update();

        while (toSpawn.Count() > 0)
        {
            GameObject spawn = toSpawn.Dequeue();
            FallingBullet fallingBullet = spawn.GetComponent<FallingBullet>();

            if (fallingBullet != null)
            {
                fallingBullet.MySpawner.QueueBullet(spawn);
            }
        }
    }

    public bool CanMatchMore() { return frame.CanMatchMore(); }

    public bool SwapBullets(GameObject firstBullet, GameObject SecondBullet)
    {
        if (firstBullet == null || SecondBullet == null)
        {
            return false;
        }

        FallingBullet first = firstBullet.GetComponent<FallingBullet>();
        FallingBullet second = SecondBullet.GetComponent<FallingBullet>();

        if (first == null || second == null || first == second || first.BulletReference == null || second.BulletReference == null)
        {
            return false;
        }
        
        Position2D firstPos = frame.GetBulletPosition(first.BulletReference);
        Position2D secondPos = frame.GetBulletPosition(second.BulletReference);

        bool canSwap = frame.CanSwapPositions(firstPos, secondPos);
        bool shouldSwap = frame.IsBadSwap(firstPos, secondPos);

        if (canSwap)
        {
            Vector3 firstGOPosition = firstBullet.transform.position;
            firstBullet.transform.position = SecondBullet.transform.position;
            SecondBullet.transform.position = firstGOPosition;

            if (!shouldSwap)
            {
                firstGOPosition = firstBullet.transform.position;
                firstBullet.transform.position = SecondBullet.transform.position;
                SecondBullet.transform.position = firstGOPosition;
            }
            else
            {
                frame.SwapPositions(firstPos, secondPos);
            }
        }

        return canSwap;
    }

    #region Debug

    public void Debug_PauseGameplay(string[] Params)
    {
        if(Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }
    }

    public void Debug_PrintGrid(string[] Params)
    {
        Debug.Log(frame.Debug_PrintGrid());
    }

    #endregion
}
