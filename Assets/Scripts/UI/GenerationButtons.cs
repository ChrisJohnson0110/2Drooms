using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GenerationButtons : MonoBehaviour
{
    [SerializeField] private Slider _numberOfRooms;
    [SerializeField] private Slider _distanceFromStart;

    [SerializeField] private TMP_Text _numberOfRoomsNumberDisplay;
    [SerializeField] private TMP_Text _distanceFromStartNumberDisplay;

    private void Start()
    {
        _numberOfRooms.value = RoomManager.instance._maxRooms;
        _distanceFromStart.value = RoomManager.instance._maxDistanceFromStart;

        _numberOfRooms.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        _distanceFromStart.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        ValueChangeCheck();
    }

    private void ValueChangeCheck()
    {
        _numberOfRoomsNumberDisplay.text = _numberOfRooms.value.ToString();
        _distanceFromStartNumberDisplay.text = _distanceFromStart.value.ToString();
    }

    public void newGeneration()
    {
        RoomManager.instance.ClearGeneration();
        RoomManager.instance.StartMapGeneration(((int)_numberOfRooms.value), ((int)_distanceFromStart.value));

    }
}
