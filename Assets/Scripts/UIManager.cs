#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
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
	[SerializeField] private Button _revertButton;
	[SerializeField] private Button _quitButton;
	[SerializeField] private CanvasGroup _mainCanvasGroup;
	[SerializeField] private CanvasGroup _pauseCanvasGroup;
	[SerializeField] private CanvasGroup _gameoverGroup;
	[SerializeField] private UIGameover _gameover;
	[SerializeField] private Character _character;
	[SerializeField] private UISwapBiome _uiSwapBiome;
	
	public bool IsPause { get; private set; } = true;

	private void Awake()
	{
		if (!_sInstance)
		{
			_sInstance = this;
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
		
		_gameoverGroup.alpha = 0.0f;
		_gameoverGroup.blocksRaycasts = false;
		_revertButton.onClick.AddListener(OnRevertButtonClicked);
		_character.OnDie += OnCharacterDie;

		_quitButton.onClick.AddListener(OnQuitButtonClicked);
	}

	private void OnQuitButtonClicked()
	{
		Application.Quit();
#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
#endif
	}

	private void OnRevertButtonClicked()
	{
		SceneManager.LoadScene("MainScene");
	}

	private void OnCharacterDie()
	{
		_gameoverGroup.alpha = 1.0f;
		_gameoverGroup.blocksRaycasts = true;
		_gameover.UpdateScore();
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

	public void Pause()
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
		EventSystem.current.SetSelectedGameObject(null);
	}

	private void OnStartButtonClicked()
	{
		EventSystem.current.SetSelectedGameObject(null);
		_mainCanvasGroup.alpha = 0.0f;
		_mainCanvasGroup.blocksRaycasts = false;
		Time.timeScale = 1.0f;
		GameManager.Get().StartGame();
		IsPause = false;
	}

	public void SwapBiome()
	{
		_uiSwapBiome.gameObject.SetActive(true);
	}
}