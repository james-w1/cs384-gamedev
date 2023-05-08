using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(fileName = "Data", 
    menuName = "Achievement/newAchievement", order = 1)]
public class Achievement : ScriptableObject
{
    [SerializeField] private string achName;
    [SerializeField] private string achDescription;
    [SerializeField] private TMP_Text AchText;
    private bool unlocked;

    public void Start()
    {
    }

    // Update is called once per frame
    public void Unlock()
    {
        if (!unlocked)
        {
            unlocked = false;
            // unlock animation in UI
            AchText.text = name + " unlocked";
        }
    }

}
