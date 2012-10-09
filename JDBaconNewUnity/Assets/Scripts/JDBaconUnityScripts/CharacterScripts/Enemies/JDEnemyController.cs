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
}
