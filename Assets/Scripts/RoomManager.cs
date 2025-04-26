using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public List<RoomData> availableRooms; // List of room templates
    public List<RoomInstance> spawnedRooms = new List<RoomInstance>(); // Rooms spawned so far
    public List<Doorway> openDoorways = new List<Doorway>(); // Doorways waiting to be connected
}
