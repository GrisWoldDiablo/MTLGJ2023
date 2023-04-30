using TMPro;
using UnityEngine;

public class UIGameover : MonoBehaviour
{
	[SerializeField] private TMP_Text _scoreText;
	[SerializeField] private TMP_Text _titleText;

	public void UpdateScore(string textToDisplay)
	{
		_titleText.text = $"{textToDisplay} of August 24, 79ce";
		_scoreText.text = $"Distance Ran : {GameManager.Get().RunningDistance:F1}m";
	}
}