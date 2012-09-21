using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(SphereCollider))]
public class Bullet : MonoBehaviour {
	
	protected float mSpeed = 20.0f;
	public float speed{get{return mSpeed;}}
	
	protected float mMaxSpeed = 25.0f;
	public float maxSpeed{get{return maxSpeed;}}
	
	protected Vector3 mMovementDirection;
	public float movementDirection{get{return movementDirection;}}
	
	protected float mAcceleration = 1.0f;
	public float acceleration{get{return acceleration;}}
	
	protected float mDamage = 10.0f;
	public float damage{get{return damage;}}
	
	protected float mLifeSpan = 10.0f;
	public float lifeSpan{get{return lifeSpan;}}
	
	// Knows what types of creature shot the projectile
	protected Type mInstigatorType;
	
	
	// Use this for initialization
	void Start () {
		
		// bullet moves forward
		mMovementDirection = transform.forward;
		
		
		gameObject.GetComponent<SphereCollider>().isTrigger = true;
		
		// Start mini thread to destroy object after period of time
		StartCoroutine(kill());
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += (mMovementDirection * mSpeed * Time.deltaTime);
		
		// speed up to max speed
		if(mSpeed < mMaxSpeed){
			mSpeed += mAcceleration * Time.deltaTime;
		}
		
		// Destroy game object if its off the screen
		if(!renderer.isVisible)
		{
			Destroy(gameObject);
		}
	}
	
	// object destroyed after 10 seconds
	protected IEnumerator kill(){
		yield return new WaitForSeconds(mLifeSpan);
		Destroy(gameObject);
	}
	
	//Script used on collision
	void OnTriggerEnter(Collider other){
		
	}
	// Function to set who shot the item
	//public void setInstigator();
}
