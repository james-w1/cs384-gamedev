using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Level
{
    [Serialized] public static List<Vector2> FriendlyPosList { get; }
    [Serialized] public static List<Vector2> EnemyPosList { get; }

    public static string name;
    // store tilemap in this struct
}

public class LevelScript : MonoBehaviour
{
    // behaviour of levels gg maybe a start func idk
}
