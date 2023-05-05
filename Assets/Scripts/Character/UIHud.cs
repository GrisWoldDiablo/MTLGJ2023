using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIHud : MonoBehaviour
{
	[SerializeField] private GameObject _healthLayout;
	[SerializeField] private GameObject _healthObject;
	[SerializeField] private TMP_Text _runningDistance;

	private Stack<GameObject> _usedHealthObjects = new();
	private Stack<GameObject> _unusedHealthObjects = new();
	private Character _character;
	private int _currentHeath = 0;

	private void Awake()
	{
		_character = FindObjectOfType<Character>();
		_character.OnHealthChange += OnHealthChange;
		_character.OnDie += OnDeath;
		_runningDistance.text = "0.0m";
	}

	private void Update()
	{
		_runningDistance.text = $"{GameManager.Get().RunningDistance:F1}m";
	}

	private void OnDeath()
	{
		foreach (var health in _usedHealthObjects)
		{
			health.gameObject.SetActive(false);
		}
	}

	private void OnHealthChange(int value)
	{
		int healthDifference = value - _currentHeath;
		for (int i = 0; i < Mathf.Abs(healthDifference); i++)
		{
			if (healthDifference > 0)
			{
				if (!_unusedHealthObjects.TryPop(out var healthObject))
				{
					healthObject = Instantiate(_healthObject, _healthLayout.transform);
				}

				healthObject.gameObject.SetActive(true);
				_usedHealthObjects.Push(healthObject);
			}
			else
			{
				if (_usedHealthObjects.TryPop(out var healthObject))
				{
					healthObject.gameObject.SetActive(false);
					_unusedHealthObjects.Push(healthObject);
				}
			}
		}

		_currentHeath = value;
	}
}