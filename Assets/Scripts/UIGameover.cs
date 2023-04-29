using TMPro;
using UnityEngine;

public class UIGameover : MonoBehaviour
{
	[SerializeField] private TMP_Text _scoreText;

	public void UpdateScore()
	{
		_scoreText.text = $"Distance Ran : {GameManager.Get().RunningDistance:F1}m";
	}
}