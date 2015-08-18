
using UnityEngine;
using System.Collections;

public class hud : MonoBehaviour {
	
	//HUD
	public Texture healthBar100;
	public Texture healthBar90;
	public Texture healthBar80;
	public Texture healthBar70;
	public Texture healthBar60;
	public Texture healthBar50;
	public Texture healthBar40;
	public Texture healthBar30;
	public Texture healthBar20;
	public Texture healthBar10;
	public Texture healthBar0;
	public Texture baconnaise;
	public Texture bonusPower;
	private string playerScore;
	private string collectableCounter1;
	private string collectableCounter2;
	private int health;
	
	// End of Game Screen
	public Texture2D endCinematic01;
	public Texture2D endCinematic02;
	public Texture2D endCinematic03;
	public Texture2D endCinematic04;
	public Texture2D thankYouScreen;
	public Texture2D credit001;
	public Texture2D credit002;
	public Texture2D credit003;
	public Texture2D credit004;
	public Texture2D credit005;
	public Texture2D credit006;
	public Texture2D credit007;
	public Texture2D credit008;
	public Texture2D credit009;
	public Texture2D credit010;	
	public Texture2D credit011;	
	public Texture2D credit012;	
	public Texture2D credit013;
	private Texture2D display;
	private float levelTime;
	private float creditsStartTime;
	private bool startedCredits;
	
	void Start()
	{

		startedCredits = false;
	}
	
	void OnGUI()
	{
		//HUD
		if ( Application.loadedLevel != 0 && Application.loadedLevel != 4)
		{
		if ( health > 90)
		{
			GUI.DrawTexture (new Rect (4,4,215,40), healthBar100, ScaleMode.StretchToFill, true,1.0f);
			
		}
		
		if ( health <= 90 && health > 80)
		{
			GUI.DrawTexture (new Rect (4,4,215,40), healthBar90, ScaleMode.StretchToFill, true,1.0f);
			
		}
		
		if ( health <= 80 && health > 70)
		{
			GUI.DrawTexture (new Rect (4,4,215,40), healthBar80, ScaleMode.StretchToFill, true,1.0f);
			
		}
		
		if ( health <= 70 && health > 60)
		{
			GUI.DrawTexture (new Rect (4,4,215,40), healthBar70, ScaleMode.StretchToFill, true,1.0f);
			
		}
		
		if ( health <= 60 && health > 50)
		{
			GUI.DrawTexture (new Rect (4,4,215,40), healthBar60, ScaleMode.StretchToFill, true,1.0f);
			
		}
		
		if ( health <= 50 && health > 40)
		{
			GUI.DrawTexture (new Rect (4,4,215,40), healthBar50, ScaleMode.StretchToFill, true,1.0f);
			
		}
		
		if ( health <= 40 && health > 30)
		{
			GUI.DrawTexture (new Rect (4,4,215,40), healthBar40, ScaleMode.StretchToFill, true,1.0f);
			
		}
		
		if ( health <= 30 && health > 20)
		{
			GUI.DrawTexture (new Rect (4,4,215,40), healthBar30, ScaleMode.StretchToFill, true,1.0f);
			
		}
		
		if ( health <= 20 && health > 10)
		{
			GUI.DrawTexture (new Rect (4,4,215,40), healthBar20, ScaleMode.StretchToFill, true,1.0f);
			
		}
				
		if ( health <= 10 && health > 0)
		{
			GUI.DrawTexture (new Rect (4,4,215,40), healthBar10, ScaleMode.StretchToFill, true,1.0f);
			
		}
		
		if ( health <= 0)
		{
			GUI.DrawTexture (new Rect (4,4,215,40), healthBar0, ScaleMode.StretchToFill, true,1.0f);
		}

		GUI.DrawTexture(new Rect (4,50,25,40), baconnaise, ScaleMode.StretchToFill, true,1.0f);
		
		GUI.DrawTexture(new Rect (100,50,45,40), bonusPower, ScaleMode.StretchToFill, true,1.0f);
		
			
		GUI.Box ( new Rect(35,50,40,40), collectableCounter1);
			
		GUI.Box ( new Rect(150,50,40,40), collectableCounter2);
			
		GUI.Box ( new Rect(Screen.width - 205,4,200,40),playerScore);
		

				
		}
		
				//End Of Game and Credits
		if ( Application.loadedLevel == 4)
		{
			GUI.DrawTexture (new Rect (0,0,Screen.width,Screen.height), display, ScaleMode.StretchToFill, true,0.5f);
		}
	}
	
