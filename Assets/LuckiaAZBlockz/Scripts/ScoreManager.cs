using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : Singleton<ScoreManager> 
{
	[SerializeField] private TextMeshProUGUI txtScore;
	[SerializeField] private TextMeshProUGUI txtBestScore;
	[SerializeField] private GameObject scoreAnimator;
	[SerializeField] private Text txtAnimatedText;
	
	[HideInInspector] public int Score = 0;
	int bestScore = 0;

	void Start()
	{
		txtScore.text = Score.ToString ();	
		int bestScore = PlayerPrefs.GetInt ("BestScore_" + GameController.gameMode.ToString (), Score);
		txtBestScore.text = "Best Score <color=yellow>" + bestScore + "</color>";
	}

	public void AddScore(int scoreToAdd, bool doAnimate = true)
	{
		int oldScore = Score;
		Score += scoreToAdd;

		StartCoroutine (SetScore(oldScore, Score));

		if (doAnimate) {
			Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			mousePos.z = 0;
			scoreAnimator.transform.position = mousePos;
			txtAnimatedText.text = "+" + scoreToAdd.ToString ();
			scoreAnimator.SetActive (true);
		}
	}

	public int GetScore()
	{
		return Score;
	}

	IEnumerator SetScore(int lastScore, int currentScore)
	{
		int IterationSize = (currentScore - lastScore) / 10;

		for (int index = 1; index < 10; index++) {
			lastScore += IterationSize;
			txtScore.text =  string.Format("{0:#,#.}", lastScore);
			yield return new WaitForEndOfFrame ();
		}
		txtScore.text =  string.Format("{0:#,#.}", currentScore);
		yield return new WaitForSeconds (1F);
		scoreAnimator.SetActive (false);
	}
}
