using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject Credits;
    public GameObject Controls;
    //private bool k = false;
    //private bool l = false;
    public void onStartClick()
    {
        SceneManager.LoadScene("Testin");
    }

    public void onCreditsClick()
    {
        MainMenu.SetActive(false);
        Credits.SetActive(true);
    }

    public void onExitClick()
    {
        Application.Quit();
        Debug.Log("oof.lt");
    }
    public void onCreditsBackClick()
    {
        Credits.SetActive(false);
        MainMenu.SetActive(true);
    }
    public void onControlsBackClick()
    {
        Controls.SetActive(false);
        MainMenu.SetActive(true);
    }
    public void onControlsClick()
    {
        Controls.SetActive(true);
        MainMenu.SetActive(false);
    }
}
