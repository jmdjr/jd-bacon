using UnityEngine;
using System;
using System.Collections;

using Object = UnityEngine.Object;
using System.Collections.Generic;

public class BulletGameGlobal
{


    private static BulletGameGlobal instance;
    public static BulletGameGlobal Instance
    {
        get 
        {
            if (instance == null)
            {
                instance = new BulletGameGlobal();
            }
            
            return instance;
        }
    }

    private BulletGameGlobal()
    {
    }

    public bool PreventBulletBouncing { get; set; }
}
