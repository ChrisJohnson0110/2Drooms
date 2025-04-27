using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterCamera : MonoBehaviour
{
    public static CenterCamera instance;

    [SerializeField] private float _cameraHeight = 10f;
    private List<GameObject> _objectsToCenterOn = new List<GameObject>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void AllignCameraCenter()
    {
        _objectsToCenterOn.Clear();

        foreach (Room room in RoomManager.instance.occupiedRooms.Values)
        {
            _objectsToCenterOn.Add(room.RoomModel);
        }

        Vector3 centerPoint = GetCenterPoint();

        Camera.main.transform.position = new Vector3(centerPoint.x, centerPoint.y + /*_cameraHeight **/ 30 + (RoomManager.instance._maxDistanceFromStart * 1.6f), centerPoint.z); //pos
        Camera.main.transform.rotation = Quaternion.Euler(90f, 0f, 0f); //dir
    }

    private Vector3 GetCenterPoint()
    {
        if (_objectsToCenterOn.Count == 1)
            return _objectsToCenterOn[0].transform.position;

        Vector3 total = Vector3.zero;
        foreach (GameObject obj in _objectsToCenterOn)
        {
            Vector3 pos = obj.transform.position;
            total += new Vector3(pos.x, 0, pos.z); 
        }

        Vector3 averageXZ = total / _objectsToCenterOn.Count;
        return new Vector3(averageXZ.x, 0f, averageXZ.z);
    }
}
