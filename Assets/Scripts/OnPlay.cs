using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPlay : MonoBehaviour
{
    void Start()
    {
        RoomManager.instance.StartMapGeneration();
    }

    public void newGeneration()
    {
        RoomManager.instance.ClearGeneration();
        RoomManager.instance.StartMapGeneration();
    }
}
