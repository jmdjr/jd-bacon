
function OnTriggerEnter(obj : Collider) 
{
	if (obj.gameObject.CompareTag("Player"))
	{
		var temp = obj.GetComponent(CrushVictim).inCrushZone;
		
		if (temp == 1)
		{
			obj.GetComponent(CharacterMotor).OnDeath();
		}
	}
}