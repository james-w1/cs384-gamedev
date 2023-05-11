using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

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

    public void Start()
    {
        panel.SetActive(false);
        pausePanel.SetActive(false);
        gameData = new GameData(_friendlies, _enemies, _terrain, _moveSelector, Camera.main);
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
