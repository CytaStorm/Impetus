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

	public string EntranceDoorDirection { get; private set; }
	public string ExitDoorDirection { get; private set; }

	public GameObject EntranceDoor { get; private set; }
	public GameObject ExitDoor { get; private set; }
	public DoorScript EntranceDoorScript { get; private set; }
	public DoorScript ExitDoorScript { get; private set; }

	public void SetupFirstRoomDoor()
	{
		int entranceDoorIndex = Random.Range(0, 2);
		ExitDoorDirection = _doors[entranceDoorIndex].tag;

		//Delete extra doors and assign entrance/exit door(s).
		for (int i = 0; i < _doors.Count; i++)
		{
			if (_doors[i].tag != ExitDoorDirection)
			{
				Destroy(_doors[i]);
				_doors.RemoveAt(i);
				i--;
			}
			else
			{
				ExitDoor = _doors[i];
				ExitDoorScript = _doors[i].GetComponent<DoorScript>();
				ExitDoorScript.IsExit = true;
			}
		}
		return;
	}

	public void SetupMiddleRoomDoors(string entrance, RoomScript prevRoomScript)
	{
		//Set entrance/exit doors.
		//ASSUMES EACH MIDDLE ROOM DOOR HAS ONLY 2 DOORS MAX
		EntranceDoorDirection = entrance;
		for (int i = 0; i < _doors.Count; i++)
		{
			if (_doors[i].tag == EntranceDoorDirection)
			{
				EntranceDoor = _doors[i];
				EntranceDoorScript = EntranceDoor.GetComponent<DoorScript>();
				EntranceDoorScript.IsEntrance = true;
			}
			else
			{
				ExitDoor = _doors[i];
				ExitDoorScript = ExitDoor.GetComponent<DoorScript>();
				ExitDoorScript.IsExit = true;
				ExitDoorDirection = ExitDoor.tag;
			}
		}

		LinkPrevRoom(prevRoomScript);
		return;
	}

	public void SetupBossRoomDoor(string entrance, RoomScript prevRoomScript)
	{
		EntranceDoorDirection = entrance;

		//Delete extra doors and assign entrance/exit door(s).
		for (int i = 0; i < _doors.Count; i++)
		{
			if (_doors[i].tag != EntranceDoorDirection)
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
