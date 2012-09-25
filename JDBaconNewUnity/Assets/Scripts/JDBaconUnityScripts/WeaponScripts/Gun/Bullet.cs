using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(SphereCollider))]
public class Bullet : MonoBehaviour {
	
	protected float mSpeed = 20.0f;
	public float Speed{get{return mSpeed;}}
	
	protected float mMaxSpeed = 25.0f;
	public float MaxSpeed{get{return mMaxSpeed;}}
	
	protected Vector3 mMovementDirection;
    public Vector3 MovementDirection { get { return mMovementDirection; } }
	
	protected float mAcceleration = 1.0f;
	public float Acceleration{get{return mAcceleration;}}
	
	protected int mDamage = 10;
	public int Damage{get{return mDamage;}}
	
	protected float mLifeSpan = 10.0f;
	public float LifeSpan{get{return mLifeSpan;}}
	
	// Knows what types of creature shot the bullet
	protected Type mCharacterType;
    public Type CharacterType { get { return mCharacterType; } }
	
	
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
	
    // Sets the character who fired the weapon
    public void setCharacter(Character character){
        mCharacterType = character.GetType();
    }
	
    //Script used on collision
	void OnTriggerEnter(Collider other){
        
        // Instantiate a sprite for reference of the collision
        Character sprite = other.gameObject.GetComponent<Character>();

        if (sprite != null){
            if (sprite.GetType() != mCharacterType){
                sprite.TakeDamage(mDamage);
            }
        }
	}

}
