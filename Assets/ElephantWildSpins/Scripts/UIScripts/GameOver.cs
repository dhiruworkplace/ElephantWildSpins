using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class GameOver : MonoBehaviour {

	[SerializeField] TextMeshProUGUI txtScore;
	[SerializeField] private TextMeshProUGUI txtCoinReward;
	[SerializeField] private Text txtBestScore;

	public void SetLevelScore(int score, int coinReward)
	{
		int bestScore = PlayerPrefs.GetInt ("BestScore_" + GameController.gameMode.ToString (), score);

		if (score >= bestScore) 
		{
			PlayerPrefs.SetInt ("BestScore_" + GameController.gameMode.ToString (), score);
			bestScore = score;
		}

		txtScore.text = string.Format("{0:#,#.}", score.ToString("0")) + "";
		txtBestScore.text = string.Format("{0:#,#.}", bestScore.ToString("0"));
		txtCoinReward.text = string.Format("{0:#,#.}", coinReward.ToString("0"));

		CurrencyManager.Instance.AddCoinBalance (coinReward);
		//StaticHelper.coins += coinReward;
	}

	public void OnHomeButtonPressed()
	{
		if (InputManager.Instance.canInput ()) {
			AudioManager.Instance.PlayButtonClickSound ();
			StackManager.Instance.mainMenu.Activate();
			gameObject.Deactivate();
		}
	}

	public void OnReplayButtonPressed()
	{
		if (InputManager.Instance.canInput ()) {
			AudioManager.Instance.PlayButtonClickSound ();
			StackManager.Instance.ActivateGamePlay();
			gameObject.Deactivate();
		}
	}

    private void OnEnable()
    {
        //You can adjust ad show delay based on your requirement.
        ShowInterstitial();
    }

    void ShowInterstitial()
    {
        if(AdManager.Instance.isAdsAllowed())
        {
            AdManager.Instance.ShowInterstitial();
        }
    }
}
