using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Linq;

using Object = UnityEngine.Object;
using Random = System.Random;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class FallingBullet : JDMonoBodyBehavior
{
    public JDBullet BulletReference;
    public override void Update()
    {
        base.Update();

        if (BulletGameGlobal.Instance.PreventBulletBouncing)
        {
            Vector3 velocity = this.rigidbody.velocity;

            if (velocity.y > 0)
            {
                this.rigidbody.velocity = new Vector3(velocity.x, 0, velocity.z);
            }
        }
    }
}