private var other;
private var damageAmount;
private var created;
private var direction;

private var player : GameObject;

public var masterGO : GameObject;


function Awake ()
{
masterGO = GameObject.FindGameObjectWithTag("MasterGO");
created = Time.time;
}

function Update ()
{
	if (Time.time - created > 0.15)
	{
		Destroy(gameObject);
	}
}

function damage (damageInput : int)
{
	damageAmount = damageInput;
	
	//player = GameObject.FindWithTag("Player");
}

function OnTriggerEnter (other : Collider) 
{
    if (other.gameObject.tag == "Player")
    {
    	masterGO.gameObject.GetComponent("MasterGO").playerHealth -= damageAmount;
    	
    	Debug.Log(damageAmount.ToString());
    	
    	var other2 = GameObject.FindGameObjectWithTag("PlayerModel");
    	other2.GetComponent("MyCharController").GotHit();
    	
    	if (other2.GetComponent("MyCharController").IsLiving())
    	{
	    	if (other.transform.position.x > transform.position.x)
	    	{
	    		other.transform.position.x = other.transform.position.x + 0.5;
	    	}
	    	else
	    	{
	    		other.transform.position.x = other.transform.position.x - 0.5;
	    	}
    	}
    }
}


