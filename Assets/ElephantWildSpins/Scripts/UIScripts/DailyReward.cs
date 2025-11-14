using UnityEngine;
using UnityEngine.UI;
using System;

public class DailyReward : MonoBehaviour
{
    public Button[] dayButtons; // Assign buttons for each day in the inspector
    public Text[] rewardTimers; // Text fields to show the timer for the next reward
    public int[] coinRewards; // Assign coin rewards for each day in the inspector
    public int totalDays = 7;
    public float rewardIntervalHours = 24f; // Time in hours between rewards
    public Text txtCoinBalance;

    private DateTime lastClaimedTime;
    private int currentDayIndex;
    private bool isTimerRunning = false;
    private float timeRemaining;


    void OnEnable()
    {
        if (CurrencyManager.Instance != null)
        {
            int coinBalance = CurrencyManager.Instance.GetCoinBalance();
            txtCoinBalance.text = string.Format("{0:#,#.}", coinBalance);
            Debug.Log("Coins Updat Method Called onEnable");
        }
    }

    void Start()
    {
        LoadRewardData();
        CheckWeeklyReset(); // Check if a weekly reset is needed
        UpdateRewardButtons();
    }

    void Update()
    {
        if (isTimerRunning)
        {
            UpdateRewardTimers();
        }
    }

    void UpdateRewardButtons()
    {
        currentDayIndex = GetDayIndex();

        for (int i = 0; i < totalDays; i++)
        {
            if (i == currentDayIndex && !IsDayClaimed(i))
            {
                // Set current day button to be clickable
                EnableButton(dayButtons[i]);
            }
            else
            {
                // Disable future day buttons
                DisableButton(dayButtons[i]);
            }
        }

        UpdateRewardTimers();
    }

    public void ClaimReward(int dayIndex)
    {
        if (dayIndex == currentDayIndex && !IsDayClaimed(dayIndex))
        {
            // Grant the player their reward
            GrantCoins(coinRewards[dayIndex]);

            // Mark the day as claimed
            SaveClaimedDay(dayIndex);

            // Update last claimed time
            lastClaimedTime = DateTime.Now;
            SaveRewardData();

            // Start the timer for the next reward
            isTimerRunning = true;
            SaveTimerData();

            // Update buttons
            UpdateRewardButtons();

            // Disable the button
            DisableButton(dayButtons[dayIndex]);
        }
    }

    void GrantCoins(int amount)
    {
        // Add coins to the player's total
        CurrencyManager.Instance.AddCoinBalance(amount);
        PlayerPrefs.SetInt("PlayerCoins", PlayerPrefs.GetInt("PlayerCoins", 0) + amount);
    }

    void EnableButton(Button button)
    {
        button.interactable = true;
        var colors = button.colors;
        colors.normalColor = new Color(1f, 1f, 1f, 1f); // Full opacity
        button.colors = colors;
    }

    void DisableButton(Button button)
    {
        button.interactable = false;
        var colors = button.colors;
        colors.normalColor = new Color(1f, 1f, 1f, 0.3f); // 30% opacity
        button.colors = colors;
    }

    void UpdateRewardTimers()
    {
        if (!isTimerRunning) return;

        TimeSpan timeSinceLastClaim = DateTime.Now - lastClaimedTime;

        if (timeSinceLastClaim.TotalHours >= rewardIntervalHours)
        {
            // The next day's reward is ready to be claimed
            isTimerRunning = false;
            PlayerPrefs.SetInt("IsTimerRunning", 0);
            UpdateRewardButtons();
        }
        else
        {
            int nextDayIndex = currentDayIndex + 1;

            if (nextDayIndex < totalDays)
            {
                currentDayIndex = GetDayIndex();

                // Update visibility of reward timers
                for (int i = 0; i < totalDays; i++)
                {
                    if (i == nextDayIndex)
                    {
                        // Show the timer for the next day
                        rewardTimers[i].gameObject.SetActive(true);
                        timeRemaining = rewardIntervalHours - (float)timeSinceLastClaim.TotalHours;
                        rewardTimers[i].text = GetFormattedTime(timeRemaining);
                    }
                    else
                    {
                        // Hide other timers
                        rewardTimers[i].gameObject.SetActive(false);
                    }
                }

                SaveTimerData(); // Save the remaining time to PlayerPrefs
            }
        }
    }

