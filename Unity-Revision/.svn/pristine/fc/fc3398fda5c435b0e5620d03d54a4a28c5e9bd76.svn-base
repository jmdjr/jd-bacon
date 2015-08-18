using UnityEngine;
using System.Collections;

public class ClassCollectable : MonoBehaviour {
public int sodaHP;
private int tempHP;
	
	// Use this for initialization
	public void Start () 
	{
		//Counter Tracking
		//GameObject.FindWithTag("CollectableHandler").GetComponent<CollectableHandler>().Total++;
	}
	public void OnTriggerEnter(Collider obj)
	{
		tempHP = GameObject.FindWithTag("MasterGO").GetComponent<MasterGO>().playerHealth;
		if(obj.CompareTag("Player"))
		{
			GameObject.FindWithTag("MasterGO").GetComponent<MasterGO>().collectableCounter1++;
			GameObject.FindWithTag("MasterGO").GetComponent<MasterGO>().collectableCounter2++;
			//if(this.CompareTag("Soda"))
			//{
				if(GameObject.FindWithTag("MasterGO").GetComponent<MasterGO>().playerHealth >= 100){
				;
				}
				else{
		    		tempHP = tempHP + 10;
					GameObject.FindWithTag("MasterGO").GetComponent<MasterGO>().addHealth(tempHP);
				}
			//}
			Destroy(gameObject);
		}
	}
	
	
}
