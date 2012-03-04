using UnityEngine;
using System.Collections;

public class pausemenu : MonoBehaviour
{
	private int centerX;
	private int buttonCenter;
	public int boxWidth;
	public int boxHeight;
	public int buttonWidth;
	public int buttonHeight;
	public int spacing;
	public Font fonts;
	public Texture pauseTexture;
	public Texture xboxTexture;
	public Texture keyboardTexture;
	public Texture backButtonTexture;
	public Texture nextButtonTexture;
	private string state = "start";

	
	void OnGUI() 
	{
	if (Application.loadedLevel != 0)
	{
		if (Input.GetKeyDown("escape"))
		{ 
			state = "go";
		}
			
		if (state == "go")
		{ 
			Time.timeScale = 0;
				
			GUI.skin.font = fonts;
		
			centerX = Screen.width/2 - boxWidth/2;
			buttonCenter = Screen.width/2 - buttonWidth/2;

			// Make a background box
			GUI.DrawTexture (new Rect (centerX,10,boxWidth,boxHeight), pauseTexture, ScaleMode.StretchToFill, true,0.5f);
		

			// Make the first button.
			if (GUI.Button (new Rect (buttonCenter,75,buttonWidth,buttonHeight), "RESUME GAME")) 
			{
					Time.timeScale = 1f;
					state = "start";
			}

			// Make the second button.
			if (GUI.Button ( new Rect (buttonCenter,135,buttonWidth,buttonHeight), "HELP AND OPTIONS")) 
				{
					state = "controllerpause";
				}
		
			// Make the third button.
			if (GUI.Button ( new Rect (buttonCenter,195,buttonWidth,buttonHeight), "EXIT GAME")) 
				{
					Application.Quit();
				}
			}
		
		if (state == "keyboardpause")
		{
			GUI.skin.font = fonts;
		
			centerX = Screen.width/2 - boxWidth/2;
			buttonCenter = Screen.width/2 - buttonWidth/2;

			// Make a background box
			GUI.DrawTexture (new Rect (0,0,Screen.width,Screen.height), keyboardTexture, ScaleMode.StretchToFill, true,0.5f);
			
			//Makes the back button
			if (GUI.Button ( new Rect (Screen.width -200,Screen.height - 75,174,66), backButtonTexture)) 
				{
					state = "controllerpause";
				}
		}
		
		
		if (state == "controllerpause")
		{
			GUI.skin.font = fonts;
		
			centerX = Screen.width/2 - boxWidth/2;
			buttonCenter = Screen.width/2 - buttonWidth/2;

			// Make a background box
			GUI.DrawTexture (new Rect (0,0,Screen.width,Screen.height), xboxTexture, ScaleMode.StretchToFill, true,0.5f);
			
			//Makes the back button
			if (GUI.Button ( new Rect (Screen.width -200,Screen.height - 75,174,66), backButtonTexture)) 
				{
					state = "go";
				}
						//Makes the back button
			if (GUI.Button ( new Rect (Screen.width -400,Screen.height - 75,174,66), nextButtonTexture)) 
				{
					state = "keyboardpause";
				}
		}
	}
	}

}

