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
        money = 0;
        tanks = 1;
        unlockedAchievements = new List<string>();
        ammo = new List<string>();
        for (int i = 0; i < 5; i++)
            ammo.Add("HEAT");
    }

    public string playerName {get; set;}
    public int levelsCompleted {get; set;}
    public int money {get; set;}
    public int tanks {get; set;}
    public List<string> ammo {get; set;}
    public List<string> unlockedAchievements;
}
