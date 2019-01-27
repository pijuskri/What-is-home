using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject Credits;
    public GameObject Controls;
    public AudioSource Click;
    //private bool k = false;
    //private bool l = false;
    public void onStartClick()
    {
        Click.Play();
        SceneManager.LoadScene("Testin");
    }

    public void onCreditsClick()
    {
        Click.Play();
        MainMenu.SetActive(false);
        Credits.SetActive(true);
    }

    public void onExitClick()
    {
        Click.Play();
        Application.Quit();
        Debug.Log("oof.lt");
    }
    public void onCreditsBackClick()
    {
        Credits.SetActive(false);
        MainMenu.SetActive(true);
        Click.Play();
    }
    public void onControlsBackClick()
    {
        Click.Play();
        Controls.SetActive(false);
        MainMenu.SetActive(true);
    }
    public void onControlsClick()
    {
        Click.Play();
        Controls.SetActive(true);
        MainMenu.SetActive(false);
    }
}
