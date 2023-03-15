using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    }

    public List<GameObject> friends {get; set;}
    public List<GameObject> enemies {get; set;}
    
    public GameObject terrain {get; set;}
    public GameObject moveSelector {get; set;}

    public Vector3 lastMousePos {get; set;}
    public Camera cam {get; set;}
}
