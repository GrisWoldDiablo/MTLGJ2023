using System.Collections.Generic;
using UnityEngine;

public class UIHud : MonoBehaviour
{
	[SerializeField] private GameObject _healthLayout;
	[SerializeField] private GameObject _healthObject;

	private Stack<GameObject> _usedHealthObjects = new();
	private Stack<GameObject> _unusedHealthObjects = new();
	private Character _character;

	private void Awake()
	{
		_character = FindObjectOfType<Character>();
		_character.OnHealthChange += OnHealthChange;
	}

	private void OnHealthChange(int value)
	{
		for (int i = 0; i < Mathf.Abs(value); i++)
		{
			if (value > 0)
			{
				if (_unusedHealthObjects.TryPop(out var healthObject))
				{
					healthObject.gameObject.SetActive(true);
				}
				else
				{
					_usedHealthObjects.Push(Instantiate(_healthObject, _healthLayout.transform));
				}
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

	}
}