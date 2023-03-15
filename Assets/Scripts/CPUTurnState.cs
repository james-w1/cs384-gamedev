using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CPUTurnState : IGameState
{
    private bool waitDone = false;
    private float enterTime;

    public void Enter(GameControlScript gcs, GameData gamedata)
    {
        Debug.Log("Entering PlayerTurnState");
        enterTime = Time.time;
        return;
    }

    public IGameState Tick(GameControlScript gcs, GameData gameData)
    {
        if (enterTime - Time.time > 5.0f)
            return new PlayerTurnState();

        return null;
    }

    public void Exit(GameControlScript gcs)
    {
        return;
    }
}
