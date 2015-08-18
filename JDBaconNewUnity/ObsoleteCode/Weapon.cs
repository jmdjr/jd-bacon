using UnityEngine;
using System.Collections;

public abstract class Weapon : MonoBehaviour {
	
	public float attackRate = 1.0f;
    protected Character mCreator;
	

	// Use this for initialization
	void Start () {
        mCreator = transform.parent.gameObject.GetComponent<Character>();
	}
	
	// Remove object from the game
	protected virtual void Die()
	{
		DestroyObject(gameObject);	
	}

    public abstract IEnumerator Attack();
}
