using System;
using UnityEngine;

public class Character : MonoBehaviour
{
	public bool KILLME = false;
	[SerializeField] private int _startingHeath = 3;
	[SerializeField] private bool _isInvincible = false;
	[SerializeField] private Sprite[] _possibleItems;
	[SerializeField] private SpriteRenderer itemSlot;
	private int _health = 0;
	private bool _isDead = false;
	[SerializeField]private bool _hasHammer = false;
	public bool HasHammer => _hasHammer;
	public event Action<int> OnHealthChange;
	public event Action OnDie;
	
	public int Health
	{
		get => _health;
		set => _health = value;
	}

	public bool IsDead => _isDead;

	private PlayerMovement _playerMove;
	public PlayerMovement PlayerMovement => _playerMove;

	private void Start()
	{
		_playerMove = GetComponentInChildren<PlayerMovement>();
		ModifyHealth(_startingHeath);
	}
	
	private void Update()
	{
		if (KILLME || Input.GetKeyDown(KeyCode.P))
		{
			KILLME = false;
			ModifyHealth(-_health);
		}
		if (HasHammer)
		{
			float angle = Mathf.PingPong(Time.time*50, 30)-30;
			itemSlot.gameObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		}
	}
	
	public void GetHit()
	{
		if (HasHammer)
		{
			itemSlot.sprite = null;
			_hasHammer = false;
		}
		else
		{
			ModifyHealth(-1);
		}
	}

	public void ModifyHealth(int damage)
	{
		if (_isInvincible || _isDead)
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
		_isDead = true;
		OnDie?.Invoke();
	}

	public enum PowerUp 
	 {
		Bread,Hammer,Fan
	 }
	 public void ReceivePowerUp(PowerUp power)
	{
		switch (power)
		{
			case PowerUp.Bread:
				
				break;
			case PowerUp.Hammer:
				itemSlot.sprite = _possibleItems[0];
				_hasHammer = true;
				
				break;
			case PowerUp.Fan:
				
				break;
		}
	}

	public void ModifySpeed(float modification,float timer)
	{
		PlayerMovement.ModifySpeed(modification,timer);
	}
}