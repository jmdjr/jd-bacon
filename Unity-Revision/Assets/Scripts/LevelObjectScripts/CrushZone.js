
function OnTriggerEnter (obj : Collider) 
{
	if (obj.CompareTag("Player"))
	{
		obj.GetComponent(CrushVictim).inCrushZone = 1;
	}
}

function OnTriggerExit (obj : Collider)
{
	if (obj.CompareTag("Player"))
	{
		obj.GetComponent(CrushVictim).inCrushZone = 0;
	}
}