    string GetFormattedTime(float hoursRemaining)
    {
        TimeSpan time = TimeSpan.FromHours(hoursRemaining);
        return string.Format("{0:D2}:{1:D2}:{2:D2}", time.Hours, time.Minutes, time.Seconds);
    }

    int GetDayIndex()
    {
        // Calculate the index for the current day in the week, ensuring it starts from 0
        TimeSpan timeSinceStartOfWeek = DateTime.Now - StartOfWeek();
        int dayIndex = (int)Math.Floor(timeSinceStartOfWeek.TotalDays);

        // Ensure the day index is within the totalDays range
        return dayIndex % totalDays;
    }

    DateTime StartOfWeek()
    {
        DateTime today = DateTime.Now;
        int diff = today.DayOfWeek - DayOfWeek.Monday;
        if (diff < 0)
        {
            diff += 7;
        }

        return today.AddDays(-1 * diff).Date;
    }

    bool IsDayClaimed(int dayIndex)
    {
        return PlayerPrefs.GetInt("DayClaimed_" + dayIndex, 0) == 1;
    }

    void SaveClaimedDay(int dayIndex)
    {
        PlayerPrefs.SetInt("DayClaimed_" + dayIndex, 1);
        PlayerPrefs.Save();
    }

    void LoadRewardData()
    {
        // Check if the last claimed time is stored
        if (PlayerPrefs.HasKey("LastClaimedTime"))
        {
            lastClaimedTime = DateTime.Parse(PlayerPrefs.GetString("LastClaimedTime"));
        }
        else
        {
            lastClaimedTime = DateTime.Now.AddHours(-rewardIntervalHours); // Set to allow claiming first day immediately
        }

        // Load timer state
        isTimerRunning = PlayerPrefs.GetInt("IsTimerRunning", 0) == 1;
        if (isTimerRunning)
        {
            timeRemaining = PlayerPrefs.GetFloat("TimeRemaining", rewardIntervalHours);
        }
    }

    void SaveRewardData()
    {
        PlayerPrefs.SetString("LastClaimedTime", lastClaimedTime.ToString());
        PlayerPrefs.SetInt("CurrentDayIndex", currentDayIndex);
        PlayerPrefs.Save();
    }

    void SaveTimerData()
    {
        PlayerPrefs.SetInt("IsTimerRunning", isTimerRunning ? 1 : 0);
        PlayerPrefs.SetFloat("TimeRemaining", timeRemaining);
        PlayerPrefs.Save();
    }

    public void ResetWeeklyRewards()
    {
        // Clear claimed days
        for (int i = 0; i < totalDays; i++)
        {
            PlayerPrefs.SetInt("DayClaimed_" + i, 0);
        }
        PlayerPrefs.Save();
        UpdateRewardButtons();
    }

    void CheckWeeklyReset()
    {
        DateTime lastResetDate = PlayerPrefs.HasKey("LastResetDate")
            ? DateTime.Parse(PlayerPrefs.GetString("LastResetDate"))
            : DateTime.Now.AddDays(-1); // Default to yesterday if no reset date is stored

        DateTime currentDate = DateTime.Now;

        if (currentDate.Date > lastResetDate.Date)
        {
            // Check if it's a new week
            if (currentDate.DayOfWeek == DayOfWeek.Monday)
            {
                ResetWeeklyRewards();
                PlayerPrefs.SetString("LastResetDate", currentDate.ToString());
            }
        }
    }

    void OnApplicationQuit()
    {
        SaveRewardData();
        SaveTimerData();
    }
}
