using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour {

	public enum AIStates
	{
		Idle,
		ChasePlayer,
		AttackPlayer
	}
	
	public GameObject node1;
	public GameObject node2;
	public bool stationary;
	
	public AIStates myState;
	private GameObject player;
	
	public GameObject ourModel;
	
	public GameObject HippyHitZone;
	
	private Object blastZone;
	
	public float fSpeed;
	public float chaseDistance = 10f;
	public float attackDistance = 4.5f;
	private float moveRate;
	
	private EnemyInputController input;
	
	private float hippySwingStart;
	private int hippySwingStarted;
	private float hippySwung;
	
		
	void Awake ()
	{
		player = GameObject.FindWithTag("Player");
		input = gameObject.GetComponent<EnemyInputController>();
	}
	
//	IEnumerator Start () 
//	{
//		while(true)
//		{
//
//						
//			}
//			yield return new WaitForSeconds(0);
//		}	
//	}
	
	private void Idle()
	{
		input.directionVector = new Vector3(0,0,0);	
		
		if (PlayerDistance() < chaseDistance)
		{
			myState = EnemyAI.AIStates.ChasePlayer;
		}
	}
	
	private void ChasePlayer()
	{
	

		input.directionVector = new Vector3(moveRate,0,0);
		if (!ourModel.animation.IsPlaying("hippyhit"))
		{
		ourModel.animation.Play("hippywalk");
		}
		
		
		if (PlayerDistance() > chaseDistance)
		{
			myState = EnemyAI.AIStates.Idle;
		}
		else if (PlayerDistance() < attackDistance)
		{
			myState = EnemyAI.AIStates.AttackPlayer;
		}
	}
	
	private void AttackPlayer()
	{

	input.directionVector = new Vector3(0,0,0);
		if (!ourModel.animation.IsPlaying("hippyhit"))
		{
			//ourModel.animation.Play("hippyattack");
		}
		
		if (hippySwingStarted == 0)
		{
		hippySwingStarted = 1;
		hippySwingStart = Time.time;
		
		}
		
		if (hippySwingStarted == 1 && Time.time - hippySwingStart > 0.27)
		{
			hippySwingStarted = 2;
			HippySwing();	
		}
		
		if (hippySwingStarted == 2 && Time.time - hippySwingStart > 1)
		{
			hippySwingStarted = 0;	
		}
		
		//Debug.Log("attacking");
		
		if (PlayerDistance() > attackDistance)
		{
			myState = EnemyAI.AIStates.ChasePlayer;
		}
	}
	
	private float PlayerDistance ()
	{
		return Vector3.Distance(transform.position, player.transform.position);
	}
	
	// Update is called once per frame
	void Update () 
	{
		setDirection();
		if (gameObject.transform.position.z != 0)
		{
		gameObject.transform.position = new Vector3(transform.position.x,transform.position.y, 0);
		}
		
		
		
		switch (myState)
		{
		case AIStates.Idle:
				Idle();	
				break;
		case AIStates.ChasePlayer:
				ChasePlayer();
				break;
		case AIStates.AttackPlayer:
				AttackPlayer();
				break;
		}	
	}
	
	private void setDirection()
	{
			if (player.transform.position.x > gameObject.transform.position.x)
		{
			moveRate = 1f;
		}
		else
		{
			moveRate = -1f;	
		}	
	}
	
private void HippySwing ()
{
		
		
		GameObject blastZone = Instantiate(HippyHitZone, transform.position,Quaternion.Inverse(transform.rotation)) as GameObject;

			
	    blastZone.GetComponent<HitZonerEnemy>().damage(10);
	    blastZone.transform.position = transform.position;
		blastZone.transform.parent = transform;
		blastZone.transform.Translate(Vector3.forward * -1);
		blastZone.transform.Translate(Vector3.up);
	
}
	
	public void GotHit()
	{
		//ourModel.animation.Stop("hippyhit");	
		//ourModel.animation.Play("hippyhit", PlayMode.StopAll);
	}
}
