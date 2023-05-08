using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AchievementSystem : MonoBehaviour
{
    private Queue<Achievement> achQueue = new Queue<Achievement>();
    [SerializeField] private TMP_Text AchText;
    private Achievement currentAch;

    public void AddToQueue(Achievement ach)
    {
        achQueue.Enqueue(ach);
    }

    private void Start()
    {
        StartCoroutine("AchQueueCheck");
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    private void Unlock(Achievement ach)
    {
        ach.Unlock();
    }

    private IEnumerator AchQueueCheck()
    {
        for (; ;)
        {
            if (achQueue.Count > 0)
            {
                currentAch = achQueue.Dequeue();
                Unlock(currentAch);
                AchText.text = currentAch.name + " unlocked";
                yield return new WaitForSeconds(5f);
            }
        }
    }
}
