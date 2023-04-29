using UnityEngine;

public class GameManager : MonoBehaviour
{
	private static GameManager _sInstance;

	public static GameManager Get()
	{
		return _sInstance;
	}

	public bool IsGamePaused => UIManager.Get().IsPause;

	[SerializeField] private Character _player;

	public Character Player => _player;

	public bool HasStarted { get; private set; } = false;

	private void Awake()
	{
		Time.timeScale = 0.0f;
		if (!_sInstance)
		{
			_sInstance = this;
			DontDestroyOnLoad(this);
		}
		else
		{
			DestroyImmediate(this);
		}
	}

	public void StartGame()
	{
		// TODO logic start game.
		Debug.Log("Start Game");
		HasStarted = true;
	}
}