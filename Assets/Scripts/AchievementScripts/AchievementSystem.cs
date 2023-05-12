using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AchievementSystem : MonoBehaviour
{
    private Queue<Achievement> achQueue = new Queue<Achievement>();
    [SerializeField] private TMP_Text AchText;
    [SerializeField] private Achievement[] achievements;

    private void OnEnable()
    {

    }

    public void oneShotAch()
    {
        UnlockAch(new Achievement("JuanDeeg", "Finish Game With One Shot"));
    }

    public void lessThan5()
    {
        UnlockAch(new Achievement("<5", "Finish the level in < 5 turns"));
    }

    private void UnlockAch(Achievement ach)
    {
        AchText.text = ach.achName + " unlocked";
        ach.Unlock();
    }
}
