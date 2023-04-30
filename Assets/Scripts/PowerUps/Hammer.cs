using UnityEngine;

public class Hammer : Powerup
{

	public override void PickUp(Character character)
	{
		character.ReceivePowerUp(Character.PowerUp.Hammer);
		Destroy(gameObject.transform.parent.gameObject);
	}
}
