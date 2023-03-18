using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Event")]
public class GameEvent : ScriptableObject
{
    public HashSet<GameEventListener> listeners = new HashSet<GameEventListener>();

    public void Invoke()
    {
        foreach (var globalEventListener in listeners)
            globalEventListener.RaiseEvent();
    }

    public void Register(GameEventListener listener) => listeners.Add(listener);
    public void Unregister(GameEventListener listener) => listeners.Remove(listener);
}
