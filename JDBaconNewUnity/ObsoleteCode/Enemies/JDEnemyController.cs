using UnityEngine;
using System;
using System.Collections;
using SmoothMoves;

[RequireComponent(typeof(Rigidbody))]
public class JDEnemyController : JDMonoBodyBehavior
{
    JDEnemy EnemyReference;
    public override void Awake()
    {
        base.Awake();
        EnemyReference = new JDEnemy();
        JDCollection.Add(EnemyReference);
    }

    public override void Update()
    {
        base.Update();

        if (this.EnemyReference.IsDead)
        {
            this.StopAllCoroutines();

            if (this.gameObject != null)
            {
                Debug.Log(this.gameObject);
                Destroy();
                JDGame.GrimReaper.Kill(this.gameObject);
            }
        }
    }
}
