using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Doorway
{
    public GameObject gameObjectRef;
    public Direction direction;

    public void open()
    {
        gameObjectRef.SetActive(false);
    }
}