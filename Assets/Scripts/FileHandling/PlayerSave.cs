using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerSave
{
    public PlayerSave(string iPlayerName)
    {
        playerName = iPlayerName;
        levelsCompleted = 0;
        unlockedAchievements = new List<string>();
    }

    public string playerName {get; set;}
    public int levelsCompleted {get; set;}
    public List<string> unlockedAchievements;
}
