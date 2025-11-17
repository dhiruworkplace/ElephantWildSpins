using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    public int levelNo;
    public TextMeshProUGUI levelNoText;
    public GameObject cleared;
    public GameObject currLvl;
    public GameObject lockObj;

    // Start is called before the first frame update
    void Start()
    {
        levelNo = transform.GetSiblingIndex() + 1;
        levelNoText.text = "Level " + levelNo.ToString("00");

        if (StaticHelper.saveLevel.Equals(levelNo))
        {
            cleared.SetActive(false);
            currLvl.SetActive(true);
            lockObj.SetActive(false);
        }
        else if (StaticHelper.saveLevel < levelNo)
        {
            cleared.SetActive(false);
            currLvl.SetActive(false);
            lockObj.SetActive(true);
        }
        else
        {
            cleared.SetActive(true);
            currLvl.SetActive(false);
            lockObj.SetActive(false);
        }
    }

    public void OnClick()
    {
        if (lockObj.activeSelf)
            return;

        //SoundMansion.instance.PlaySound(0);
        StaticHelper.selectedLevel = levelNo;
        FindAnyObjectByType<SelectMode>().OnClassicButtonPressed();
    }
}