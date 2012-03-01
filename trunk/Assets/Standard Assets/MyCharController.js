var hor;
var temp;
var exBaconHitZone : GameObject;
var ShotgunBlastZone : GameObject;

private var runTime : float;
private var walkTime : float;
private var idleTime : float;
private var jumpTime : float;
private var moveState = 1000;

public var lastDirection = 1; // 1 = left 2 = right.
public var thePlayer : GameObject;
public var shotEmitter : GameObject;
public var shotEmitter2 : GameObject;
public var theSword : GameObject;
public var theGun : GameObject;


private var shooting = 1;
private var swordSwinging1 = 0;
private var swordSwinging2 = 0;
private var swordSwung1 = 0;
private var swordHit1 = 0;
private var swordHit2 = 0;
private var swingTime : float = 0.0001;
private var shotCooldown : float = 0.02;

private var meleeHitDelay = 0.27;

private var MELEE1 		= "baconslash1";
private var MELEE2 		= "baconslash2";
private var RUN			= "baconrun";
private var WALK		= "baconwalk";
private var JUMP		= "baconjump";
private var JUMP_POSE	= "baconjumphold";
private var IDLE		= "baconidle";
private var SHOTGUN		= "baconShoot2";

public var  living = 1;

private var died = 0;

function awake () {

//thePlayer = GameObject.FindGameObjectWithTag("player");

animation[MELEE1].layer = 1;
animation[MELEE1].wrapMode = WrapMode.Once;
animation[MELEE2].layer = 1;
animation[MELEE2].wrapMode = WrapMode.Once;
animation[IDLE].layer = 0;

living = 1;
died = 0;

}


function Update () 
{
		
		hor = Input.GetAxis("Horizontal");

		if (hor > 0.1)
		{
			lastDirection = 2;
		}
		else if (hor < -0.1)
		{
			lastDirection = 1;
		}

//animation.CrossFade ("idle1");
	Debug.Log(living);
	if (living == 1)
	{
		if (!animation.IsPlaying("baconHit"))
		{
			Shooting();
			SwordSwing();
			AnimationLayer1 ();
			
			if (!swordSwinging1 && !swordSwinging2 && !shooting)
			{
				AnimationLayer0 ();
			}
		}
	}
}

function MeleeHitCheck ()
{
	var swingZone = GameObject.Instantiate(exBaconHitZone);
    swingZone.GetComponent("HitZoner").damage(6);
    swingZone.transform.position = transform.position;
	swingZone.transform.parent = transform;
	swingZone.transform.localPosition = (Vector3.forward * 90);
	swingZone.transform.position += Vector3(0,1.25,0);
	
	
	
	//Destroy(swingZone);
	
}

// ACTION ANIMATIONS***********************************
function AnimationLayer1 ()
{

	if (Input.GetButtonDown("Fire2"))
	{
		
		if (Time.time - shotCooldown > 0.5)
		{
			shotCooldown = Time.time;
			shooting = 1;
			animation.Play(SHOTGUN);
			
			PopAttack();
			
			shotEmitter.particleEmitter.Emit(70);
			shotEmitter2.particleEmitter.Emit(400);
			
			moveState = 10;
			ShotgunHitCheck();
		}
	}

	else if (Input.GetButtonDown("Fire1"))
	{
		if (!swordSwinging1) // Load up swing one.
		{
			theSword.renderer.enabled = true;
			theGun.renderer.enabled = false;
			swordSwinging1 = 1;
			animation.Play(MELEE1);
			PopAttack();
			swingTime = Time.time;
			moveState = 10;
		}
		else if (!swordSwinging2) // Load up swing two if swing one is already happening.
		{
			swordSwinging2 = 1;
			animation.PlayQueued(MELEE2, QueueMode.CompleteOthers);
		}
	}
	
	
	if (swordSwinging2 && swordSwinging1)
		{
			if (!animation.IsPlaying(MELEE2) && !animation.IsPlaying(MELEE1))
			{
				theGun.renderer.enabled = true;
				theSword.renderer.enabled = false;
				swordSwinging1 = 0;
				swordSwinging2 = 0;
			}
		}
	else if (swordSwinging1)
	{
		if (!animation.IsPlaying(MELEE1))
		{
			swordSwinging1 = 0;
			theSword.renderer.enabled = false;
			theGun.renderer.enabled = true;
		}
	}

	if (swordSwinging1 == 0 && swordSwinging2 == 0)
	{
			theSword.renderer.enabled = false;
			theGun.renderer.enabled = true;
	}
	
}
	
