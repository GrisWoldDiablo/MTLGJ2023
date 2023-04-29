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

	public float RunningDistance { get; private set; } = 0.0f;

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
		
		// Start level music loop
		MusicManager.Get().TransitionToLevelMusic();
	}

	public void IncrementRunningDistance(float value)
	{
		RunningDistance += value;
	}
}