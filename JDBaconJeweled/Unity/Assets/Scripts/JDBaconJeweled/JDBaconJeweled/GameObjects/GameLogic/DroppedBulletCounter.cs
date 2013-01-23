using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Object = UnityEngine.Object;
using Random = System.Random;

public class DroppedBulletCounter : JDMonoGuiBehavior
{
    private List<GameObject> droppedBullets = new List<GameObject>();
    private List<FallingBullet> bulletGroups;

    public override void Awake()
    {
        base.Awake();

        var childrenComps = this.gameObject.transform.GetComponentsInChildren(typeof(FallingBullet));

        foreach (var comp in childrenComps)
        {
            FallingBullet bullet = comp.gameObject.GetComponentInChildren<FallingBullet>();
            bulletGroups.Add(bullet);
        }

    }

    // bullets to be counted then dropped.
    public void AddBullets(List<GameObject> bullets)
    {

    }
}