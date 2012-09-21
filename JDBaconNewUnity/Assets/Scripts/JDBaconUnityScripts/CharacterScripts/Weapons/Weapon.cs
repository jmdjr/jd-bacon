using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {
	
	public float attackRate = 1.0f;
	public bool lockRotation;
	private Quaternion startingRotation;
	
	void onDrawGizmos()
	{
		Gizmos.DrawSphere(transform.position, 0.1f);
		Gizmos.DrawLine(transform.position, transform.position + transform.forward * .5f);
	}
	// Use this for initialization
	void Start () {
        
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator attack()
    {
        while (true)
        {
            if (renderer.isVisible)
            {
                transform.rotation = startingRotation;
            }
        }

    }
  

	
	// Remove object from the game
	protected virtual void die()
	{
		DestroyObject(gameObject);	
	}
}
