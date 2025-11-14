using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class LoadingBar : MonoBehaviour
{
    public Image LoadingBarImg;
    public GameObject LoadingPAnel;
    private void OnEnable()
    {
        LoadingBarImg.transform.localScale = new Vector3(0,1,1);
        LoadingBarImg.transform.DOScaleX(1, 10f).OnComplete(() => HidePanell());
    }
    public void HidePanell()
    {
        LoadingPAnel.SetActive(false);
    }
}
