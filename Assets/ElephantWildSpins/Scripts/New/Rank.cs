using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Rank : MonoBehaviour
{
    public bool me;
    public Image panel;
    public Image rankImg;
    public GameObject crown;
    public TextMeshProUGUI noText;
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI points;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SetData(bool me, int no, string name, int _points)
    {
        noText.text = "#" + no;
        playerName.text = name;
        points.text = _points.ToString();
    }
}