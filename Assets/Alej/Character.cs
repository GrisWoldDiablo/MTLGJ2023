using UnityEngine;

public class Character : MonoBehaviour
{
	[SerializeField] private int _health;

	public int Health { get => _health; set => _health = value; }

	public PlayerMovement PlayerMovement { get => GetComponent<PlayerMovement>(); }

	public void ModifyHealth(int damage)
	{
		_health += damage;
	}

	public void ModifySpeed(float modification)
	{
		PlayerMovement.Speed += modification;
	}
}
