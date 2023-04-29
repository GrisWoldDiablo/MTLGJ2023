using System;
using System.Collections;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    [SerializeField] private Character _character;
    [SerializeField] private PlayerMovement _playerMovement;
	[SerializeField] private SpriteRenderer _spriteRenderer;
	[Min(1.0f)] [SerializeField] private float _animationSpeed = 100.0f;
	[SerializeField] private Sprite[] _runSprites;
	[SerializeField] private Sprite[] _jumpSprites;
	[SerializeField] private Sprite[] _slideSprites;
	[SerializeField] private Sprite[] _walkBackSprites;
	[SerializeField] private Sprite[] _dieSprites;

	private Sprite[] _currentSprites;

	private void Start()
	{
		_currentSprites = _runSprites;
		StartCoroutine(Animate());
	}

	void Update()
	{
		if (_character.IsDead) // TODO Move dead logic to Player Class
		{
			return;
		}

		switch (_playerMovement.GetPlayerState())
		{
		case PlayerState.Jump:
			_currentSprites = _jumpSprites;
			break;
		case PlayerState.Run:
			_currentSprites = _playerMovement.IsMovingForward ? _runSprites : _walkBackSprites;
			break;
		case PlayerState.Slide:
			_currentSprites = _playerMovement.IsMovingForward ? _slideSprites : _walkBackSprites;
			break;
		}
	}

	private IEnumerator Animate()
	{
		int currentIndex = 0;
		while (!_character.IsDead)
		{
			yield return new WaitForSeconds(1.0f / _animationSpeed);
			if (_currentSprites.Length == 0)
			{
				continue;
			}

			if (currentIndex >= _currentSprites.Length)
			{
				currentIndex = 0;
			}

			_spriteRenderer.sprite = _currentSprites[currentIndex];
			currentIndex++;
		}

		currentIndex = 0;
		_currentSprites = _dieSprites;
		while (currentIndex < _currentSprites.Length)
		{
			yield return new WaitForSeconds(1.0f / _animationSpeed);
			_spriteRenderer.sprite = _currentSprites[currentIndex];
			currentIndex++;
		}

		_playerMovement.IsDead = true;
	}
}