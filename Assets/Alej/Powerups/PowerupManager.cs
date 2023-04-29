using UnityEngine;

public class PowerupManager : MonoBehaviour
{
	[SerializeField] private Powerup[] powerups;

	public PowerupManager Instance { get; private set; }

	private void Awake()
	{
		if (!Instance)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}

	}

	private void Start()
	{
		SpawnPowerup();
	}

	public void SpawnPowerup()
	{
		Instantiate(powerups[0]);
		Instantiate(powerups[1]);
	}

}
