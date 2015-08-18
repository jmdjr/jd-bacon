
public var healthMax : int;
public var healthCurrent = 0;

public var MasterGO : GameObject;

public var ourModel : GameObject;

function Awake ()
{
	healthCurrent = healthMax;
	MasterGO = GameObject.FindGameObjectWithTag("MasterGO");
	
}

function Update () 
{

	if (healthCurrent <= 0) {
	MasterGO.GetComponent("MasterGO").playerScore += 10;
	DestroySelf ();
	}

}

public function TakeDamage (damage)
{
	healthCurrent -= damage;
	Debug.Log(healthCurrent);

}

function DestroySelf ()
{
	Destroy(transform.gameObject);
}