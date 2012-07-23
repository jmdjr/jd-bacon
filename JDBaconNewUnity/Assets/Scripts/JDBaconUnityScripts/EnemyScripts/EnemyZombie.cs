using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class EnemyZombie : Health
{
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.transform.tag == "Player")
        {
            this.ChangeCurrentHealth(-5);
            if (!this.IsAlive())
            {
                this.Dead();
                JDGame.GrimReaper.Kill(this.gameObject);
            }
        }
    }
}
