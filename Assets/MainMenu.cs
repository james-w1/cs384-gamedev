using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

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

    [SerializeField] AudioMixer mixer;
    public void SetLevel (float sliderValue)
    {
        mixer.SetFloat("MainVol", Mathf.Log10(sliderValue) * 20);
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
