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
    PlayerSave sPlayer;
    List<string> playerNames;

    //[SerializeField] private Button addButton;
    [SerializeField] private TMP_Text textBox;
    [SerializeField] private TMP_Dropdown dropDown;
    [SerializeField] private TMP_Text infoText;
    [SerializeField] private TMP_Text tank1text;
    [SerializeField] private TMP_Text tank2text;
    [SerializeField] private TMP_Text tank3text;
    

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
        playerNames = new List<string>();
        players = FileHandlingScript.LoadAllPlayers();

        if (players.Count > 0) {
            foreach (PlayerSave playerSave in players) {
                playerNames.Add(playerSave.playerName);
            }

            dropDown.AddOptions(playerNames);
        }
        dropDownSelected();
    }

    public void dropDownSelected()
    {
        if (players.Count < 0)
            return;

        sPlayer = players[dropDown.value];

        string achString = "";
        foreach (string achName in sPlayer.unlockedAchievements)
        {
            achString = achString + "\n-" + achName;
        }

        infoText.text = "name: " + sPlayer.playerName 
            + "\nLevels: " + sPlayer.levelsCompleted
            + "\nMoney: $" + sPlayer.money
            + "\nTanks: " + sPlayer.tanks
            + "\nAmmo: " + sPlayer.ammo.Count
            + "\nAchievements: " + achString;

        renderStore();
    }

    void renderStore()
    {
        if (sPlayer.tanks >= 1)
            tank1text.text = "owned";
        else
            tank1text.text = "$10";

        if (sPlayer.tanks >= 2)
            tank2text.text = "owned";
        else
            tank2text.text = "$10";

        if (sPlayer.tanks >= 3)
            tank3text.text = "owned";
        else
            tank3text.text = "$10";
    }

    public void buyTank()
    {
        if (sPlayer.money > 10 && sPlayer.tanks < 3) {
            sPlayer.tanks += 1;
            sPlayer.money -= 10;
        }
        dropDownSelected();
    }

    public void buyAmmo(string s) 
    {
        if (sPlayer.ammo.Count < 15)
        {
            switch (s)
            {
                case "HEAT":
                    if (sPlayer.money >= 2) {
                        sPlayer.money -= 2;
                        sPlayer.ammo.Add("HEAT");
                    }
                    break;
                case "APFSDS":
                    if (sPlayer.money >= 1) {
                        sPlayer.money -= 1;
                        sPlayer.ammo.Add("APFSDS");
                    }
                    break;
            }
        }
        dropDownSelected();
    }

    public void addMoney()
    {
        sPlayer.money += 5;
        dropDownSelected();
    }

    void savePlayer()
    {
        FileHandlingScript.SavePlayerData(sPlayer.playerName, sPlayer);
    }

    public void addPlayer()
    {
        if (players.Exists(x => x.playerName == textBox.text))
            return;

        FileHandlingScript.SavePlayerData(textBox.text, null);
        reloadPlayers();
    }

    public void BackButton() 
    {
        savePlayer();
        GlobalData.loadedPlayer = sPlayer;
        SceneManager.LoadScene("MainMenu");
    }
}
