using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class FortuneWheel : MonoBehaviour
{
    public Transform wheel;

    public List<float> prize;
    public AnimationCurve animationCurves;

    private float anglePerItem;
    public int itemNumber;
    public int spinTime;

    [Space(10)]
    public GameObject spinBtn;
    public GameObject ClaimBtn;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI rewardText;

    private int coin = 0;

    private void OnEnable()
    {
        anglePerItem = 360 / prize.Count;
        InvokeRepeating(nameof(CheckQuest), 0f, 1f);
    }

    private void OnDisable()
    {
        StopTimer();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Spin();
        }
    }

    public void Spin()
    {
        coin = 0;
        TapSound();
        spinBtn.SetActive(false);
        itemNumber = Random.Range(0, prize.Count);
        float maxAngle = 360 * spinTime + (prize[itemNumber] * anglePerItem);

        StartCoroutine(SpinTheWheel(0.3f * spinTime, maxAngle));
    }

    IEnumerator SpinTheWheel(float time, float maxAngle)
    {
        //SoundManager.Instance.PlaySound(25);

        float timer = 0.0f;
        float startAngle = transform.eulerAngles.z;
        maxAngle = maxAngle - startAngle;

        while (timer < time)
        {
            //to calculate rotation
            float angle = maxAngle * animationCurves.Evaluate(timer / time);
            wheel.eulerAngles = new Vector3(0.0f, 0.0f, -(angle + startAngle));
            timer += Time.deltaTime;
            yield return 0;
        }

        wheel.eulerAngles = new Vector3(0.0f, 0.0f, -(maxAngle + startAngle));

        GetResult(itemNumber);
    }

    private void GetResult(int index)
    {
        int coins = 0;
        if (index.Equals(0))
        {
            Debug.Log("500");
            coins = 500;
        }
        else if (index.Equals(1))
        {
            Debug.Log("1000");
            coins = 1000;
        }
        else if (index.Equals(2))
        {
            Debug.Log("2000");
            coins = 2000;
        }
        else if (index.Equals(3))
        {
            Debug.Log("3000");
            coins = 3000;
        }
        else if (index.Equals(4))
        {
            Debug.Log("0");
            coins = 0;
        }
        else if (index.Equals(5))
        {
            Debug.Log("4000");
            coins = 4000;
        }
        else if (index.Equals(6))
        {
            Debug.Log("0");
            coins = 0;
        }
        else if (index.Equals(7))
        {
            Debug.Log("5000");
            coins = 5000;
        }
        coin = coins;
        //rewardText.text = coin.ToString();
        if (coin > 0)
            ClaimBtn.SetActive(true);
        else
            NoClaim();
    }

    private void CheckQuest()
    {
        System.DateTime lastDT = new System.DateTime();
        if (!PlayerPrefs.HasKey("lastSpin"))
        {
            spinBtn.SetActive(true);
            timerText.text = "Ready to Spin!";
            timerText.transform.parent.gameObject.SetActive(false);
            StopTimer();
            return;
        }
        lastDT = System.DateTime.Parse(PlayerPrefs.GetString("lastSpin"));

        System.TimeSpan diff = (lastDT - System.DateTime.Now);
        timerText.text = string.Format("{0:D2}:{1:D2}:{2:D2}", diff.Hours, diff.Minutes, diff.Seconds);

        if (diff.TotalSeconds <= 0)
        {
            StopTimer();
            spinBtn.SetActive(true);
            timerText.transform.parent.gameObject.SetActive(false);
            timerText.text = "Ready to Spin!";
        }
    }

    private void StartTimer()
    {
        PlayerPrefs.SetString("lastSpin", System.DateTime.Now.AddHours(24).ToString());
        PlayerPrefs.Save();

        timerText.transform.parent.gameObject.SetActive(true);
        InvokeRepeating(nameof(CheckQuest), 0f, 1f);
    }

    private void StopTimer()
    {
        CancelInvoke(nameof(CheckQuest));
    }

    public void ClaimButton()
    {
        TapSound();
        StaticHelper.coins += coin;
        ClaimBtn.SetActive(false);
        Invoke(nameof(StartTimer), 0f);
        FindFirstObjectByType<MainScreen>().SetCoins();
    }

    private void NoClaim()
    {
        Invoke(nameof(StartTimer), 0f);
    }

    private void TapSound()
    {
        //MSSetting.instance.PlaySound(0);
    }
}