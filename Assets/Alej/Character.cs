using System;
using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    private static Character _sInstance;

    public static Character Get()
    {
        return _sInstance;
    }

    public bool KILLME = false;
    [SerializeField] private int _startingHealth = 3;
    [SerializeField] private bool _isInvincible = false;
    [SerializeField] private Sprite[] _possibleItems;
    [SerializeField] private SpriteRenderer itemSlot;
    [SerializeField] private SpriteRenderer playerSprite;

    private int _health = 0;
    private bool _isDead = false;
    public bool IsDead { get => _isDead; set => _isDead = value; }

    [SerializeField] private bool _hasHammer = false;
    public bool HasHammer => _hasHammer;
    public event Action<int> OnHealthChange; // Send current health
    public event Action OnDie;

    public int Health
    {
        get => _health;
        set => _health = value;
    }
    private void Awake()
    {
        if (!_sInstance)
        {
            _sInstance = this;
        }
        else
        {
            DestroyImmediate(this);
        }
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
	
    private PlayerMovement _playerMove;
    public PlayerMovement PlayerMovement => _playerMove;

    public void Die()
	{
		//game manager end game
		_health = 0;
		IsDead = true;
		OnDie?.Invoke();
        CharacterSFXManager.Get().PlayDieSFX();
	}

    public void ModifyHealth(int damage)
    {
        if (_isInvincible || IsDead )
        {
            return;
        }

        if (damage < 0)
        {
	        if (HasHammer)
	        {
		        _hasHammer = false;
		        return;
	        }
            else
            {
                CharacterSFXManager.Get().PlayHurtSFX();
                ApplyDamageStatusEffects();
            }
        }

        _health += damage;
		_health = Math.Clamp(_health, 0, _startingHealth);
		
        OnHealthChange?.Invoke(_health);
		
        if (_health <= 0)
        {
            Die();
        }
    }

    private void ApplyDamageStatusEffects()
    {
        StartCoroutine(BlinkSprite());
        ModifySpeed(0.33f, 1.125f);
    }

    [SerializeField] Color blinkColor;
    private IEnumerator BlinkSprite()
	{
		var originalColor = playerSprite.color;
        for (int i = 0; i < 2; i++)
        {
            playerSprite.color = blinkColor;
            yield return new WaitForSeconds(0.1f);
            playerSprite.color = originalColor;
            yield return new WaitForSeconds(0.1f);
        }
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
                _hasHammer = true;
                break;
            case PowerUp.Fan:
                break;
        }
    }

    public void ModifySpeed(float ratio, float timer)
    {
        PlayerMovement.ModifySpeed(ratio, timer);
    }
}