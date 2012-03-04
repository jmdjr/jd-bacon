private var other;
private var damageAmount;
private var created;
private var direction;

private var player : GameObject;




function Update ()
{
	if (Time.time - created > 0.05)
	{
		Destroy(gameObject);
	}
}

function damage (damageInput : int)
{
	damageAmount = damageInput;
	created = Time.time;
	player = GameObject.FindWithTag("Player");
}

function OnTriggerEnter (other : Collider) 
{
    if (other.gameObject.tag == "Enemy")
    {
    	other.gameObject.GetComponent("EnemyController").healthCurrent -= damageAmount;
    	
    	other.GetComponent("EnemyAI").GotHit();
    	
    	if (other.transform.position.x > player.transform.position.x)
    	{
    		other.transform.position.x = other.transform.position.x + 2;
    	}
    	else
    	{
    		other.transform.position.x = other.transform.position.x - 2;
    	}
    }
}


