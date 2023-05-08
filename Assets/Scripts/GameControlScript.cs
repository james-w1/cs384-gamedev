using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameControlScript : MonoBehaviour
{
    private IGameState currentState = new PlayerTurnState();
    private GameData gameData;

    public UnityEvent TurnUpdate;

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
            TurnUpdate.Invoke();

            currentState.Exit(this);
            currentState = newState;
            newState.Enter(this, gameData);
        }
    }
    
    public void InjectEventIntoGameData(string e)
    {
        //Debug.Log("caught " + e);
        gameData.events.Add(e);
    }
}
