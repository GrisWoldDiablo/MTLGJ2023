using System.Collections;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
	[SerializeField] private Character _character;
	[SerializeField] private PlayerMovement _playerMovement;
	[SerializeField] private SpriteRenderer _spriteRenderer;
	[Min(1.0f)] [SerializeField] private float _animationSpeed = 10.0f;
	[SerializeField] private Sprite[] _runSprites;
	[SerializeField] private Sprite[] _runSpritesHammer;
	[SerializeField] private Sprite[] _jumpSprites;
	[SerializeField] private Sprite[] _jumpSpritesHammer;
	[SerializeField] private Sprite[] _slideSprites;
	[SerializeField] private Sprite[] _walkBackSprites;
	[SerializeField] private Sprite[] _walkBackSpritesHammer;
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
			_currentSprites = _character.HasHammer ? _jumpSpritesHammer : _jumpSprites;
			break;
		case PlayerState.Run:
			if (_playerMovement.IsMovingForward)
			{
				_currentSprites = _character.HasHammer ? _runSpritesHammer : _runSprites;
			}
			else
			{
				_currentSprites = _character.HasHammer ? _walkBackSpritesHammer : _walkBackSprites;
			}
			break;
		case PlayerState.Slide:
			_currentSprites = _playerMovement.IsMovingForward ? _slideSprites : _walkBackSprites;
			break;
		}

		_animationSpeed = _playerMovement.IsRunningFast ? 20f : 10f;
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

		GameManager.Get().CanReceiveInput = false;

		currentIndex = 0;
		_currentSprites = _dieSprites;
		while (currentIndex < _currentSprites.Length)
		{
			yield return new WaitForSeconds(1.0f / _animationSpeed);
			_spriteRenderer.sprite = _currentSprites[currentIndex];
			currentIndex++;
		}

		_playerMovement.CanMove = false;
	}
}