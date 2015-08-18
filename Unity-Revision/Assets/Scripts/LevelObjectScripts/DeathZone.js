
var DeadSound : AudioClip;

function OnTriggerEnter (obj : Collider) 
{
	var other : CharacterMotor = obj.GetComponent(CharacterMotor);
	audio.PlayOneShot(DeadSound);
	//other.OnDeath();
}