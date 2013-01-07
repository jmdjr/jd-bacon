using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Object = UnityEngine.Object;
using Random = System.Random;

public class BulletRackDropper : JDMonoBodyBehavior
{
    public override void OnScriptTriggerEnter(Collision other)
    {
        base.OnScriptTriggerEnter(other);

    }
}
