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


    /// <summary>
    /// Randomly assigns entrance and exit doors of room.
    /// </summary>
    public void AssignFirstRoomDoor()
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

			_exitDoor = doorScript.gameObject;
			_exitDoorScript = doorScript;
		}
		return;
	}

	/// <summary>
	/// Sets entrance and exit doors based on entrance door.
	/// </summary>
	public void AssignMiddleRoomDoors(Directions entrance, RoomScript prevRoomScript)
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
				_entranceDoor = doorScript.gameObject;
				_entranceDoorScript = doorScript;
			}
			else if (doorScript.Direction == _exitDoorDirection)
			{
				_exitDoor = doorScript.gameObject;
				_exitDoorScript = doorScript;
			}
			else
			{
				Destroy(door.gameObject);
			}
		}

		//Link previous room's door to this one, and vice versa.
		prevRoomScript.ExitDoorScript.LinkedDoor = _entranceDoor;
		EntranceDoorScript.LinkedDoor = prevRoomScript.ExitDoorScript.gameObject;
		
		return;
	}

	public void AssignLastRoomDoor(Directions entrance, RoomScript prevRoomScript)
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

			_entranceDoor = doorScript.gameObject;
			_entranceDoorScript = doorScript;
		}

		//Link previous room's door to this one, and vice versa.
		prevRoomScript.ExitDoorScript.LinkedDoor = _entranceDoor;
		EntranceDoorScript.LinkedDoor = prevRoomScript.ExitDoorScript.gameObject;
		return;
	}
}
