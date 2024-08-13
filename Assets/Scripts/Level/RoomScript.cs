using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Unity.VisualScripting;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
	[SerializeField] private List<GameObject> _doors;
	public bool HasTopDoor;
	public bool HasBottomDoor;
	public bool HasLeftDoor;
	public bool HasRightDoor;

	public string ExitDoorDirection { get; private set; }

	public GameObject EntranceDoor { get; private set; }
	public GameObject ExitDoor { get; private set; }
	public DoorScript EntranceDoorScript { get; private set; }
	public DoorScript ExitDoorScript { get; private set; }

	public void SetupFirstRoomDoor()
	{
		//Set entrance door
		int entranceDoorIndex = Random.Range(0, 2);
		foreach (GameObject door in _doors)
		{
			if (door.tag == _doors[entranceDoorIndex].tag)
			{
				EntranceDoor = door;
				continue;
			}
			//Only other door is exit.
			ExitDoor = door;
			ExitDoorScript = door.GetComponent<DoorScript>();
			ExitDoorScript.IsExit = true;
			continue;
		}
	}

	public void SetupMiddleRoomDoors(string entranceDirection,
		RoomScript prevRoomScript)
	{
		//Set entrance/exit doors.
		//ASSUMES EACH MIDDLE ROOM DOOR HAS ONLY 2 DOORS MAX
		for (int i = 0; i < _doors.Count; i++)
		{
			if (_doors[i].tag == entranceDirection)
			{
				EntranceDoor = _doors[i];
				EntranceDoorScript =
					EntranceDoor.GetComponent<DoorScript>();
				EntranceDoorScript.IsEntrance = true;
			}
			else
			{
				ExitDoor = _doors[i];
				ExitDoorScript = ExitDoor.GetComponent<DoorScript>();
				ExitDoorScript.IsExit = true;
			}
		}

		LinkPrevRoom(prevRoomScript);
		return;
	}

	public void SetupBossRoomDoors(string entranceDirection, RoomScript prevRoomScript)
	{
		//Delete extra doors and assign entrance/exit door(s).
		for (int i = 0; i < _doors.Count; i++)
		{
			if (_doors[i].tag != entranceDirection)
			{
				Destroy(_doors[i]);
				_doors.RemoveAt(i);
				i--;
			}
			else
			{
				EntranceDoor = _doors[i];
				EntranceDoorScript = EntranceDoor.GetComponent<DoorScript>();
				EntranceDoorScript.IsEntrance = true;
			}
		}

		LinkPrevRoom(prevRoomScript);
		return;
	}
	private void LinkPrevRoom(RoomScript prevRoomScript)
	{
		prevRoomScript.ExitDoorScript.LinkedDoor = EntranceDoor;
		EntranceDoorScript.LinkedDoor = prevRoomScript.ExitDoorScript.gameObject;
	}
}

//using System.Collections;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Net.Http.Headers;
//using Unity.VisualScripting;
//using UnityEngine;

//public class RoomScript : MonoBehaviour
//{
//    [SerializeField] private List<GameObject> _doors;
//    public bool HasTopDoor;
//    public bool HasBottomDoor;
//    public bool HasLeftDoor;
//    public bool HasRightDoor;

//    public string ExitDoorDirection { get; private set; }

//    public GameObject EntranceDoor { get; private set; }
//    public GameObject ExitDoor { get; private set; }
//    public DoorScript EntranceDoorScript { get; private set; }
//    public DoorScript ExitDoorScript { get; private set; }

//    private void Awake()
//    {
//        if (_doors == null || _doors.Count == 0)
//        {
//            UnityEngine.Debug.LogError("Doors not assigned in " + gameObject.name);
//        }
//    }

//    public void SetupFirstRoomDoor()
//    {
//        if (_doors == null || _doors.Count < 2)
//        {
//            UnityEngine.Debug.LogError("Not enough doors assigned in " + gameObject.name);
//            return;
//        }

//        //Set entrance door
//        int entranceDoorIndex = Random.Range(0, 2);
//        foreach (GameObject door in _doors)
//        {
//            if (door.tag == _doors[entranceDoorIndex].tag)
//            {
//                EntranceDoor = door;
//                continue;
//            }
//            //Only other door is exit.
//            ExitDoor = door;
//            ExitDoorScript = door.GetComponent<DoorScript>();
//            ExitDoorScript.IsExit = true;
//            continue;
//        }
//    }

//    public void SetupMiddleRoomDoors(string entranceDirection,
//    RoomScript prevRoomScript)
//    {
//        //Set entrance/exit doors.
//        //ASSUMES EACH MIDDLE ROOM DOOR HAS ONLY 2 DOORS MAX
//        for (int i = 0; i < _doors.Count; i++)
//        {
//            if (_doors[i].tag == entranceDirection)
//            {
//                EntranceDoor = _doors[i];
//                EntranceDoorScript =
//                EntranceDoor.GetComponent<DoorScript>();
//                EntranceDoorScript.IsEntrance = true;
//            }
//            else
//            {
//                ExitDoor = _doors[i];
//                ExitDoorScript = ExitDoor.GetComponent<DoorScript>();
//                ExitDoorScript.IsExit = true;
//            }
//        }

//        LinkPrevRoom(prevRoomScript);
//        return;
//    }

//    public void SetupBossRoomDoors(string entranceDirection, RoomScript prevRoomScript)
//    {
//        //Delete extra doors and assign entrance/exit door(s).
//        for (int i = 0; i < _doors.Count; i++)
//        {
//            if (_doors[i].tag != entranceDirection)
//            {
//                Destroy(_doors[i]);
//                _doors.RemoveAt(i);
//                i--;
//            }
//            else
//            {
//                EntranceDoor = _doors[i];
//                EntranceDoorScript = EntranceDoor.GetComponent<DoorScript>();
//                EntranceDoorScript.IsEntrance = true;
//            }
//        }

//        LinkPrevRoom(prevRoomScript);
//        return;
//    }
//    private void LinkPrevRoom(RoomScript prevRoomScript)
//    {
//        prevRoomScript.ExitDoorScript.LinkedDoor = EntranceDoor;
//        EntranceDoorScript.LinkedDoor = prevRoomScript.ExitDoorScript.gameObject;
//    }
//}
