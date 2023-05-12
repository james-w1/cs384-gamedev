using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Achievement : IAch
{
    public string achName;
    public string achDescription;
    private bool unlocked;

    public Achievement(string n, string d)
    {
        this.achName = n;
        this.achDescription = d;
        unlocked = false;
    }

    public void Unlock()
    {
        if (!unlocked)
        {
            unlocked = true;
            GlobalData.loadedPlayer.unlockedAchievements.Add(this.achName);
            Debug.Log(GlobalData.loadedPlayer.unlockedAchievements[0]);
            FileHandlingScript.SavePlayerData(GlobalData.loadedPlayer.playerName, GlobalData.loadedPlayer);
        }
    }

}
