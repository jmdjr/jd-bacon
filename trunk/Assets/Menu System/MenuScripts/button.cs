using UnityEngine;
using System.Collections;

public class button : MonoBehaviour {
	
	// Variables
	private float X;
	private float Y;
	private float Z;
	public string stage = "start";
	private Vector3 position;
	public float distance;
	public int level;
	public GameObject buttonName;
	public int buttonIndex;
	public GameObject Button1;
	public GameObject Button2;
	public GameObject Button3;
	private button button1;
	private button button2;
	private button button3;
	


	// Use this for initialization
	void Start () 
	{
		button1 = Button1.GetComponent<button>();
		button2 = Button2.GetComponent<button>();
		button3 = Button3.GetComponent<button>();
	
		X = buttonName.transform.position.x;
		Y = buttonName.transform.position.y;
		Z = 0.0f;		
	}
	
	//When the mouse is over
	void OnMouseOver()
	{
		stage = "over";
		UpdateButtons(buttonIndex);
	}
	
	//When the mouse exits
	void OnMouseExit()
	{
		stage = "start";	
	}
	
	//When the mouse button is released
	void OnMouseUp()
	{
		if (level > 0)
		{
			Application.LoadLevel(level);
		}
		 else if (level == -1)
		{
		Application.Quit();
			print("QUIT");
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		if (stage == "start")
		{
			//Z = 0.0f;
			buttonName.transform.localScale = new Vector3(1,1,1);
		}
		
		if (stage == "over" )
		{
			buttonName.transform.localScale = new Vector3(distance, distance, distance);
			//Z = distance * 0.1f;

		}
		
		//X = buttonName.transform.position.x;
		//Y = buttonName.transform.position.y;
		//buttonName.transform.position = new Vector3(X, Y, Z);
		
	}
	
	
	public void UpdateButtons(int index)
	{
		if (index == 1)
		{
			button1.stage = "over";
			button2.stage = "start";
			button3.stage = "start";
		}
		else if (index == 2)
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

