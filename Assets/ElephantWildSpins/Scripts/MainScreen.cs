using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainScreen : MonoBehaviour 
{
    /// <summary>
    /// Raises the play button pressed event.
    /// </summary>
    public TextMeshProUGUI txtCoinBalance;

    public GameObject agreementPanel;
    public GameObject tcTick;
    public GameObject ppTick;
    public Button acceptBtn;

    public GameObject timerImg;
    public GameObject claimBtn;
    public TextMeshProUGUI questTimer;
    private List<int> rewards = new List<int>() { 1000, 2000, 3000, 4000, 5000 };

    // Time interval to update coins
    public float updateInterval = 2.0f;
    
    void OnEnable()
    {
        SetCoins();
    }

    public void SetCoins()
    {
        //if (CurrencyManager.Instance != null)
        //{
        //    int coinBalance = CurrencyManager.Instance.GetCoinBalance();
        //}
        txtCoinBalance.text = StaticHelper.coins.ToString();
    }

    void Start()
    {
        SetCoins();

        //if (!PlayerPrefs.HasKey("agreement"))
        //{
        //    PlayerPrefs.SetInt("agreement", 1);
        //    PlayerPrefs.Save();
        //    agreementPanel.SetActive(true);
        //}
        //InvokeRepeating(nameof(CheckQuest), 0f, 1f);
    }

    private void StopTimer()
    {
        CancelInvoke(nameof(CheckQuest));
    }

    private void CheckQuest()
    {
        System.DateTime lastDT = new System.DateTime();
        if (!PlayerPrefs.HasKey("lastSpin"))
        {
            //PlayerPrefs.SetString("lastSpin", DateTime.Now.AddHours(24).ToString());
            //PlayerPrefs.Save();

            timerImg.SetActive(false);
            claimBtn.SetActive(true);
            questTimer.text = "Claim";
            StopTimer();
            return;
        }
        lastDT = System.DateTime.Parse(PlayerPrefs.GetString("lastSpin"));

        System.TimeSpan diff = (lastDT - System.DateTime.Now);
        questTimer.text = string.Format("{0:D2}:{1:D2}:{2:D2}", diff.Hours, diff.Minutes, diff.Seconds);

        if (diff.TotalSeconds <= 0)
        {
            StopTimer();
            timerImg.SetActive(false);
            claimBtn.SetActive(true);
            questTimer.text = "Claim";
        }
    }

    private void StartTimer()
    {
        PlayerPrefs.SetString("lastSpin", System.DateTime.Now.AddHours(24).ToString());
        PlayerPrefs.Save();
        timerImg.SetActive(true);
        claimBtn.SetActive(false);
            
        InvokeRepeating(nameof(CheckQuest), 0f, 1f);
    }

    public void ClaimBtn()
    {
        int day = PlayerPrefs.GetInt("DailyReward", 0);
        if (day < 7)
        {
            day++;
            PlayerPrefs.SetInt("DailyReward", day);
            int reward = (day >= 5 ? rewards[Random.Range(0, rewards.Count)] : (day * 1000));
            StaticHelper.coins += (day * 1000);
            Invoke(nameof(StartTimer), 0f);
            SetCoins();
        }
    }

    public void TickAgreement(bool isTc)
    {
        if (isTc)
        {
            tcTick.SetActive(!tcTick.activeSelf);
        }
        else
        {
            ppTick.SetActive(!ppTick.activeSelf);
        }
        acceptBtn.interactable = (tcTick.activeSelf && ppTick.activeSelf);
        Click();
    }

    public void Click()
    {
        AudioManager.Instance.PlayButtonClickSound();
    }

    public void OnPlayButtonPressed()
	{
		if (InputManager.Instance.canInput ()) {
			AudioManager.Instance.PlayButtonClickSound ();
			StackManager.Instance.selectModeScreen.Activate();
		}
	}
}
