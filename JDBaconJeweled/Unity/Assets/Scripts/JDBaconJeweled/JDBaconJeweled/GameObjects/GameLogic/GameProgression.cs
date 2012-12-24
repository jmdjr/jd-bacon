using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Object = UnityEngine.Object;
using Random = System.Random;

public class GameProgression : JDMonoGuiBehavior
{
    public override void Awake()
    {
        base.Awake();
        GameStatistics.Instance.AllowedBulletStat = JDIStatTypes.INDIVIDUALS;
    }
    private bool hasPrinted = false;

    public override void Update()
    {
        base.Update();

        if (!hasPrinted)
        {
            var g = GameObject.Find("Frame");

            if (g != null)
            {
                var f = g.GetComponent<Frame10x10>();
                if (f != null)
                {
                    f.Debug_PrintGrid();
                    hasPrinted = true;

                }
                else
                {
                    Debug.Log("Did not find Script");
                }
            }
            else
            {
                Debug.Log("Did not find Frame");
            }
        }
    }
}
