using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum GameState { BEGIN, PLAYER_TURN, CPU_TURN, END }
enum PlayerTurnType { MOVE, ATTACK, RANGE_FIND }

public class GameControlScript : MonoBehaviour
{
    public List<GameObject> friendlies;
    public List<GameObject> enemies;

    public GameState gameState;
    public GameState prevState;

    private bool inState = false;

    /*
     * Start is called before the first frame update
     *
     * In this case we change the gameState var and enter the 
     * StateChangeHandler() func to set stuff up.
     */
    void Start()
    {
        this.gameState = GameState.BEGIN;
        StartCoroutine(StateChangeHandler());
    }

    // Update is called once per frame
    void Update()
    {
        if (!inState)
        {
            StartCoroutine(StateChangeHandler());
        }
    }

    /*
     * Handles entering new states and changing between states in the game loop.
     */
    IEnumerator StateChangeHandler() 
    {
        inState = true;
        switch (this.gameState)
        {
            case GameState.BEGIN:
                BeginLevel();
                break;
            case GameState.PLAYER_TURN:
                foreach (GameObject player in friendlies)
                {
                    // do player turn 
                    // wait for turn finished event from friendly tank object
                    yield return new WaitUntil(true); 
                }
                gameState = GameState.CPU_TURN;
                break;
            case GameState.CPU_TURN:
                DoCPUTurn();
                break;
            case GameState.END:
                EndLevel();
                break;
        }
        inState = false;
    }

    /*
     * Handles the cpu turns
     */
    IEnumerator DoCPUTurn() 
    {
        /*
         * ## Notes:
         *  - CPU tanks will need to move if they have been close to a prev 
         *      player shot
         *  - They will need to adjust fire if they missed a prev shot
         *  - if they havent done either then 50/50 move or shoot
         *      - if it hasnt shot yet then introduce some random error to the 
         *          shot to give the player a chance
         */
    }

    /*
     * setup the level using the level data
     */
    void BeginLevel()
    {
        // load the player and enemies from the level file
    }

    /*
     * Handles the end of one game and return to level selection scene
     */
    void EndLevel()
    {
        // delete all objects, change scene and save replay to a file
        StopAllCoroutines();
    }
}
