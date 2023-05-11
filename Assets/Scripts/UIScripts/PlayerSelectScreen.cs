using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class PlayerSelectScreen : MonoBehaviour
{
    List<PlayerSave> players;
    List<string> playerNames;

    //[SerializeField] private Button addButton;
    [SerializeField] private TMP_Text textBox;
    [SerializeField] private TMP_Dropdown dropDown;

    void Start()
    {
        // load all the player data and populate thingy
        players = new List<PlayerSave>();
        playerNames = new List<string>();
        reloadPlayers();
    }

    private void reloadPlayers()
    {
        dropDown.ClearOptions();
        players = FileHandlingScript.LoadAllPlayers();

        if (players.Count > 0) {
            foreach (PlayerSave playerSave in players) {
                playerNames.Add(playerSave.playerName);
            }

            dropDown.AddOptions(playerNames);
        }
    }

    public void addPlayer()
    {
        Debug.Log(FileHandlingScript.SavePlayerData(textBox.text, null));
        reloadPlayers();
    }

    public void BackButton() 
    {
        SceneManager.LoadScene("MainMenu");
    }
}
