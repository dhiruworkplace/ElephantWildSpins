using UnityEngine;

public class SettingMansion : MonoBehaviour
{
    [Space(5)]
    public GameObject soundOn;
    public GameObject musicOn;

    private void Start()
    {
        soundOn.SetActive(StaticHelper.sound.Equals(1));
        musicOn.SetActive(StaticHelper.music.Equals(1));
    }

    public void SetMusic()
    {
        if (StaticHelper.music.Equals(1))
        {
            StaticHelper.music = 0;
            AudioManager.Instance.isMusicEnabled = false;
            BackgroundMusic.Instance.PauseBGMusic();
        }
        else
        {
            StaticHelper.music = 1;
            AudioManager.Instance.isMusicEnabled = true;
            BackgroundMusic.Instance.StartBGMusic();
        }
        musicOn.SetActive(StaticHelper.music.Equals(1));
        AudioManager.Instance.PlayButtonClickSound();
    }

    public void SetSound()
    {
        if (StaticHelper.sound.Equals(1))
        {
            StaticHelper.sound = 0;
            AudioManager.Instance.isSoundEnabled = false;
        }
        else
        {
            StaticHelper.sound = 1;
            AudioManager.Instance.isSoundEnabled = true;
        }
        soundOn.SetActive(StaticHelper.sound.Equals(1));
        AudioManager.Instance.PlayButtonClickSound();
    }
}