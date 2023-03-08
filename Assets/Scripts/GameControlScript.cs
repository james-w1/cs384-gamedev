using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum GameState { BEGIN, PLAYER_TURN, CPU_TURN, END }
enum PlayerTurnType { MOVE, ATTACK, RANGE_FIND }

public class GameControlScript : MonoBehaviour
{
    [SerializeField] private List<GameObject> friendlies;
    [SerializeField] private List<GameObject> enemies;

    [SerializeField] private GameObject terrain;
    [SerializeField] private GameObject moveSelector;

    private Renderer moveSelectorRenderer;
    private RaycastHit2D rayHit;
    private Vector3 validMovePos;
    private Camera camera;

    private GameState gameState;
    private PlayerTurnType playerTurnType;

    private Vector3 lastMousePos;
    private Vector3 lastMousePosWClick;

    private bool inState = false;
    private bool hasSelected = false;

    private float speed = 0.003f; // dont ask
    private Dictionary<string, PlayerTurnType> keyMapping 
        = new Dictionary<string, PlayerTurnType>()
    {
        {"M", PlayerTurnType.MOVE},
        {"A", PlayerTurnType.ATTACK},
        {"R", PlayerTurnType.RANGE_FIND},
    };
    string chosenKey;

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
        if (gameState == GameState.PLAYER_TURN && playerTurnType == PlayerTurnType.MOVE)
        {
            lastMousePos = camera.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButtonDown(1))
            {
                lastMousePosWClick = camera.ScreenToWorldPoint(Input.mousePosition);
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
                    yield return StartCoroutine(DecidePlayerTurn());
                    
                    // do player turn 
                    // wait for turn finished event from friendly tank object
                    switch (playerTurnType)
                    {
                        case PlayerTurnType.ATTACK:
                            yield return StartCoroutine(DoPlayerAttackTurn(player));
                            break;
                        case PlayerTurnType.MOVE:
                            yield return StartCoroutine(DoPlayerMoveTurn(player));
                            break;
                    }
                }
                gameState = GameState.CPU_TURN;
                break;
            case GameState.CPU_TURN:
                yield return StartCoroutine(DoCPUTurn());
                gameState = GameState.PLAYER_TURN;
                break;
            case GameState.END:
                EndLevel();
                break;
        }
        inState = false;
    }

    IEnumerator DoPlayerAttackTurn(GameObject friend)
    {
        Debug.Log("Do Player Attack Turn");
        yield break;
    }

    IEnumerator DoPlayerMoveTurn(GameObject friend)
    {
        hasSelected = false;
           
        if (!terrain || !friend)
            yield break;

        while (!hasSelected)
        {
            moveSelectorRenderer.enabled = true;
            rayHit = Physics2D.Raycast(lastMousePos, Vector2.down);

            if ((rayHit.collider.tag != "ground") || (rayHit.distance < 0.1))
            {
                // invalid move position logic
            } 
            else 
            {
                moveSelector.transform.position = rayHit.point;
                validMovePos = rayHit.point;
            }

            yield return new WaitForEndOfFrame();
        }
        hasSelected = false;

        float sTime = Time.time;
        while (Vector2.Distance(friend.transform.position, validMovePos) > speed)
        {
            if (Time.time - sTime > 5.0) {
                Debug.Log("Move Timed Out...");
                moveSelectorRenderer.enabled = false;
                yield break;
            }

            friend.transform.position = Vector2.MoveTowards(friend.transform.position, validMovePos, speed);
            yield return new WaitForEndOfFrame();
        }

        friend.transform.position = rayHit.point;
        moveSelectorRenderer.enabled = false;
        Debug.Log("Player Move Turn Done");
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
        Debug.Log("Do CPU");
        yield return new WaitForSeconds(3f);
    }

    IEnumerator DecidePlayerTurn()
    {
        chosenKey = null;
        Debug.Log("Select A Turn Type");

        while (chosenKey == null)
        {
            if (Input.GetKey(KeyCode.A))
                chosenKey = "A";
            if (Input.GetKey(KeyCode.M))
                chosenKey = "M";
            if (Input.GetKey(KeyCode.R))
                chosenKey = "R";

            yield return 0;
        }
        
        playerTurnType = keyMapping[chosenKey];
    }

    /*
     * setup the level using the level data
     */
    void BeginLevel()
    {
        gameState = GameState.PLAYER_TURN;
        moveSelectorRenderer = moveSelector.GetComponent<Renderer>(); 
        camera = Camera.main;
        moveSelectorRenderer.enabled = false;
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
