using UnityEngine;

public static class StaticHelper
{
    public static int selectedTheme = 0;
    public static int selectedLevel = 1;
    public static bool showLevels;
    public static bool infinite = false;

    public static int badge
    {
        get { return PlayerPrefs.GetInt("badge", -1); }
        set { PlayerPrefs.SetInt("badge", value); }
    }

    public static int score
    {
        get { return PlayerPrefs.GetInt("score", 0); }
        set { PlayerPrefs.SetInt("score", value); }
    }

    public static int saveLevel
    {
        get { return PlayerPrefs.GetInt("savelevel", 1); }
        set
        {
            PlayerPrefs.SetInt("savelevel", value);
            PlayerPrefs.Save();
        }
    }

    public static int music
    {
        get { return PlayerPrefs.GetInt("music", 1); }
        set { PlayerPrefs.SetInt("music", value); }
    }

    public static int sound
    {
        get { return PlayerPrefs.GetInt("sound", 1); }
        set { PlayerPrefs.SetInt("sound", value); }
    }

    public static string name
    {
        get { return PlayerPrefs.GetString("name", "You"); }
        set { PlayerPrefs.SetString("name", value); }
    }

    public static int coins
    {
        get { return PlayerPrefs.GetInt("coins", 0); }
        set
        {
            PlayerPrefs.SetInt("coins", value);
        }
    }

    public static int Reward(int level)
    {
        if (level >= 10)
            return 1000;
        else
            return (level * 100);
    }

    public static int LevelTime(int level)
    {
        if (level > 0 && level <= 4)
            return 110;
        else if (level > 4 && level <= 8)
            return 100;
        else if (level > 8 && level <= 12)
            return 90;
        else if (level > 12 && level <= 16)
            return 80;
        else if (level > 16 && level <= 20)
            return 70;
        else if (level > 20 && level <= 24)
            return 60;
        else
            return 60;
    }
}