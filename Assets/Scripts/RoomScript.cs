using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Unity.VisualScripting;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
	public List<Directions> AllDoorDirections = new List<Directions>();
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
			if (door.gameObject.GetComponent<DoorScript>().Direction !=
				_exitDoorDirection)
			{
				Destroy(door.gameObject);
			}
			_exitDoor = door.gameObject;
		}
		return;
	}

	/// <summary>
	/// Sets entrance and exit doors based on entrance door.
	/// </summary>
	public void AssignDoors(Directions entrance)
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
			if (door.gameObject.GetComponent<DoorScript>().Direction ==
				_entranceDoorDirection)
			{
				_entranceDoor = door.gameObject;
			}
			else if (door.gameObject.GetComponent<DoorScript>().Direction ==
				_exitDoorDirection)
			{
				_exitDoor = door.gameObject;
			}
			else
			{
				Destroy(door.gameObject);
			}
		}
	}

	public void AssignLastRoomDoor(Directions entrance)
	{
		_entranceDoorDirection = entrance;
		_exitDoorDirection = Directions.None;

		//Delete extra doors and assign entrance/exit door(s).
		foreach (Transform door in transform)
		{
			if (door.gameObject.GetComponent<DoorScript>().Direction !=
				_entranceDoorDirection)
			{
				Destroy(door.gameObject);
			}
			_entranceDoor = door.gameObject;
		}
		return;
	}

	
}
