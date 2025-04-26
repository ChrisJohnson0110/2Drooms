using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public RoomData startingRoom;

    public List<RoomData> availableRooms; // room prefabs
    public List<Room> spawnedRooms = new List<Room>(); // rooms
    public List<Doorway> openDoorways = new List<Doorway>(); // doors opened
    //public HashSet<Vector3> occupiedPositions = new HashSet<Vector3>();
    Queue<Room> roomsToExpand = new Queue<Room>();
    public int maxRooms = 50;

    public Dictionary<Vector3, Room> occupiedRooms = new Dictionary<Vector3, Room>();
    public RoomData endCapRoom;

    private void Start()
    {
        CreateNewRoom(startingRoom);

        roomsToExpand.Enqueue(spawnedRooms[0]);

        StartCoroutine(GenerateRooms());
    }

    
    IEnumerator GenerateRooms()
    {
        while (roomsToExpand.Count > 0 && spawnedRooms.Count < maxRooms)
        {
            Room current = roomsToExpand.Dequeue();
            CheckRoomForGeneration(current);
            yield return new WaitForSeconds(0.03f); 
        }

        PlacePlugs();
    }

    //if any doorways within this room havent been generated
    private void CheckRoomForGeneration(Room a_room)
    {
        if (a_room.doorways == null)
        {
            return;
        }

        foreach (Direction door in a_room.doorways)
        {
            Vector3 spawnPosition = a_room.room.transform.position + GetPosition(door);
            RoomData roomToGenerate = GetRoomToGenerate(door, spawnPosition);
            if (roomToGenerate != null)
            {
                CreateNewRoom(a_room, roomToGenerate, door);
            }
            else
            {
                Debug.LogWarning("Couldn't find valid room for spawn position " + spawnPosition);

                // Spawn end cap room
                if (!occupiedRooms.ContainsKey(spawnPosition)) // extra safety
                {
                    CreateNewRoom(a_room, endCapRoom, door);
                }
            }
        }

        a_room.doorways.Clear();
    }

    //get a random tile that can be connected to the given doorway
    private RoomData GetRoomToGenerate(Direction a_doorway, Vector3 spawnPosition)
    {
        List<RoomData> PossibleRooms = new List<RoomData>();

        foreach (RoomData roomdata in availableRooms)
        {
            foreach (Direction door in roomdata.doors)
            {
                if (door == GetOppositeDirection(a_doorway))
                {
                    if (IsValidRoomPlacement(roomdata, spawnPosition))
                    {
                        PossibleRooms.Add(roomdata);
                    }
                }
            }
        }

        if (PossibleRooms.Count == 0)
        {
            return null; // couldn't find valid room
        }

        return PossibleRooms[Random.Range(0, PossibleRooms.Count)];
    }

    //create room logic
    private void CreateNewRoom(Room parentRoom, RoomData a_roomData, Direction a_doorway)
    {
        Room newRoomInstance = new Room(a_roomData);

        Vector3 spawnPosition = parentRoom.room.transform.position + GetPosition(a_doorway);

        if (IsValidRoomPlacement(a_roomData, spawnPosition) && FindRoomAtPosition(spawnPosition) == null) 
        {
            newRoomInstance.room = Instantiate(newRoomInstance.RoomData.prefab, spawnPosition, transform.rotation);

            spawnedRooms.Add(newRoomInstance);
            occupiedRooms[spawnPosition] = newRoomInstance;

            roomsToExpand.Enqueue(newRoomInstance);
        }
        else
        {
            Debug.Log("overlap");
            return;
        }

        spawnedRooms.Add(newRoomInstance);
    }

    //starting room
    private void CreateNewRoom(RoomData a_roomData)
    {
        Room newRoomInstance = new Room(a_roomData);
        newRoomInstance.room = Instantiate(newRoomInstance.RoomData.prefab, Vector3.zero, transform.rotation);
        spawnedRooms.Add(newRoomInstance);
        occupiedRooms[Vector3.zero] = newRoomInstance;
    }

    //get the opposite direction to the given direction
    private Direction GetOppositeDirection(Direction dir)
    {
        switch (dir)
        {
            case Direction.North: return Direction.South;
            case Direction.South: return Direction.North;
            case Direction.East: return Direction.West;
            case Direction.West: return Direction.East;
            default: return dir;
        }
    }
    
    //get the position of the next room
    private Vector3 GetPosition(Direction dir)
    {
        switch (dir)
        {
            case Direction.North: return new Vector3(0, 0, 10);
            case Direction.South: return new Vector3(0, 0, -10);
            case Direction.East: return new Vector3(10, 0, 0);
            case Direction.West: return new Vector3(-10, 0, 0);
            default: return new Vector3(0,0,0);
        }
    }

    //is there a room at the given position 
    Room FindRoomAtPosition(Vector3 position)
    {
        if (occupiedRooms.TryGetValue(position, out Room room))
        {
            return room;
        }
        return null;
    }

    bool IsValidRoomPlacement(RoomData roomData, Vector3 spawnPosition)
    {
        foreach (Direction dir in System.Enum.GetValues(typeof(Direction)))
        {
            Vector3 neighborPos = spawnPosition + GetPosition(dir);
            if (occupiedRooms.TryGetValue(neighborPos, out Room neighborRoom))
            {
                bool neighborHasDoorFacingUs = neighborRoom.RoomData.doors.Contains(GetOppositeDirection(dir));
                bool ourRoomHasDoorFacingNeighbor = roomData.doors.Contains(dir);

                if (neighborHasDoorFacingUs && !ourRoomHasDoorFacingNeighbor)
                {
                    // Neighbor expects a door, but we don't have one — BAD
                    return false;
                }
                if (!neighborHasDoorFacingUs && ourRoomHasDoorFacingNeighbor)
                {
                    // We have a door, but neighbor doesn't expect one — BAD
                    return false;
                }
            }
        }

        return true;
    }

    //check all tiles for gaps and place plugs
    void PlacePlugs()
    {
        List<Vector3> checkedPositions = new List<Vector3>();

        foreach (var kvp in occupiedRooms)
        {
            Vector3 roomPos = kvp.Key;

            foreach (Direction dir in System.Enum.GetValues(typeof(Direction)))
            {
                Vector3 neighborPos = roomPos + GetPosition(dir);

                if (occupiedRooms.ContainsKey(neighborPos) || checkedPositions.Contains(neighborPos))
                {
                    // Already has room there or already checked
                    continue;
                }

                // Spawn plug
                GameObject plug = Instantiate(endCapRoom.prefab, neighborPos, Quaternion.identity, transform);
                checkedPositions.Add(neighborPos);
            }
        }
    }


}