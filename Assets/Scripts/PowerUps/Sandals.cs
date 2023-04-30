using System.Collections;
using UnityEngine;

public class Sandals : Powerup
{
	[SerializeField] private float speedBuff = 1.8f;
	[SerializeField] private float timer = 1.0f;

	public override void PickUp(Character character)
	{
		character.ModifySpeed(speedBuff,timer);
		character.ModifyHealth(1);
		Destroy(gameObject.transform.parent.gameObject);
	}

	private void OnDestroy()
	{
		StopAllCoroutines();
	}
}
