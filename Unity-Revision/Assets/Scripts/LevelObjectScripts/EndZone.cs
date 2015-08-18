using UnityEngine;
using System.Collections;

public class EndZone : MonoBehaviour 
{
	//the index number for the scene to load
	public int LeveltoLoad = 0;
	public AudioClip WNRsound;
	// OnTriggerEnter is called when an object enters the Trigger Region
	public void OnTriggerEnter (Collider obj) 
	{
		//if the object entering our trigger is the player
		if(obj.CompareTag("Player"))
		{
			audio.PlayOneShot(WNRsound);
			//load the level defined in the Unity Editor
			Application.LoadLevel(LeveltoLoad);
		}
	}
}
