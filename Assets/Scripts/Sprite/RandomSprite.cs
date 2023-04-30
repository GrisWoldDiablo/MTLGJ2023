using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(SpriteRenderer))]
public class RandomSprite : MonoBehaviour
{
	[SerializeField] private SpritesSheetScriptable _spritesSheetScriptable;
	[SerializeField] bool _shouldAnimate;
	[SerializeField] float _speed = 0.25f;
	private float _lerpValue = 0.0f;
	private SpriteRenderer _spriteRenderer;

	private void Awake()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_spriteRenderer.sprite = _spritesSheetScriptable.GetRandomSprite();
		_speed = Random.Range(_speed * 0.5f, _speed * 1.5f);
	}

	private void Update()
	{
		if (!_shouldAnimate)
		{
			enabled = false;
			return;
		}

		_lerpValue += Time.unscaledDeltaTime/ _speed;
		_spriteRenderer.color = Color.Lerp(Color.white, Color.black, _lerpValue);
		if (_lerpValue >= 1.0f)
		{
			_lerpValue = 0.0f;
			_speed = Random.Range(_speed * 0.5f, _speed * 1.5f);
			_speed = Mathf.Clamp(_speed, 1.0f, 10.0f);
		}
	}
}