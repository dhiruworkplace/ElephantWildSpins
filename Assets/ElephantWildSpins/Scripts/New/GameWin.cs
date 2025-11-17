using System.Collections;
using TMPro;
using UnityEngine;

public class GameWin : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreText.text = ScoreManager.Instance.Score.ToString();

        if ((StaticHelper.selectedLevel + 1) > StaticHelper.saveLevel && StaticHelper.selectedLevel <= 15)
            StaticHelper.saveLevel = (StaticHelper.selectedLevel + 1);
    }

    public void OnHomeButtonPressed()
    {
        if (InputManager.Instance.canInput())
        {
            AudioManager.Instance.PlayButtonClickSound();
            StackManager.Instance.mainMenu.Activate();
            gameObject.Deactivate();
        }
    }

    public void OnResetButtonPressed()
    {
        if (InputManager.Instance.canInput())
        {
            AudioManager.Instance.PlayButtonClickSound();
            StartCoroutine(ResetGame());
        }
    }

    /// <summary>
    /// Raises the close button pressed event.
    /// </summary>
    public void OnCloseButtonPressed()
    {
        if (InputManager.Instance.canInput())
        {
            AudioManager.Instance.PlayButtonClickSound();
            gameObject.Deactivate();
        }
    }

    IEnumerator ResetGame()
    {
        StackManager.Instance.DeactivateGamePlay();
        yield return new WaitForSeconds(0.1F);
        StackManager.Instance.ActivateGamePlay();
        gameObject.Deactivate();
    }

    public void NextLevel()
    {
        if (StaticHelper.selectedLevel < 15)
        {
            StaticHelper.selectedLevel++;
            OnResetButtonPressed();
        }
        else
        {
            OnHomeButtonPressed();
        }
    }
}