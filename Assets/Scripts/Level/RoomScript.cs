using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Unity.VisualScripting;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
	public List<Directions> AllDoorDirections;
	public bool HasDoorTop { get => AllDoorDirections.Contains(Directions.Top); }
	public bool HasDoorBottom { get => AllDoorDirections.Contains(Directions.Bottom); }
	public bool HasDoorLeft { get => AllDoorDirections.Contains(Directions.Left); }
	public bool HasDoorRight { get => AllDoorDirections.Contains(Directions.Right); }

	private Directions _entranceDoorDirection = Directions.None;
	private Directions _exitDoorDirection = Directions.None;
	public Directions EntranceDoorDirection { get => _entranceDoorDirection; }
	public Directions ExitDoorDirection { get => _exitDoorDirection; }

	private GameObject _entranceDoor;
	private GameObject _exitDoor;
	private DoorScript _entranceDoorScript;
	private DoorScript _exitDoorScript;
	public DoorScript EntranceDoorScript { get => _entranceDoorScript; }
	public DoorScript ExitDoorScript { get => _exitDoorScript; }

    public void SetupFirstRoomDoor()
	{
		int entranceDoorIndex = Random.Range(0, 2);
		_exitDoorDirection = AllDoorDirections[entranceDoorIndex];

		//Delete extra doors and assign entrance/exit door(s).
		foreach (Transform door in transform)
		{
			DoorScript doorScript = door.gameObject.GetComponent<DoorScript>();
			if (doorScript.Direction != _exitDoorDirection)
			{
				Destroy(door.gameObject);
			}
			else
			{
				_exitDoorScript = doorScript;
				_exitDoor = doorScript.gameObject;
			}
		}
	}
	public void SetupMiddleRoomDoors(
		Directions entrance, RoomScript prevRoomScript)
	{
		if (AllDoorDirections.IndexOf(entrance) == -1)
		{
			throw new System.Exception("Room does not have this entrance door!");
		}
		_entranceDoorDirection = entrance;
		AllDoorDirections.Remove(entrance);

		_exitDoorDirection = AllDoorDirections[0];

		//Delete extra doors and assign entrance/exit door(s).
		foreach (Transform door in transform)
		{
			DoorScript doorScript = door.gameObject.GetComponent<DoorScript>();
			if (doorScript.Direction == _entranceDoorDirection)
			{
				_entranceDoorScript = doorScript;
				_entranceDoor = doorScript.gameObject;
			}
			else if (doorScript.Direction == _exitDoorDirection)
			{
				_exitDoorScript = doorScript;
				_exitDoor = doorScript.gameObject;
			}
			else
			{
				Destroy(door.gameObject);
			}
		}

		//Link previous room's door to this one, and vice versa.
		//print(prevRoomScript.gameObject.name);
		//print(prevRoomScript.ExitDoorScript.gameObject.name);
		//print(_entranceDoor.name);
		prevRoomScript.ExitDoorScript.LinkedDoor = _entranceDoor;
		EntranceDoorScript.LinkedDoor = prevRoomScript.ExitDoorScript.gameObject;
		//print($"Parent: {gameObject.name}\n" +
		//	$"Entrance Door Name: {EntranceDoorScript.gameObject.name}\n" +
		//	$"Exit Door Name: {ExitDoorScript.gameObject.name}");
		
		return;
	}
	public void SetupBossRoomDoor(
		Directions entrance, RoomScript prevRoomScript)
	{
		_entranceDoorDirection = entrance;
		_exitDoorDirection = Directions.None;

		//Delete extra doors and assign entrance/exit door(s).
		foreach (Transform door in transform)
		{
			DoorScript doorScript = door.gameObject.GetComponent<DoorScript>();
			if (doorScript.Direction != _entranceDoorDirection)
			{
				Destroy(door.gameObject);
			}
			else
			{
				_entranceDoorScript = doorScript;
				_entranceDoor = doorScript.gameObject;
			}
		}

		//Link previous room's door to this one, and vice versa.
		prevRoomScript.ExitDoorScript.LinkedDoor = _entranceDoor;
		EntranceDoorScript.LinkedDoor = prevRoomScript.ExitDoorScript.gameObject;
		return;
	}
}
