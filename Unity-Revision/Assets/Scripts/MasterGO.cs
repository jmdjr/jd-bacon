using UnityEngine;
using System.Collections;

public class MasterGO : MonoBehaviour {
	
	public int playerHealth;
	public int collectableCounter1;
	public int collectableCounter2;
	public int playerScore;
	
	private GameObject thePlayer;
	
	// Use this for initialization
	void Awake () 
	{
		DontDestroyOnLoad(gameObject);
		playerHealth = 100;
		
	}
	
	// Update is called once per frame
	void Update () {
		
//		if ( Input.GetKeyDown("o"))
//			{
//				playerHealth -=10;
//			}
//		
		if (playerHealth < 1)
		{
			thePlayer = GameObject.FindGameObjectWithTag("PlayerModel");
			thePlayer.GetComponent<MyCharController>().Death();
			collectableCounter1 = 0;
			collectableCounter2 = 0;
			Application.LoadLevel(Application.loadedLevel);
			playerHealth = 100;
		}
			
	}
	public void addHealth(int aHP){
			playerHealth = playerHealth + aHP;
	}
	
}