using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CPUTurnState : IGameState
{
    private int enemyIterator = 0;
    private GameObject currentEnemy;
    private bool hasMoved;
    private bool turnDone;
    private Vector3 currentPos;

    public void Enter(GameControlScript gcs, GameData gamedata)
    {
        hasMoved = false;
        turnDone = false;
        currentPos = Vector3.negativeInfinity;
        return;
    }

    public IGameState Tick(GameControlScript gcs, GameData gameData)
    {
        currentEnemy = gameData.enemies[enemyIterator];

        if (turnDone)
            return new PlayerTurnState();

        moveCamToPlayer(gameData.cam, currentEnemy);

        if (!hasMoved)
        {
            currentPos = currentEnemy.transform.position;
            currentEnemy.SendMessage("MoveTankTo", 
                    currentPos + new Vector3(UnityEngine.Random.Range(-5f, 5f), 0, 0));
        }

        hasMoved = true;

        if (gameData.hasEventFired("Tank Stopped"))
        {
            nextTank(gameData);
            hasMoved = false;
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

    public void moveCamToPlayer(Camera camera, GameObject player)
    {
        if (Vector2.Distance(camera.transform.position, player.transform.position) > 1.0f)
            camera.transform.position = Vector3.Lerp(camera.transform.position, new Vector3(player.transform.position.x, player.transform.position.y, -10), Time.deltaTime);
    }
    public void Exit(GameControlScript gcs)
    {
        return;
    }
}
