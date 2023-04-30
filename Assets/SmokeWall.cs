using UnityEngine;

public class SmokeWall : MonoBehaviour
{
	[SerializeField] private Character player;
    [SerializeField] private GameObject playerBody;

    [SerializeField] private float maxDistanceToPlayer = 20.0f;

    [SerializeField] ParticleSystem[] _particleSystems;

	int[] numWallSections = new int[3];

	private float speed = 5f;

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

		//Debug.Log(gameObject.transform.position.x + playerBody.gameObject.transform.position.x);

		if (gameObject.transform.position.x + maxDistanceToPlayer < playerBody.gameObject.transform.position.x)
		{
			transform.position = new Vector3(playerBody.transform.position.x - maxDistanceToPlayer, transform.position.y, transform.position.z);
		}

		transform.Translate(Vector3.right * Speed * Time.deltaTime);
		
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		Character player = other.GetComponentInParent<Character>();

		if (player != null)
		{
			player.Die();

		}
	}
}