using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [SerializeField] GameEvent gameEvent;
    [SerializeField] UnityEvent unityEvent;

    void Awake() => gameEvent.Register(this);
    void OnDestroy() => gameEvent.Unregister(this);
    public void RaiseEvent() => unityEvent.Invoke();
}
