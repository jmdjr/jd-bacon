using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class EnemyZombie : MonoBehaviour
{
	private Health ZombieHealth = new Health(20);
	
	
	public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.transform.tag == "Player")
        {
			ZombieHealth.ChangeCurrentHealth(-5);
			if(!ZombieHealth.IsAlive())
			{
				ZombieHealth.Dead();
				Destroy (gameObject);
			}
        }
    }
}
