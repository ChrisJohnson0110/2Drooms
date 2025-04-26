using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public RoomData startingRoom;

    public List<RoomData> availableRooms; // room prefabs
    public List<Room> spawnedRooms = new List<Room>(); // rooms
    public List<Doorway> openDoorways = new List<Doorway>(); // doors opened
    public HashSet<Vector3> occupiedPositions = new HashSet<Vector3>();
    Queue<Room> roomsToExpand = new Queue<Room>();
    public int maxRooms = 50;

    private void Start()
    {
        CreateNewRoom(startingRoom);
        //CheckRoomForGeneration(spawnedRooms[0]);
        //CheckRoomForGeneration(spawnedRooms[2]);

        roomsToExpand.Enqueue(spawnedRooms[0]);
        GenerateRooms();
    }

    void GenerateRooms()
    {
        while (roomsToExpand.Count > 0 && spawnedRooms.Count < maxRooms)
        {
            Room current = roomsToExpand.Dequeue();
            CheckRoomForGeneration(current);
        }
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
            CreateNewRoom(a_room, GetRoomToGenerate(door), door);
        }
        a_room.doorways.Clear();
    }

    //get a random tile that can be connected to the given doorway
    private RoomData GetRoomToGenerate(Direction a_doorway)
    {
        List<RoomData> PossibleRooms = new List<RoomData>();

        foreach (RoomData roomdata in availableRooms)
        {
            foreach (Direction door in roomdata.doors)
            {
                if (door == GetOppositeDirection(a_doorway))
                {
                    PossibleRooms.Add(roomdata);
                }
            }
        }

        return PossibleRooms[Random.Range(0, PossibleRooms.Count)];
    }

    //create room logic
    private void CreateNewRoom(Room parentRoom, RoomData a_roomData, Direction a_doorway)
    {
        Room newRoomInstance = new Room(a_roomData);

        Vector3 spawnPosition = parentRoom.room.transform.position + GetPosition(a_doorway);

        if (IsValidRoomPlacement(a_roomData, spawnPosition) && !occupiedPositions.Contains(spawnPosition))
        {
            newRoomInstance.room = Instantiate(newRoomInstance.RoomData.prefab, spawnPosition, transform.rotation);

            spawnedRooms.Add(newRoomInstance);
            occupiedPositions.Add(spawnPosition);

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
        newRoomInstance.room = Instantiate(newRoomInstance.RoomData.prefab, new Vector3(0,0,0), transform.rotation);
        spawnedRooms.Add(newRoomInstance);
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

    //is there a room at the given position // TODO swap to dictionary
    Room FindRoomAtPosition(Vector3 position)
    {
        foreach (Room room in spawnedRooms)
        {
            if (room.room.transform.position == position)
            {
                return room;
            }
        }
        return null;
    }

    bool IsValidRoomPlacement(RoomData roomData, Vector3 spawnPosition)
    {
        foreach (Direction dir in System.Enum.GetValues(typeof(Direction)))
        {
            Vector3 neighborPos = spawnPosition + GetPosition(dir);
            Room neighborRoom = FindRoomAtPosition(neighborPos); //error line
            if (neighborRoom != null)
            {
                // Neighbor exists, check doorways
                bool neighborHasDoorFacingUs = neighborRoom.doorways.Contains(GetOppositeDirection(dir));
                bool ourRoomHasDoorFacingNeighbor = roomData.doors.Contains(dir);

                if (neighborHasDoorFacingUs && !ourRoomHasDoorFacingNeighbor)
                {
                    // Neighbor expects a door but this room has none = bad
                    return false;
                }
                else if (!neighborHasDoorFacingUs && ourRoomHasDoorFacingNeighbor)
                {
                    // Our room expects a door but neighbor has none = bad
                    return false;
                }
            }
        }

        return true;
    }
}