using System;
using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    public bool KILLME = false;
    [SerializeField] private int _startingHealth = 3;
    [SerializeField] private bool _isInvincible = false;
    [SerializeField] private Sprite[] _possibleItems;
    [SerializeField] private SpriteRenderer itemSlot;
    [SerializeField] private SpriteRenderer playerSprite;

    private int _health = 0;
    private bool _isDead = false;
    [SerializeField] private bool _hasHammer = false;
    public bool HasHammer => _hasHammer;
    public event Action<int> OnHealthChange;
    public event Action OnDie;

    public int Health
    {
        get => _health;
        set => _health = value;
    }

	private void Start()
	{
		_playerMove = GetComponentInChildren<PlayerMovement>();
		ModifyHealth(_startingHealth);
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
	
	public void GetHit(int damage)
	{
		if (HasHammer)
		{
			itemSlot.sprite = null;
			_hasHammer = false;
		}
		else
		{
			ModifyHealth(-damage);
            CharacterSFXManager.Get().PlayHurtSFX();
		}
	}

    private PlayerMovement _playerMove;
    public PlayerMovement PlayerMovement => _playerMove;

		_health += damage;
		if (_health <= 0)
		{
			Die();
		}
		if (_health > _startingHealth)
		{
			_health = _startingHealth;
		}
		OnHealthChange?.Invoke(damage);
	}
	
	void Die()
	{
		//game manager end game
		_isDead = true;
		OnDie?.Invoke();
        CharacterSFXManager.Get().PlayDieSFX();
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
            float angle = Mathf.PingPong(Time.time * 50, 30) - 30;
            itemSlot.gameObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    public void GetHit(int damage)
    {
        if (HasHammer)
        {
            itemSlot.sprite = null;
            _hasHammer = false;
        }
        else
        {
            ModifyHealth(-damage);
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
        else if(damage < 0)
        {
            ApplyStatusEffects();
        }
        if (_health > _startingHealth)
        {
            _health = _startingHealth;
        }
        OnHealthChange?.Invoke(damage);
    }

    void Die()
    {
        //game manager end game
        _isDead = true;
        OnDie?.Invoke();
    }

    private void ApplyStatusEffects()
    {
        StartCoroutine(BlinkSprite());
        StartCoroutine(SlowDown());
    }

    private IEnumerator SlowDown()
    {
        float savedSpeed = _playerMove.Speed;
        _playerMove.Speed = 3.0f;
        yield return new WaitForSeconds(1.0f);
        _playerMove.Speed = savedSpeed;
    }

    private IEnumerator BlinkSprite()
    {
        for (int i = 0; i < 3; i++)
        {
            playerSprite.color = Color.red;//new Color(255, 134, 134f);
            yield return new WaitForSeconds(0.1f);
            playerSprite.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }

        playerSprite.color = Color.white;

    }

    public enum PowerUp
    {
        Bread, Hammer, Fan
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

    public void ModifySpeed(float modification, float timer)
    {
        PlayerMovement.ModifySpeed(modification, timer);
    }
}