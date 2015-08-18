

function Update () {

	if (Input.GetButtonDown("Fire1"))
	{
		Debug.Log("Fired 1");
		swordSwinging1 = 1;
		
	}
	
	if (swordSwinging1)
	{
		animation.Play ("1h_attack2");
		if (!animation.IsPlaying("mouseOverEffect"))
		{
			Debug.Log("equals 0!");
			swordSwinging1 = 0;
		}
		//animation.CrossFade("jump_pose");
	}
}