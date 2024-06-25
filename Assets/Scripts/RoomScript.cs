using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    public bool HasDoorLeft;
    public bool HasDoorRight;
    public bool HasDoorTop;
    public bool HasDoorBottom;

    private DoorDirections _entranceDoor;
    private DoorDirections _exitDoor;
    public DoorDirections EntranceDoor
    {
        get => _entranceDoor;
    }
    public DoorDirections ExitDoor
    {
        get => _exitDoor;
    }

    

    /// <summary>
    /// Finds all doors in room.
    /// </summary>
    /// <returns>List of directions in which there are doors.</returns>
    private List<DoorDirections> FindDoors()
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
        return allDoorsInRoom;
    }

    /// <summary>
    /// Randomly assigns entrance and exit doors of room.
    /// </summary>
    public void AssignDoors()
    {
        List<DoorDirections> allDoorsInRoom = FindDoors();
        int entranceDoorIndex = Random.Range(0, 2);
        _entranceDoor = allDoorsInRoom[entranceDoorIndex];
        allDoorsInRoom.RemoveAt(entranceDoorIndex);

        _exitDoor = allDoorsInRoom[0];
        return;
    }

    /// <summary>
    /// Sets entrance and exit doors based on entrance door.
    /// </summary>
    public void AssignDoors(DoorDirections entrance)
    {
        List<DoorDirections> allDoorsInRoom = FindDoors();
        if (allDoorsInRoom.IndexOf(entrance) == -1)
        {
            throw new System.Exception("Room does not have this entrance door!");
        }
        _entranceDoor = entrance;
        allDoorsInRoom.Remove(entrance);

        _exitDoor = allDoorsInRoom[0];
    }
}
