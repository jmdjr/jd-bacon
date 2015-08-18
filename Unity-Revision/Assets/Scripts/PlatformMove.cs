using UnityEngine;
using System.Collections;

public class PlatformMove : MonoBehaviour 
{
	//this class script is designed for moving a platform
	
	//Unity Editor Values to be seen:
	//Node1 is the first position for our platform to move to
	public Transform Node1;
	//Node2 is the second position for our platform to move to
	public Transform Node2;
	
//Hypothetical:
//if we're going to use more than 2 points, we'll just create an array of Transforms
//public Transform[] NodeArray;
//this would allow us to have multiple destinations for the platform
	
	//moveSpeed is the rate at which the platform will move
	public float moveSpeed = 0.1f;
	//stopDelay is used to delay movement once a node is reached
	public float stopDelay = 1.0f;
	//control whether the platform moves or not manually with this boolean
	public bool bMovePlatform = true;

	//Code Values to be used within the class privately:
	//_vPos1 is where we will store the Vector3 position of Node1
	private Vector3 _vPos1;
	//_vPos2 is where we will store the Vector3 position of Node2
	private Vector3 _vPos2;
	//	we could use NodeX.position (instead of _vPosX), but _vPosX is faster for scripting purposes
	
	//we'll use this variable to keep track of our current platform target...
	//0 = Node1
	//1 = Node2
	private int iCurrentPlatform = 0;

//Hypothetical:
//if we wanted to have multiple nodes for our platform to move along
//	we would need a boolean to keep track of going up or down the list,
//	for the sake of simplicity, it would be like this:
// 	private bool _bForward = true;
	
	public void Awake()
	{
		_vPos1 = Node1.position;
		_vPos2 = Node2.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(bMovePlatform)
		{
			//store the current platform's position for reference
			Vector3 currentPosition = transform.position;
			//create a dummy variable for our destination Vector 3
			Vector3 destinationPosition = new Vector3(0,0,0);
			
			//if we're traveling to Node1
			if(iCurrentPlatform == 0)
			{
				//set our destinationPosition to the Vector3 position of Node1
				destinationPosition = _vPos1;
			}
			//else if we're traveling to Node2
			else if(iCurrentPlatform == 1)
			{
				//set our destinationPosition to the Vector3 position of Node2
				destinationPosition = _vPos2;
			}
			
//Hypothetical:
//supposing we're using more than one node in a transform array
//destinationPosition = NodeArray[iCurrentPlatform].position
//this will also change what happens upon arrivial
			
			//determine the difference between the current position and desired position,
			//	using simple subtraction of Vector3's
			Vector3 vDifference = destinationPosition - currentPosition;
			
			//make the vDifference equal a value of 1
			vDifference.Normalize();
			
			//multiply our movespeed by the value of vDifference
			vDifference *= moveSpeed;
			//add the projected distance to travel to our current position
			currentPosition += vDifference;
			
			//before we check for the validity of our movement,
			//let's assume we'll be arriving at our destination
			bool bArrived = true;
			
			//check the current position's x Value

			//are we even moving in the X Axis?
			//first, check if the vDifferent's X value is greater than 0
			if(vDifference.x > 0)
			{
				//is our destination X Axis position above the current X Axis Position?
				if(destinationPosition.x > currentPosition.x)
				{
					bArrived = false;
				}
			}
			//if the vDifference value is less than 0
			else if(vDifference.x < 0)
			{
				//is our destination beneath the current X Axis position?
				if(destinationPosition.x < currentPosition.x)
				{
					bArrived = false;
				}
			}
			
			//check for movement in the Y Axis (same notes as X Axis)
			if(vDifference.y > 0)
			{
				if(destinationPosition.y > currentPosition.y)
				{
					bArrived = false;
				}
			}
			else if(vDifference.y < 0)
			{
				if(destinationPosition.y < currentPosition.y)
				{
					bArrived = false;
				}
			}
			
			//check for movement in the Z Axis (same notes as X Axis)
			if(vDifference.z > 0)
			{
				if(destinationPosition.z > currentPosition.z)
				{
					bArrived = false;
				}
			}
			else if(vDifference.z < 0)
			{
				if(destinationPosition.z < currentPosition.z)
				{
					bArrived = false;
				}
			}
			
			//if the current position checks have all matched the destination position
			if(bArrived)
			{
				//call for the platform movement to be stopped for stopDelay seconds
				StopThePlatform();

				//set our currentPosition value to our destinationPosition
				currentPosition = destinationPosition;
				
				
				//this is our current setup for only 2 node points
				//if the current platform we're moving to is _vPos1
				if(iCurrentPlatform == 0)
				{
					//set the next position we're looking for to _vPos2
					iCurrentPlatform = 1;
				}
				//else if the current platform we're moving to is _vPos2
				else if(iCurrentPlatform == 1)
				{
					//set the next position we're looking for to _vPos2
					iCurrentPlatform = 0;
				}

//Hypothetical:
//if we were using more than one node:
//if(_bForward)
//{
//	if(iCurrentPlatform < NodeArray.Length)
//	{
//		iCurrentPlatform++;
//	}
//	else
//	{
//		iCurrentPlatform--;
//		_bForward = false;
//	}
//}
//else if(!_bForward)
//{
//	if(iCurrentPlatform > 0)
//	{
//		iCurrentPlatform--;
//	}
//	else
//	{
//		iCurrentPlatform++;
//		_bForward = true;
//	}
			}
			
			//update the platform's transform position to the currentPosition
			transform.position = currentPosition;
			// *note* 
			//		this update in position is outside of the if(bArrived) check
			//		it just looks out of place, because of the hypothetical situation
			//		that uses an array for node movement instead of only 2 nodes		
			// /*note*
			
		}
	}
	
	
	//use an IEnumerator to wait for a return value
	private IEnumerator StopThePlatform()
	{
		//stop the updates for moving our platform
		bMovePlatform = false;
		//delay the return value for stopDelay seconds
		yield return new WaitForSeconds(stopDelay);
		//allow the platform to be moved again
		bMovePlatform = true;
	}
}
