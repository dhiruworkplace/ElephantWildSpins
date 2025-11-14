using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Splash : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(nameof(GotoGame));
    }
    IEnumerator GotoGame()
    {
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene(1);
    }
}

