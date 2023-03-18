using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Actions {CHOOSING, SELECTING_MOVE, MOVING, SELECTING_ATTACK, ATTACKING}

public class PlayerTurnState : IGameState
{
    private bool turnDone = false;
    private GameObject currentFriendly;
    private int friendlyIterator = 0;
    private Actions currentAction = Actions.CHOOSING;

    private RaycastHit2D rayHit;
    private Vector3 validMovePos;
    SpriteRenderer moveRenderer;

    public void Enter(GameControlScript gcs, GameData gameData)
    {
        Debug.Log("Entering PlayerTurnState");
        validMovePos = Vector3.negativeInfinity;
        return;
    }

    public IGameState Tick(GameControlScript gcs, GameData gameData)
    {

        // on the end of the player turn we hand over to the CPU.
        if (turnDone)
            return new CPUTurnState();

        switch (currentAction)
        {
            case Actions.CHOOSING: 
                if (Input.GetKey(KeyCode.M))
                    currentAction = Actions.SELECTING_MOVE;
                if (Input.GetKey(KeyCode.A))
                    currentAction = Actions.SELECTING_ATTACK;
                break;
            case Actions.SELECTING_MOVE: 
                moveRenderer = gameData.moveSelector.GetComponent<SpriteRenderer>();
                movingMethod(gcs, gameData);
                break;
            case Actions.MOVING:
                if (gameData.hasEventFired("Tank Stopped"))
                {
                    currentAction = Actions.CHOOSING;
                    moveRenderer.enabled = false;
                    nextFriendly(gameData);
                }
                break;
            case Actions.SELECTING_ATTACK: 
                attackingMethod(gcs, gameData);
                break;
            case Actions.ATTACKING: 
                    currentAction = Actions.CHOOSING;
                break;
        }
        
        return null;
    }

    public void movingMethod(GameControlScript gcs, GameData gameData)
    {
        currentFriendly = gameData.friends[friendlyIterator];
        moveRenderer.enabled = true;

        rayHit = Physics2D.Raycast(gameData.lastMousePos, Vector2.down);
        if (rayHit.collider.tag != "ground" || rayHit.distance < 0.1) {
            // invalid move
        } else {
            gameData.moveSelector.transform.position = rayHit.point;
            validMovePos = rayHit.point;
        }

        if (Input.GetMouseButtonDown(1)){
            currentFriendly.SendMessage("MoveTankTo", validMovePos);
            currentAction = Actions.MOVING;
        }
    }

    public void attackingMethod(GameControlScript gcs, GameData gameData)
    {
        currentFriendly = gameData.friends[friendlyIterator];

        if (Input.GetKey(KeyCode.UpArrow))
            currentFriendly.SendMessage("UpdateAngle", 1);
        if (Input.GetKey(KeyCode.DownArrow))
            currentFriendly.SendMessage("UpdateAngle", -1);

        if (Input.GetKey(KeyCode.A))
        {
            currentFriendly.SendMessage("Fire");
            currentAction = Actions.ATTACKING;
            return;
        }
    }

    public void nextFriendly(GameData gameData)
    {
        if (gameData.friends.Count > friendlyIterator + 2)
        {
            friendlyIterator++;
            currentAction = Actions.CHOOSING;
        } else {
            turnDone = true;
        }
    }

    public void Exit(GameControlScript gcs)
    {
        return;
    }
}
