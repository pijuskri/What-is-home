using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void onStartClick()
    {
        SceneManager.LoadScene("Testin");
    }

    public void onCreditsClick()
    {

    }

    public void onExitClick()
    {
        Application.Quit();
        Debug.Log("oof");
    }
}
