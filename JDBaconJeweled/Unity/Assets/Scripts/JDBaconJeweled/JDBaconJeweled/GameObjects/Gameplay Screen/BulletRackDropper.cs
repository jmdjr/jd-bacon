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
    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        FallingBullet bullet = other.gameObject.GetComponent<FallingBullet>();

        if (bullet != null)
        {
            MeshRenderer renderer = bullet.GetComponent<MeshRenderer>();

            if (renderer != null && !renderer.enabled)
            {
                renderer.enabled = true;
            }
        }
    }
}
