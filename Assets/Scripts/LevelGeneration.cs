using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public enum Directions
{
	Top = 0,
	Bottom = 1,
	Left = 2,
	Right = 3,
	None = 4
}

public class LevelGeneration : MonoBehaviour
{
	#region Room Tagging/Sorting
	[SerializeField] private List<GameObject> _normalRooms;
	[SerializeField] private List<GameObject> _bossRooms;

	//Tagged rooms
	private List<GameObject> _hasTopDoor = new List<GameObject>();
	private List<GameObject> _hasBottomDoor = new List<GameObject>();
	private List<GameObject> _hasLeftDoor = new List<GameObject>();
	private List<GameObject> _hasRightDoor = new List<GameObject>();
	#endregion

	#region Actual Level
	[SerializeField] private int _roomsToMake;
	// current layout
	private LinkedList<RoomScript> _layout = new LinkedList<RoomScript>();
	// index of current room the player is in
	public int roomIndex;
	// reference to room player is currently in
	private GameObject _currentRoom;
	#endregion

	void Start()
	{
		SortRooms();
		GenerateLayout(_roomsToMake);
	}

	void Update()
	{
	}

	/// <summary>
	/// Create a new layout linked list with non-empty rooms
	/// </summary>
	/// <exception cref="NullReferenceException">Doordirections enum
	/// is not a valid direction.</exception>
	private void GenerateLayout(int roomsToMake) 
	{
		int roomNumber = 1;

		//First room
		_layout.AddFirst(
			Instantiate(
				_normalRooms[UnityEngine.Random.Range(0, _normalRooms.Count)],
				GetRoomOffset(roomNumber),
				Quaternion.identity).GetComponent<RoomScript>());
		_layout.First.Value.AssignFirstRoomDoor();
		roomNumber++;

		//Middle rooms
		while (roomNumber < roomsToMake)
		{
			Directions newRoomEntranceDoor =
			GetOppositeDoorDirection(
				_layout.Last.Value.ExitDoorDirection);

			List<GameObject> listToPickFrom;
			switch (newRoomEntranceDoor)
			{
				case Directions.Top:
					listToPickFrom = _hasTopDoor;
					break;
				case Directions.Bottom:
					listToPickFrom = _hasBottomDoor;
					break;
				case Directions.Left:
					listToPickFrom = _hasLeftDoor;
					break;
				case Directions.Right:
					listToPickFrom = _hasRightDoor;
					break;
				default:
					throw new InvalidEnumArgumentException(
						"DoorDirections Enum is not a valid direction.");
			}
			_layout.AddLast(Instantiate(
				listToPickFrom[UnityEngine.Random.Range(0, listToPickFrom.Count)],
				GetRoomOffset(roomNumber),
				Quaternion.identity).GetComponent<RoomScript>());
			_layout.Last.Value.AssignMiddleRoomDoors(
				newRoomEntranceDoor, _layout.Last.Previous.Value);
			roomNumber++;
		}

		//End room (boss)
		_layout.AddLast(
			Instantiate(
				_bossRooms[UnityEngine.Random.Range(0, _bossRooms.Count)],
				GetRoomOffset(roomNumber),
				Quaternion.identity).GetComponent<RoomScript>());
		_layout.Last.Value.AssignLastRoomDoor(
			GetOppositeDoorDirection(_layout.Last.Previous.Value.ExitDoorDirection),
			_layout.Last.Previous.Value);
		return;
	}

	/// <summary>
	/// Sorts Rooms into their respective lists.
	/// </summary>
	private void SortRooms()
	{
		foreach (GameObject room in _normalRooms)
		{
			RoomScript roomScript = room.GetComponent<RoomScript>();
			//Boss rooms are separated into their own category.
			if (roomScript.HasDoorLeft)
			{
				_hasLeftDoor.Add(room);
			}
			if (roomScript.HasDoorRight)
			{
				_hasRightDoor.Add(room);
			}
			if (roomScript.HasDoorTop)
			{
				_hasTopDoor.Add(room);
			}
			if (roomScript.HasDoorBottom)
			{
				_hasBottomDoor.Add(room);
			}
		}
		return;
	}

	private Directions GetOppositeDoorDirection(Directions doorDirection)
	{
		switch (doorDirection)
		{
			case Directions.Top:
				return Directions.Bottom;
			case Directions.Bottom: 
				return Directions.Top;
			case Directions.Left:
				return Directions.Right;
			case Directions.Right:  
				return Directions.Left;
			default:
				throw new InvalidEnumArgumentException(
					"DoorDirections Enum is not a valid direction.");
		}
	}

	private Vector3 GetRoomOffset (int roomNumber)
	{
		return new Vector3(20 * roomNumber - 20, 0, 0);
	}
}
