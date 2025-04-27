using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public RoomData RoomData;
    public GameObject room;
    public List<Room> neighbours =  new List<Room>(); 
    public List<Direction> doorways = new List<Direction>();

    public Room(RoomData a_roomData)
    {
        RoomData = a_roomData;
        foreach (Direction d in RoomData.doors)
        {
            doorways.Add(d);
        }
    }

}
