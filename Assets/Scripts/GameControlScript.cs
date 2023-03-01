using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum GameState { BEGIN, PLAYER_TURN, CPU_TURN, END }
enum PlayerTurnType { MOVE, ATTACK, RANGE_FIND }

public class GameControlScript : MonoBehaviour
{
    [SerializeField] List<GameObject> friendlies;
    [SerializeField] List<GameObject> enemies;

    [SerializeField] GameObject terrain;
    [SerializeField] GameObject moveSelector;

    private GameState gameState;
    private GameState prevState;

    private Vector3 lastMousePos;
    private Vector3 lastMousePosWClick;

    private bool inState = false;
    private bool hasSelected = false;

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
        if (gameState == GameState.PLAYER_TURN)
        {
            lastMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButtonDown(1))
            {
                lastMousePosWClick = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                hasSelected = true;
            }
        }
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
                if (friendlies.Equals(null)) {
                    Debug.Log("There are no friendlies in the scene");
                    yield break;
                }
                foreach (GameObject player in friendlies)
                {
                    // do player turn 
                    // wait for turn finished event from friendly tank object
                    //yield return new WaitUntil(true); 
                    yield return StartCoroutine(DoPlayerMoveTurn(player));
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
        yield return new WaitForSeconds(1);
        inState = false;
    }

    IEnumerator DoPlayerMoveTurn(GameObject friend)
    {
        hasSelected = false;
        RaycastHit2D rayHit = Physics2D.Raycast(lastMousePosWClick, Vector2.down);
           
        if (!terrain) yield break;

        while (true)
        {
            rayHit = Physics2D.Raycast(lastMousePos, Vector2.down);
            moveSelector.transform.position = rayHit.point;

            if (hasSelected)
            {
                rayHit = Physics2D.Raycast(lastMousePosWClick, Vector2.down);
                hasSelected = false;
                break;
            }

            yield return 0;
        }

        while (Vector2.Distance(friend.transform.position, rayHit.point) > 0.005f)
        {
            friend.transform.position = Vector2.MoveTowards(friend.transform.position, rayHit.point, 0.005f);
            yield return 0;
        }

        friend.transform.position = rayHit.point; // obvs change this lmao
        yield return new WaitForSeconds(0.1f);
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
        yield return new WaitForSeconds(1);
    }

    /*
     * setup the level using the level data
     */
    void BeginLevel()
    {
        StopAllCoroutines();
        gameState = GameState.PLAYER_TURN;
        StartCoroutine(StateChangeHandler());
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
