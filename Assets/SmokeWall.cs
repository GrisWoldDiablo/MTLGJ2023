using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SmokeWall : MonoBehaviour
{
	[SerializeField] private Character player;

	[SerializeField] ParticleSystem[] _particleSystems;

	int[] numWallSections = new int[3];

	[SerializeField] float speed = 5f;

	// Start is called before the first frame update
	void Awake()
	{
		player.OnDie += OnPlayerDie;
	}

	private void OnPlayerDie()
	{
		foreach (var system in _particleSystems)
		{
			var module = system.main;
			module.startLifetime = 50.0f;
		}
	}


	// Update is called once per frame
	void Update()
	{
		if (player != null && player.IsDead)
		{

			// Get the main module of the particle system
			return;
		}
		//move wall towards the player at a constant rate
		transform.Translate(Vector3.right * speed * Time.deltaTime);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		Character player = other.GetComponent<Character>();

		if (player != null)
		{
			player.ModifyHealth(-10);

		}
	}
}