using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISwapBiome : MonoBehaviour
{
	[SerializeField] private float _fadeSpeed = 1.0f;
	[SerializeField] private Image _fadePanelImage;
	[SerializeField] private TMP_Text _textToDisplay;
	private float _lerpvalue = 0.0f;

	public void Go(string textToDisplay)
	{
		_textToDisplay.text = $"{textToDisplay}...";
		gameObject.SetActive(true);
	}
	
	private void OnEnable()
	{
		_fadePanelImage.color = Color.black;
		_lerpvalue = 0.0f;
	}

	private void Update()
	{
		_lerpvalue += Time.deltaTime / _fadeSpeed;
		_fadePanelImage.color = Color.Lerp(Color.black, Color.clear, _lerpvalue);
		if (_lerpvalue >= 1.5f)
		{
			gameObject.SetActive(false);
		}
	}
}
