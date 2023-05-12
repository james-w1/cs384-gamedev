using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using UnityEngine.Tilemaps;

public class GameControlScript : MonoBehaviour
{
    private IGameState currentState = new PlayerTurnState();
    private GameData gameData;

    public UnityEvent TurnUpdate;
    public UnityEvent GameWon;
    public UnityEvent EnemyKilled;
    public UnityEvent FriendlyKilled;
    public UnityEvent Less5Turns;
    public UnityEvent OneShot;

    [SerializeField] private GameObject _friend;
    [SerializeField] private GameObject _enemy;

    [SerializeField] private List<GameObject> _friendlies;
    [SerializeField] private List<GameObject> _enemies;

    [SerializeField] private GameObject _terrain;
    [SerializeField] private GameObject _moveSelector;

    [SerializeField] private GameObject panel; 
    [SerializeField] private GameObject pausePanel; 

    [SerializeField] private Tile tile; 
    [SerializeField] private Tilemap tilemap;

    [SerializeField] private Slider turnSlider;

    public int turnNumber;
    private int _twoCounter;

    private List<GameData> turnList;

    public void Start()
    {
        turnNumber = 1;
        panel.SetActive(false);
        pausePanel.SetActive(false);
        generateTerrain();
        spawnTanks();
        gameData = new GameData(_friendlies, _enemies, _terrain, _moveSelector, Camera.main);
        turnList = new List<GameData>();
        turnList.Add(gameData);
        TurnUpdate.Invoke();
    }

    void spawnTanks()
    {
        int friendlies = GlobalData.loadedPlayer.tanks;

        if (friendlies == 2) {
            GameObject friend2 = Instantiate(_friend);
            friend2.transform.position = new Vector3(friend2.transform.position.x + UnityEngine.Random.Range(-10f, -8f),0,0);
            _friendlies.Add(friend2);
        }
        if (friendlies == 3) {
            GameObject friend2 = Instantiate(_friend);
            friend2.transform.position = new Vector3(friend2.transform.position.x + UnityEngine.Random.Range(-8f, -5f),0,0);
            GameObject friend3 = Instantiate(_friend);
            friend3.transform.position = new Vector3(friend3.transform.position.x + UnityEngine.Random.Range(-10f, -12f),0,0);
            _friendlies.Add(friend2);
            _friendlies.Add(friend3);
        }

        int enemies = GlobalData.loadedPlayer.levelsCompleted;
    
        Vector3 newPos = new Vector3(_enemy.transform.position.x,0,0);
        for (int i = 0; i < enemies; i++) {
            if (i > 3)
                break;
            GameObject gO = Instantiate(_enemy);
            newPos = new Vector3(newPos.x + UnityEngine.Random.Range(5f, 8f),0,0);
            gO.transform.position = newPos;
            _enemies.Add(gO);
        }
    }

    private float topY = 0.0f;
    private float previousY = 0.0f;
    private float count = 0;

    void generateTerrain()
    {
        for (float x = -30; x < 30; x += 0.05f)
        {
            count+= 0.05f;
            if (count > 0.5f)
            {
                topY = previousY + UnityEngine.Random.Range(-0.1f, 0.1f);
                count = 0f;
            } else {
                topY = previousY + UnityEngine.Random.Range(-0.05f, 0.05f);
            }

            if (topY <= -3.0f)
                topY = -3.0f;

            if (topY >= 1.0f)
                topY = 1.0f;

            for (float y = topY; y > -5; y -= 0.05f)
            {
                var tilePos = tilemap.WorldToCell(new Vector2(x, y));
                tilemap.SetTile(tilePos, tile);
            }
            previousY = topY;
        }

    }

    void Update()
    {
        gameData.lastMousePos = gameData.cam.ScreenToWorldPoint(Input.mousePosition);

        panel.SetActive(gameData.playerChoosing);

        if (Input.GetKeyDown(KeyCode.Escape))
            escPressed();

        if (gameData.playerAttacking)
            StartCoroutine("attackCooldown");

        if (gameData.enemies.Count <= 0)
            GameWon?.Invoke();

        if (gameData.hasEventFired("Enemy Killed"))
        {
            GlobalData.loadedPlayer.money += 5;
            EnemyKilled?.Invoke();
            if (gameData.shotsFired < 2)
                OneShot?.Invoke();
        }

        if (gameData.hasEventFired("Friendly Killed"))
            FriendlyKilled?.Invoke();

        UpdateState();
    }

    void UpdateState()
    {
        if ( gameData.gamePaused )
            return;

        IGameState newState = currentState.Tick(this, gameData);

        if (newState != null)
        {
            TurnUpdate.Invoke();
            if (_twoCounter == 1) {
                newTurn();
                _twoCounter = 0;
            } else {
                _twoCounter++;
            }

            currentState.Exit(this);
            currentState = newState;
            newState.Enter(this, gameData);
        }
    }

    private void newTurn()
    {
        //GameData newData = gameData.clone(gameData);
        //turnList.Add(gameData);
        //gameData = newData;

        turnNumber += 1;
        turnSlider.maxValue += 1;
        turnSlider.value = turnSlider.maxValue;
    }

    public void exitToMenu()
    {
        GlobalData.loadedPlayer.levelsCompleted++;

        if (turnNumber < 5)
            Less5Turns?.Invoke();
        
        SceneManager.LoadScene("MainMenu");
    }

    void escPressed()
    {
        gameData.gamePaused = !gameData.gamePaused;
        pausePanel.SetActive(gameData.gamePaused);
    }

    public void turnSliderUpdate()
    {
        if (turnSlider.value == turnNumber || turnSlider.value < 1)
            return;

        goToTurn((int)turnSlider.value);
    }

    private void goToTurn(int turnNo)
    {
        gameData = turnList[turnNo];
        Debug.Log(turnNo);
    }

    // Adds a gameEvent to the HashSet containing fired gameEvents. 
    // There is probably a better way to do this
    public void InjectEventIntoGameData(string e)
    {
        gameData.events.Add(e);
    }

    IEnumerator attackCooldown()
    {
        yield return new WaitForSeconds(2.0f);
        gameData.playerAttacking = false;
    }
}
