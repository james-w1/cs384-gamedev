using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnScript : MonoBehaviour
{
    private int turnNumber;
    private string[] turnType = {"Waiting", "Player", "CPU"};
    [SerializeField] private TMP_Text textObject;

    private void Start()
    {
        textObject.text = "testing text";
    }

    private void updateRenderedText()
    {
        
    }

    public void IncrementTurnTimer()
    {
        turnNumber++;
    }
}
