using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class MainMenu : MonoBehaviour
{
    private bool settingsOn;
    [SerializeField] GameObject panel;
    [SerializeField] private TMP_Text playingAs;

    void Start()
    {
	    settingsOn = false;
	    panel.SetActive(false);

        if (GlobalData.loadedPlayer != null)
        {
            playingAs.text = "Playing As: " + GlobalData.loadedPlayer.playerName;
        }
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

    public void GoToPlayerScreen() 
    {
        SceneManager.LoadScene("PlayerSelection");
    }

    public void PlayGame() 
    {
        if (GlobalData.loadedPlayer == null)
            return;
        SceneManager.LoadScene("Loading");
    }

    public void ExitGame()
    {
    	Application.Quit();
    }
}
