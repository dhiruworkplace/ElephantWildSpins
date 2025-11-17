using System.Collections.Generic;
using UnityEngine;

public class ShopMansion : MonoBehaviour
{
    public GameObject[] Themes;
    private List<int> prices = new List<int>() { 1000, 2000, 3000, 4000, 5000, 6000, 7000 };
    public GameObject noCoinPanel;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("p0", 1);
        PlayerPrefs.Save();

        CheckAllTheme();
    }

    private void CheckAllTheme()
    {
        for (int i = 0; i < Themes.Length; i++)
        {
            GameObject m = Themes[i];
            if (PlayerPrefs.GetInt("p" + i, 0) == 1)
            {
                m.transform.GetChild(1).gameObject.SetActive(false);
                m.transform.GetChild(2).gameObject.SetActive(true);
                m.transform.GetChild(3).gameObject.SetActive(false);

                if (StaticHelper.selectedTheme.Equals(i))
                {
                    m.transform.GetChild(1).gameObject.SetActive(true);
                    m.transform.GetChild(2).gameObject.SetActive(false);
                }
            }
        }
    }

    public void SelectTheme(int index)
    {
        if (PlayerPrefs.GetInt("p" + index, 0) == 1)
        {
            //if (!Container.selectedBg.Equals(index))
            {
                StaticHelper.selectedTheme = index;
                CheckAllTheme();
            }
        }
        else
        {
            if (StaticHelper.coins >= prices[index])
            {
                StaticHelper.coins -= prices[index];
                Themes[index].transform.GetChild(1).gameObject.SetActive(false);
                Themes[index].transform.GetChild(2).gameObject.SetActive(true);
                Themes[index].transform.GetChild(3).gameObject.SetActive(false);
                PlayerPrefs.SetInt("p" + index, 1);
                PlayerPrefs.Save();

                FindAnyObjectByType<MainScreen>().SetCoins();
            }
            else
                noCoinPanel.SetActive(true);
        }
        AudioManager.Instance.PlayButtonClickSound();
    }
}