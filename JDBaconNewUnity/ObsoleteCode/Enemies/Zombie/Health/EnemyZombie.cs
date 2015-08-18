using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class EnemyZombie : Character
{
    /// <summary>
    /// On creation of the object, set these values
    /// </summary>
    void Start()
    {
        mHealth = 5;
        mMaxHealth = 5;
    }

    /// <summary>
    /// On collision with the player
    /// TODO: Change it to weapon damage
    /// </summary>
    /// <param name="collision">Collision that was detected by Unity</param>
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
