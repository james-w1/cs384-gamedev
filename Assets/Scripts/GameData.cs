using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable]
public class GameData : IData
{
    public GameData(List<GameObject> ifriends, List<GameObject> ienemies, 
            GameObject iterrain, GameObject imoveSelector, Camera icam)
    {
        friends = ifriends;
        enemies = ienemies;
        terrain = iterrain;
        moveSelector = imoveSelector;
        cam = icam;
        events = new HashSet<string>();
    }

    public List<GameObject> friends {get; set;}
    public List<GameObject> enemies {get; set;}
    
    public GameObject terrain {get; set;}
    public GameObject moveSelector {get; set;}

    public Vector3 lastMousePos {get; set;}
    public Camera cam {get; set;}

    public HashSet<string> events {get; set;}

    public int turn;

    public PlayerSave currentPlayer;

    public bool hasEventFired(string e) {
        if (this.events.Contains(e)) {
            this.events.Remove(e);
            return true;
        }

        return false;
    }
}
