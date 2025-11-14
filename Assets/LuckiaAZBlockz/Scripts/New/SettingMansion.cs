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
            //SoundMansion.instance.PauseMusic();
        }
        else
        {
            StaticHelper.music = 1;
            //SoundMansion.instance.PlayMusic();
        }
        musicOn.SetActive(StaticHelper.music.Equals(1));
        //SoundMansion.instance.PlaySound(0);
    }

    public void SetSound()
    {
        if (StaticHelper.sound.Equals(1))
        {
            StaticHelper.sound = 0;
        }
        else
        {
            StaticHelper.sound = 1;
        }
        soundOn.SetActive(StaticHelper.sound.Equals(1));
        //SoundMansion.instance.PlaySound(0);
    }
}