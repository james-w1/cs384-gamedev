using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnScript : MonoBehaviour
{
    private int turnNumber;
    private int turnNoType;
    private string[] turnType = {"", "Player", "CPU", ""};
    [SerializeField] private TMP_Text textObject;

    private void Start()
    {
        textObject.text = "testing text";
    }

    private void updateRenderedText()
    {
        if (turnNoType > 2)
            turnNoType = 1;
        textObject.text = "Turn: " + turnNumber + " : " + turnType[turnNoType];
    }

    public void IncrementTurnTimer()
    {
        turnNumber++;
        turnNoType++;
        updateRenderedText();
    }
}