// OTHER ANIMATIONS ***************************
function AnimationLayer0 ()
{
	if (Input.GetButton("Jump")) {
		if (moveState != 3) {
		jumpTime = Time.time;
		moveState = 3;
	
		}
		else {
			if (Time.time - jumpTime < 0.2) {
				animation.CrossFade(JUMP);
			}
			else if (Time.time - jumpTime < 0.8) {
				animation.CrossFade(JUMP_POSE);
			}
			else {
				animation.Play (JUMP_POSE);
			}
		}
	
	
	}
	else if (hor > 0.7 || hor < -0.7) {
		if (moveState != 2) 
		{
		runTime = Time.time;
		moveState = 2;
		}
		
		

			if (Time.time - runTime < 0.1) {

				animation.CrossFade(RUN);
			}
			else {
				animation.PlayQueued (RUN);
			}

	}
	else if (hor > 0.2 || hor < -0.2) {
		if (moveState != 1) {
		walkTime = Time.time;
		moveState = 1;

		
		}
		else {
			if (Time.time - walkTime < .1) {
				animation.CrossFade(WALK);
			}
			else {
				animation.PlayQueued (WALK);
			}
		}
	}
	else {
		if (moveState != 0) {
		idleTime = Time.time;
		moveState = 0;
		}
		else {
			if (Time.time - idleTime < .1) {
				if (lastDirection == 1)
				{thePlayer.transform.eulerAngles.y = -145;}
				else if (lastDirection == 2)
				{thePlayer.transform.eulerAngles.y = 145;}
				animation.CrossFade(IDLE);
				theSword.renderer.enabled = false;
			}
			else {
				//if (lastDirection == 1)
				//{thePlayer.transform.eulerAngles.y = -145;}
				//else if (lastDirection == 2)
				//{thePlayer.transform.eulerAngles.y = 145;}
				animation.PlayQueued (IDLE);
			}
		}
	}
}

function SwordSwing ()
{

	if (swordSwinging1 == 1)
	{
		if (swordSwung1 == 0 && swordHit1 == 0 && Time.time - swingTime > meleeHitDelay && swordHit1 == 0)
		{
			MeleeHitCheck ();
			swordHit1 = 1;
			
		}
			
		if ((!animation.IsPlaying(MELEE1)) && swordSwinging2 == 1 && swordSwung1 == 0)
		{
			swordSwung1 = 1;
			swingTime = Time.time;			
		}
		
		else if ((!animation.IsPlaying(MELEE1)) && swordSwinging2 ==0)
			{
				swordSwinging1 = 0;
				swordSwung1 = 0;
				swordHit1 = 0;
				
			}
	}
	if (swordSwinging2 == 1 && swordSwung1 == 1)
	{
		if (swordHit2 == 0 && Time.time - swingTime > meleeHitDelay)
		{
			MeleeHitCheck ();
			swordHit2 = 1;
		}
		
		if (!animation.IsPlaying(MELEE2))
		{
			swordSwinging2 = 0;
			swordSwinging1 = 0;
			swordSwung1 = 0;
			swordHit1 = 0;
			swordHit2 = 0;
		}
	}
}

function Shooting()
{
	if (!animation.IsPlaying(SHOTGUN) && shooting)
	{
		shooting = 0;
	}
		
}

function ShotgunHitCheck ()
{
	var blastZone = GameObject.Instantiate(ShotgunBlastZone, transform.position,Quaternion.Inverse(transform.rotation));
	
    blastZone.GetComponent("HitZoner").damage(4);
    blastZone.transform.position = transform.position;
	blastZone.transform.parent = transform;
	blastZone.transform.localPosition = (Vector3.forward * .013);
	blastZone.transform.position += Vector3(0,1.65,0);
		
	
	//Destroy(blastZone);
	
}

function PopAttack ()
{
		if (lastDirection == 1)
		{thePlayer.transform.eulerAngles.y = -90;}
		else if (lastDirection == 2)
		{thePlayer.transform.eulerAngles.y = 90;}
}

function GotHit()
{
	if (living ==1)
	{animation.Play("baconHit");}
}

function Death()
{
	living = 0;
	if (died == 0)
	{
		animation.Play("baconDeath");
		died = 1;
	}
}

function IsLiving()
{
	if (living == 1)
	
	{return true;}
	if(living == 0)
	{return false;}
}