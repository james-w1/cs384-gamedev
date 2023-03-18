using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IData
{
    public List<GameObject> friends {get; set;}
    public List<GameObject> enemies {get; set;}
    
    public GameObject terrain {get; set;}
    public GameObject moveSelector {get; set;}

    public Vector3 lastMousePos {get; set;}
    public Camera cam {get; set;}

    public HashSet<string> events {get; set;}
}
