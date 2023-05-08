using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementSystem : MonoBehaviour
{
    private Queue<Achievement> achQueue = new Queue<Achievement>();

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
                Unlock(achQueue.Dequeue());
                yield return new WaitForSeconds(5f);
            }
        }
    }
}
