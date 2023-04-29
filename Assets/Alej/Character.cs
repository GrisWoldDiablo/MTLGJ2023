using System;
using UnityEngine;

public class Character : MonoBehaviour
{
	[SerializeField] private int _startingHeath = 3;
	[SerializeField] private bool _isInvincible = false;

	private int _health = 0;
	private bool isDead = false;

	public event Action<int> OnHealthChange;
	
	public int Health
	{
		get => _health;
		set => _health = value;
	}

	public bool IsDead => isDead;

	public PlayerMovement PlayerMovement => GetComponent<PlayerMovement>();

	private void Start()
	{
		ModifyHealth(_startingHeath);
	}

	public void ModifyHealth(int damage)
	{
		if (_isInvincible)
		{
			return;
		}

		_health += damage;
		if (_health <= 0)
		{
			Die();
		}
		
		OnHealthChange?.Invoke(damage);
	}

	void Die()
	{
		//game manager end game
		isDead = true;
	}

	public void ModifySpeed(float modification)
	{
		PlayerMovement.Speed += modification;
	}
}