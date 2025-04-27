using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPlay : MonoBehaviour
{
    void Start()
    {
        RoomManager.instance.StartMapGeneration(-1,-1);
    }
}
