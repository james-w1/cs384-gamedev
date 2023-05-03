using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private bool settingsOn;
    [SerializeField] GameObject panel;

    void Start()
    {
	    settingsOn = false;
	    panel.SetActive(false);
    }

    public void ToggleSettings()
    {
	    settingsOn = !settingsOn;
	    panel.SetActive(settingsOn);
    }

    public void PlayGame() 
    {
        SceneManager.LoadScene("Loading");
    }

    public void ExitGame()
    {
    	Application.Quit();
    }
}
