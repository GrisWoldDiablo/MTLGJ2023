using System;
using System.Collections;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
	[SerializeField] private SpriteRenderer _spriteRenderer;
	[Min(1.0f)] [SerializeField] private float _animationSpeed = 100.0f;
	[SerializeField] private Sprite[] _runSprites;

	private Sprite[] _currentSprites;

	private void Start()
	{
		_currentSprites = _runSprites;
		StartCoroutine(Animate());
	}

	void Update() { }

	private IEnumerator Animate()
	{
		int currentIndex = 0;
		while (true)
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
	}
}