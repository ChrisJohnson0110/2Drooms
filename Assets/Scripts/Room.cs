using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public RoomData RoomData;
    public List<Room> neighbours =  new List<Room>();
    public List<Doorway> doorways = new List<Doorway>();

    public Room(RoomData a_roomData)
    {
        RoomData = a_roomData;
        foreach (Doorway go in RoomData.doors)
        {

        }
    }

}
