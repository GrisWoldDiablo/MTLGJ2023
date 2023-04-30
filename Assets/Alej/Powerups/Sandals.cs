using System.Collections;
using UnityEngine;

public class Sandals : Powerup
{
	[SerializeField] private float speedBuff = 2f;
	[SerializeField] private float timer = 5.0f;

	public override void PickUp(Character character)
	{
		character.ModifySpeed(speedBuff,timer);
		Destroy(gameObject.transform.parent.gameObject);
	}

	private void OnDestroy()
	{
		StopAllCoroutines();
	}
}
