using UnityEngine;
using System.Collections;

public class MainMenuController : MonoBehaviour {
	
	
	public bool moved = false;
	public int currentButton = 1;
	
	public GameObject Button1;
	public GameObject Button2;
	public GameObject Button3;
	
	private button button1;
	private button button2;
	private button button3;
	
		
	// Use this for initialization
	void Start () {
		button1 = Button1.GetComponent<button>();
		button2 = Button2.GetComponent<button>();
		button3 = Button3.GetComponent<button>();
		UpdateButtons();
	}
	
	// Update is called once per frame
	void Update () {
		if (Application.loadedLevel == 0)
		    {
		if (Input.GetAxis("Vertical") > 0.3 && moved == false)
		{
			MoveUp();
			moved = true;
		}
		else if (Input.GetAxis("Vertical") < 0.3 && Input.GetAxis("Vertical") > -0.3)
		{
			moved = false;
		}
		else if (Input.GetAxis("Vertical") < 0.3 && moved == false)
		{
			MoveDown();
			moved = true;
		}
		
		if (Input.GetButtonDown("Jump"))
		{
			Application.LoadLevel(1);
		}
		}
	}
	
	
	
	public void MoveUp()
	{
		if (currentButton > 1)
		{
			currentButton -= 1;
			UpdateButtons();
		}
	}
	
	public void MoveDown()
	{
		if (currentButton < 3)
		{
			currentButton += 1;	
			UpdateButtons();
		}
	}
	
	public void UpdateButtons()
	{
		if (currentButton == 1)
		{
			button1.stage = "over";
			button2.stage = "start";
			button3.stage = "start";
		}
		else if (currentButton == 2)
		{
			button1.stage = "start";
			button2.stage = "over";
			button3.stage = "start";
		}
		else
		{
			button1.stage = "start";
			button2.stage = "start";
			button3.stage = "over";	
		}
	}
}


