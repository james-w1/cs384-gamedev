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
        friendlyIterator = 0;
        return;
    }

    public IGameState Tick(GameControlScript gcs, GameData gameData)
    {
        // on the end of the player turn we hand over to the CPU.
        if (turnDone)
            return new CPUTurnState();

        if (gameData.gamePaused)
            return null;

        switch (currentAction)
        {
            case Actions.CHOOSING: 
                gameData.playerChoosing = true;

                if (gameData.hasEventFired("Selected Move"))
                {
                    gameData.playerChoosing = false;
                    currentAction = Actions.SELECTING_MOVE;
                }
                if (gameData.hasEventFired("Selected Attack"))
                {
                    gameData.playerChoosing = false;
                    currentAction = Actions.SELECTING_ATTACK;
                }

                //if (Input.GetKey(KeyCode.M))
                //    currentAction = Actions.SELECTING_MOVE;
                //if (Input.GetKey(KeyCode.A))
                //    currentAction = Actions.SELECTING_ATTACK;
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
                //currentAction = Actions.CHOOSING;
                if (!gameData.playerAttacking)
                {
                    currentAction = Actions.CHOOSING;
                    nextFriendly(gameData);
                }
                break;
        }
        
        return null;
    }

    public void movingMethod(GameControlScript gcs, GameData gameData)
    {
        currentFriendly = gameData.friends[friendlyIterator];
        moveRenderer.enabled = true;
        moveCamToPlayer(gameData.cam, currentFriendly);

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
        moveCamToPlayer(gameData.cam, currentFriendly);

        if (Input.GetKey(KeyCode.UpArrow))
            currentFriendly.SendMessage("UpdateAngle", -0.2f);
        if (Input.GetKey(KeyCode.DownArrow))
            currentFriendly.SendMessage("UpdateAngle", 0.2f);

        if (Input.GetKey(KeyCode.Return))
        {
            currentFriendly.SendMessage("Fire");
            currentAction = Actions.ATTACKING;
            gameData.playerAttacking = true;
            return;
        }
    }

    public void moveCamToPlayer(Camera camera, GameObject player)
    {
        if (Vector2.Distance(camera.transform.position, player.transform.position) > 1.0f)
            camera.transform.position = Vector3.Lerp(camera.transform.position, new Vector3(player.transform.position.x, player.transform.position.y, -10), Time.deltaTime);
    }

    public void nextFriendly(GameData gameData)
    {
        if ((friendlyIterator + 1) < gameData.friends.Count)
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
