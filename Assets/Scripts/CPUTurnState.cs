using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CPUTurnState : IGameState
{
    private int enemyIterator = 0;
    private GameObject currentEnemy;
    private bool hasMoved = false;
    private bool turnDone = false;
    private Vector3 currentPos = Vector3.negativeInfinity;

    public void Enter(GameControlScript gcs, GameData gamedata)
    {
        Debug.Log("Entering CPU Turn State");
        return;
    }

    public IGameState Tick(GameControlScript gcs, GameData gameData)
    {
        if (turnDone)
            return new PlayerTurnState();

        if (!hasMoved)
        {
            currentEnemy = gameData.enemies[enemyIterator];
            currentPos = currentEnemy.transform.position;
            currentEnemy.SendMessage("MoveTankTo", 
                    currentPos + new Vector3(currentPos.x + UnityEngine.Random.Range(-1f, 1f), 0, 0));
        }

        hasMoved = true;

        if (gameData.hasEventFired("Tank Stopped"))
        {
            nextTank(gameData);
        }

        return null;
    }

    public void nextTank(GameData gameData)
    {
        if ((enemyIterator + 1) < gameData.enemies.Count)
        {
            enemyIterator++;
        } else {
            turnDone = true;
        }
    }

    public void Exit(GameControlScript gcs)
    {
        return;
    }
}
