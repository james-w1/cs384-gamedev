using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CPUTurnState : IGameState
{
    public void Enter(GameControlScript gcs, GameData gamedata)
    {
        Debug.Log("Entering CPU Turn State");
        return;
    }

    public IGameState Tick(GameControlScript gcs, GameData gameData)
    {
        return new PlayerTurnState();
    }

    public void Exit(GameControlScript gcs)
    {
        return;
    }
}
