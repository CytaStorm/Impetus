using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    public bool HasDoorLeft;
    public bool HasDoorRight;
    public bool HasDoorTop;
    public bool HasDoorBottom;

    [HideInInspector]
    public DoorDirections EntranceDoor;
    [HideInInspector]
    public DoorDirections ExitDoor;
    
    /// <summary>
    /// Assigns entrance and exit doors of room.
    /// </summary>
    public void AssignDoors()
    {
        List<DoorDirections> allDoorsInRoom = new List<DoorDirections>();
        if (HasDoorTop)
        {
            allDoorsInRoom.Add(DoorDirections.Top);
        }
        if (HasDoorBottom)
        {
            allDoorsInRoom.Add(DoorDirections.Bottom);
        }
        if (HasDoorLeft)
        {
            allDoorsInRoom.Add(DoorDirections.Left);
        }
        if (HasDoorRight)
        {
            allDoorsInRoom.Add(DoorDirections.Right);
        }

        int entranceDoorIndex = Random.Range(0, 2);
        EntranceDoor = allDoorsInRoom[entranceDoorIndex];
        allDoorsInRoom.RemoveAt(entranceDoorIndex);

        ExitDoor = allDoorsInRoom[0];
        return;
    }
}
