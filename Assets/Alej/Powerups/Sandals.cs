using System.Collections;
using UnityEngine;

public class Sandals : Powerup
{
	[SerializeField] private float speedBuff = 10.0f;
	[SerializeField] private float timer = 5.0f;

	public override void PickUp(Character character)
	{
		character.ModifySpeed(speedBuff);
	}

	private void OnDestroy()
	{
		StopAllCoroutines();
	}
}
