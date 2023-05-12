using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CPUTurnState : IGameState
{
    private int enemyIterator = 0;
    private GameObject currentEnemy;
    private bool hasMoved;
    private bool hasShot;
    private bool turnDone;
    private bool chosenTurnType;
    private Vector3 currentPos;
    private int rand;

    public void Enter(GameControlScript gcs, GameData gamedata)
    {
        hasMoved = false;
        hasShot = false;
        turnDone = false;
        chosenTurnType = false;
        rand = 999;
        currentPos = Vector3.negativeInfinity;
        return;
    }

    public IGameState Tick(GameControlScript gcs, GameData gameData)
    {
        currentEnemy = gameData.enemies[enemyIterator];

        if (currentEnemy == null)
        {
            gameData.events.Add("Enemy Killed");
            gameData.enemies.RemoveAt(enemyIterator);
            nextTank(gameData);
        }

        if (turnDone)
            return new PlayerTurnState();

        moveCamToPlayer(gameData.cam, currentEnemy);

        if (!chosenTurnType) {
            rand = Random.Range(0, 2);
            chosenTurnType = true;
        } else {
            if (rand == 0)
                movingTurn(gameData);

            if (rand == 1)
                attackingTurn(gameData);
        }
        return null;
    }

    private void movingTurn(GameData gameData)
    {
        if (!hasMoved)
        {
            currentPos = currentEnemy.transform.position;
            currentEnemy.SendMessage("MoveTankTo", 
                    currentPos + new Vector3(UnityEngine.Random.Range(-5f, 5f), 0, 0));
        }

        hasMoved = true;

        if (gameData.hasEventFired("Tank Stopped"))
        {
            hasMoved = false;
            chosenTurnType = false;
            nextTank(gameData);
        }
    }

    private void attackingTurn(GameData gameData)
    {
        // scuffed
        currentEnemy.SendMessage("UpdatePower", -1000f);

        if (!hasShot)
        {
            hasShot = true;
            currentEnemy.SendMessage("Fire");
        }
        chosenTurnType = false;
        nextTank(gameData);
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