	void Update()
	{
		if ( Application.loadedLevel != 0 && Application.loadedLevel != 4)
		{
					//Updates the score in the hud and end of game
		playerScore = GetComponent<MasterGO>().playerScore.ToString();
		collectableCounter1 = GetComponent<MasterGO>().collectableCounter1.ToString();	
		collectableCounter2 = GetComponent<MasterGO>().collectableCounter2.ToString();
		health = GetComponent<MasterGO>().playerHealth;
		}
		
		if ( Application.loadedLevel == 4)
		{

		if (startedCredits = false)
			{
				startedCredits = true;
				creditsStartTime = Time.time;
				
			}
		
		//End of Game Screen
		if (levelTime >= 0 && levelTime <= creditsStartTime+135)
		{
			display = thankYouScreen;
		}
			
//		if (levelTime >= 11 && levelTime <= creditsStartTime+21)
//		{
//			display = thankYouScreen;
//		}
//			
//		if (levelTime >= 22 && levelTime <= creditsStartTime+32)
//		{
//			display = thankYouScreen;
//		}
//			
//		if (levelTime >= 33 && levelTime <= creditsStartTime+43)
//		{
//			display = thankYouScreen;
//		}
			
//		if (levelTime >= 44 && levelTime <= creditsStartTime+50)
//		{
//			display  = thankYouScreen;
//		}
//		
//		if ( levelTime >= creditsStartTime+ 51 && levelTime <= creditsStartTime+ 57)
//		{
//			
//			display = credit001;
//		}
//		
//		if ( levelTime >= creditsStartTime+ 58 && levelTime <= creditsStartTime+ 64)
//		{
//			display = credit002;
//		}
//		
//		if ( levelTime >= creditsStartTime+ 65&& levelTime <= creditsStartTime+ 69)
//		{
//			display = credit003;
//		}
//		
//		if ( levelTime >= creditsStartTime+ 70 && levelTime <= creditsStartTime+ 76)
//		{
//			display = credit004;
//		}
//				
//		if ( levelTime >= creditsStartTime+ 77 && levelTime <= creditsStartTime+ 83)
//		{
//			display = credit005;
//		}
//				
//		if ( levelTime >= creditsStartTime+ 84 && levelTime <= creditsStartTime+ 90)
//		{
//			display = credit006;
//		}
//		
//		if ( levelTime >= creditsStartTime+ 91 && levelTime <= creditsStartTime+ 97)
//		{
//			display = credit007;
//		}
//		
//		if ( levelTime >= creditsStartTime+ 98 && levelTime <= creditsStartTime+ 104)
//		{
//			display = credit008;
//		}
//		
//		if ( levelTime >= creditsStartTime+ 105 && levelTime <= creditsStartTime+ 111)
//		{
//			display = credit009;
//		}
//		
//		if ( levelTime >= creditsStartTime+ 112 && levelTime <= creditsStartTime+ 118)
//		{
//			display = credit010;
//		}
//		
//		if ( levelTime >= creditsStartTime+ 119 && levelTime <= creditsStartTime+ 125)
//		{
//			display = credit011;
//		}
//		
//		if ( levelTime >= creditsStartTime+ 126 && levelTime <= creditsStartTime+ 132)
//		{
//			display = credit012;
//		}
//			
//		if ( levelTime >= creditsStartTime+ 133 && levelTime <= creditsStartTime+ 139)
//		{
//			display = credit013;
//		}
		
		if ( levelTime >= creditsStartTime+136)
		{
			audio.Stop();
			startedCredits = false;
			GameObject.FindWithTag("MasterGO").GetComponent<MasterGO>().collectableCounter1 = 0;
			GameObject.FindWithTag("MasterGO").GetComponent<MasterGO>().collectableCounter2 = 0;
			Application.LoadLevel(0);
		}
		
			levelTime = Time.time;
		}
	}
}
