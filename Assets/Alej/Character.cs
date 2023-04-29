using UnityEngine;

public class Character : MonoBehaviour
{
	[SerializeField] private int _health;
	[SerializeField] private float _speed;

	public int Health { get => _health; set => _health = value; }
	public float Speed { get => _speed; set => _speed = value; }

	public void ModifyHealth(int damage)
	{
		_health += damage;
	}

	public void ModifySpeed(float modification)
	{
		_speed += modification;
	}
}
