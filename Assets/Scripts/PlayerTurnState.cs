using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Actions {CHOOSING, SELECTING_MOVE, MOVING, SELECTING_ATTACK}

public class PlayerTurnState : IGameState
{
    private bool turnDone = false;
    private bool stoppedMoving = false;
    private GameObject currentFriendly;
    private int friendlyIterator = 0;
    private Actions currentAction = Actions.CHOOSING;

    private RaycastHit2D rayHit;
    private Vector3 validMovePos;

    public void Enter(GameControlScript gcs, GameData gameData)
    {
        Debug.Log("Entering PlayerTurnState");
        validMovePos = Vector3.negativeInfinity;
        //TankScript.StoppedEvent.AddListener(StoppedEvent);

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
                currentAction = Actions.SELECTING_MOVE;
                break;
            case Actions.SELECTING_MOVE: 
                movingMethod(gcs, gameData);
                break;
            case Actions.MOVING:
                if (stoppedMoving)
                {
                    nextFriendly(gameData);
                    stoppedMoving = false;
                }
                break;
        }
        
        return null;
    }

    public void movingMethod(GameControlScript gcs, GameData gameData)
    {
        currentFriendly = gameData.friends[friendlyIterator];

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

    public void StoppedEvent()
    {
        Debug.Log("test");
        stoppedMoving = true;
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
