using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { North, East, South, West }

[CreateAssetMenu(fileName = "RoomData", menuName = "ScriptableObjects/RoomData", order = 1)]
public class RoomData : ScriptableObject
{
    [field: SerializeField] public GameObject prefab { get; private set; }
    [field: SerializeField] public List<Direction> doors { get; private set; }
}