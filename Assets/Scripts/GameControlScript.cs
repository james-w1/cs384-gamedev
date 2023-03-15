using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameControlScript : MonoBehaviour
{
    private IGameState currentState = new PlayerTurnState();
    private GameData gameData;

    [SerializeField] private List<GameObject> _friendlies;
    [SerializeField] private List<GameObject> _enemies;

    [SerializeField] private GameObject _terrain;
    [SerializeField] private GameObject _moveSelector;

    public void Start()
    {
        gameData = new GameData(_friendlies, _enemies, _terrain, _moveSelector, Camera.main);
    }

    void Update()
    {
        gameData.lastMousePos = gameData.cam.ScreenToWorldPoint(Input.mousePosition);
        UpdateState();
    }

    void UpdateState()
    {
        IGameState newState = currentState.Tick(this, gameData);

        if (newState != null)
        {
            currentState.Exit(this);
            currentState = newState;
            newState.Enter(this, gameData);
        }
    }
}
