using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEngine.Tilemaps;

public class GameControlScript : MonoBehaviour
{
    private IGameState currentState = new PlayerTurnState();
    private GameData gameData;

    public UnityEvent TurnUpdate;
    public UnityEvent GameWon;

    [SerializeField] private List<GameObject> _friendlies;
    [SerializeField] private List<GameObject> _enemies;

    [SerializeField] private GameObject _terrain;
    [SerializeField] private GameObject _moveSelector;

    [SerializeField] private GameObject panel; 
    [SerializeField] private GameObject pausePanel; 

    [SerializeField] private Tile tile; 
    [SerializeField] private Tilemap tilemap;

    public void Start()
    {
        panel.SetActive(false);
        pausePanel.SetActive(false);
        generateTerrain();
        gameData = new GameData(_friendlies, _enemies, _terrain, _moveSelector, Camera.main);
    }

    private float topY = 0.0f;
    private float previousY = 0.0f;

    void generateTerrain()
    {
        for (float x = -200; x < 200; x += 0.05f)
        {
            topY = previousY + UnityEngine.Random.Range(-0.03f, 0.03f);

            if (topY <= -3.0f)
                topY = -3.0f;

            for (float y = topY; y > -3; y -= 0.05f)
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
            GameWon.Invoke();

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

            currentState.Exit(this);
            currentState = newState;
            newState.Enter(this, gameData);
        }
    }

    void exitToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void escPressed()
    {
        gameData.gamePaused = !gameData.gamePaused;
        pausePanel.SetActive(gameData.gamePaused);
    }
    
    public void InjectEventIntoGameData(string e)
    {
        //Debug.Log("caught " + e);
        gameData.events.Add(e);
    }

    IEnumerator attackCooldown()
    {
        yield return new WaitForSeconds(2.0f);
        gameData.playerAttacking = false;
    }
}
