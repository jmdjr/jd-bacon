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
    public Position2D dimension = new Position2D(10, 10);
    public float SwapDelayTime = 0.25f;

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

    private BulletMatrix frame;

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
        
        bulletSpawners = bulletSpawners.OrderBy(o => o.transform.position.x).ToList();
        if (bulletSpawners.Count > 0)
        {
            var x = bulletSpawners[0].transform.position.x;
            var y = bulletSpawners[0].transform.position.y;
            var z = bulletSpawners[0].transform.position.z;

            for (int i = 1; i < bulletSpawners.Count; ++i)
            {
                var spawner = bulletSpawners[i];
                bulletSpawners[i].transform.position = new Vector3(i * bulletSpawners[0].transform.localScale.x + x + 0.01f, y, z);
            }
        }

        childrenComps = this.gameObject.transform.GetComponentsInChildren(typeof(FallingBullet));

        foreach (var comp in childrenComps)
        {
            FallingBullet bullet = comp.gameObject.GetComponentInChildren<FallingBullet>();
            bulletGroups.Add(bullet);
        }

        frame.BulletDestroyed += new BulletActionEvent(frame_BulletDestroyed);
        frame.BulletSpawned += new BulletActionEvent(frame_BulletSpawned);

        DebugCommands.Instance.AddCommand(new ConsoleCommand("PrintFrame", "stuffs", Debug_PrintGrid));
        DebugCommands.Instance.AddCommand(new ConsoleCommand("TogglePause", "stuffs", Debug_PauseGameplay));
    }

    public override void Start()
    {
        base.Start();
        BulletGameGlobal.Instance.PauseFrame = false;
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


        return FrameStable && !BulletGameGlobal.Instance.PauseFrame;
    }

    public List<GameObject> RemainningSpawnBullets()
    {
        List<GameObject> collection = new List<GameObject>();

        foreach (BulletSpawner spawner in bulletSpawners)
        {
            collection.AddRange(spawner.BulletsToSpawn());
        }

        return collection;
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

    public void ToggleBulletsFalling(bool toggle)
    {
        this.AllBullets.ForEach(bullet =>
        {
            bullet.rigidbody.useGravity = toggle;
        });
    }

    public override void Update()
    {
        base.Update();
        if (BulletGameGlobal.Instance.PauseFrame)
        {
            Physics.gravity = Vector3.zero;
        }
        else
        {
            Physics.gravity = new Vector3(0, -9.81f, 0);
        }
        while (!BulletGameGlobal.Instance.PauseFrame && toSpawn.Count() > 0)
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
        if (firstBullet == null || SecondBullet == null || !this.IsFrameStable())
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

        if (canSwap && shouldSwap)
        {
            this.StartCoroutine(SwapDelay(firstBullet.transform, SecondBullet.transform, SwapDelayTime));

            frame.SwapPositions(firstPos, secondPos);
        }
        else if(canSwap)
        {
            this.StartCoroutine(BadSwapDelay(firstBullet.transform, SecondBullet.transform, SwapDelayTime));
        }

        return canSwap;
    }

    IEnumerator BadSwapDelay(Transform first, Transform second, float HalfTimeDelay) 
    {
        BulletGameGlobal.Instance.PauseFrame = true;
        Vector3 firstPosition = first.transform.position;
        Vector3 secondPosition = second.transform.position;

        this.StartCoroutine(SLerpTransitions.MoveWithConstantTimeTo(first, second, HalfTimeDelay));
        this.StartCoroutine(SLerpTransitions.MoveWithConstantTimeTo(second, first, HalfTimeDelay));
        yield return new WaitForSeconds(HalfTimeDelay);

        first.transform.position = secondPosition;
        second.transform.position = firstPosition;

        this.StartCoroutine(SLerpTransitions.MoveWithConstantTimeTo(first, second, HalfTimeDelay));
        this.StartCoroutine(SLerpTransitions.MoveWithConstantTimeTo(second, first, HalfTimeDelay));
        yield return new WaitForSeconds(HalfTimeDelay);
        BulletGameGlobal.Instance.PauseFrame = false;

        first.transform.position = firstPosition;
        second.transform.position = secondPosition;
        yield return 0;
    }

    IEnumerator SwapDelay(Transform first, Transform second, float TimeDelay)
    {
        this.StartCoroutine(SLerpTransitions.MoveWithConstantTimeTo(first, second, TimeDelay));
        this.StartCoroutine(SLerpTransitions.MoveWithConstantTimeTo(second, first, TimeDelay));
        BulletGameGlobal.Instance.PauseFrame = true;

        yield return new WaitForSeconds(TimeDelay);
        BulletGameGlobal.Instance.PauseFrame = false;
        
        yield return 0;
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
