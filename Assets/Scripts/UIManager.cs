using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	private static UIManager _sInstance;

	public static UIManager Get()
	{
		return _sInstance;
	}

	[SerializeField] private Button _startButton;
	[SerializeField] private Button _runButton;
	[SerializeField] private CanvasGroup _mainCanvasGroup;
	[SerializeField] private CanvasGroup _pauseCanvasGroup;

	
	public bool IsPause { get; private set; } = true;

	private void Awake()
	{
		if (!_sInstance)
		{
			_sInstance = this;
			DontDestroyOnLoad(this);
		}
		else
		{
			DestroyImmediate(this);
			return;
		}

		_startButton.onClick.AddListener(OnStartButtonClicked);
		_runButton.onClick.AddListener(OnRunButtonClicked);
		_mainCanvasGroup.alpha = 1.0f;
		_pauseCanvasGroup.alpha = 0.0f;
		_pauseCanvasGroup.blocksRaycasts = false;
	}

	private void Update()
	{
		if (!GameManager.Get().HasStarted)
		{
			return;
		}
		
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (!IsPause)
			{
				Pause();
			}
			else
			{
				OnRunButtonClicked();
			}
		}
	}

	private void Pause()
	{
		IsPause = true;
		Time.timeScale = 0.0f;
		_pauseCanvasGroup.alpha = 1.0f;
		_pauseCanvasGroup.blocksRaycasts = true;
	}

	private void OnRunButtonClicked()
	{
		IsPause = false;
		Time.timeScale = 1.0f;
		_pauseCanvasGroup.alpha = 0.0f;
		_pauseCanvasGroup.blocksRaycasts = false;
	}

	private void OnStartButtonClicked()
	{
		_mainCanvasGroup.alpha = 0.0f;
		_mainCanvasGroup.blocksRaycasts = false;
		Time.timeScale = 1.0f;
		GameManager.Get().StartGame();
		IsPause = false;
	}
}