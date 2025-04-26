using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { North, East, South, West }

[CreateAssetMenu(fileName = "RoomData", menuName = "ScriptableObjects/RoomData", order = 1)]
public class RoomData : ScriptableObject
{
    public GameObject prefab;
    public List<Doorway> doors;
}