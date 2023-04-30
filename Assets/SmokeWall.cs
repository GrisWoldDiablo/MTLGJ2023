using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using static UnityEditor.Experimental.GraphView.GraphView;

public class SmokeWall : MonoBehaviour
{
	[SerializeField] private Character player;
	[SerializeField] private float maxDistanceToPlayer = 30.0f;

    [SerializeField] ParticleSystem[] _particleSystems;

	int[] numWallSections = new int[3];

	[SerializeField] float speed = 5f;

    public float Speed { get => speed; set => speed = value; }

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

		float distanceToPlayer = Mathf.Abs(gameObject.transform.position.x - player.gameObject.transform.position.x);
		if (distanceToPlayer >= maxDistanceToPlayer)
		{
			transform.position = new Vector3(player.gameObject.transform.position.x + maxDistanceToPlayer, transform.position.y, transform.position.z);
		}
		else
		{
			//move wall towards the player at a constant rate
			transform.Translate(Vector3.right * Speed * Time.deltaTime);
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		Character player = other.GetComponentInParent<Character>();

		if (player != null)
		{
			player.ModifyHealth(-10);

		}
	}
}