using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum DoorDirections
{
	Top = 0,
	Bottom = 1,
	Left = 2,
	Right = 3,
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
	private LinkedList<GameObject> _layout = new LinkedList<GameObject>();
	// index of current room the player is in
	public int roomIndex;
	// reference to room player is currently in
	private GameObject _currentRoom;
	#endregion

	// Called whenever the attached object is created or enabled
	private void OnEnable()
	{
		DoorEventManager.OnDoorEnter += IncrementRoom;
		DoorEventManager.OnDoorExit += DecrementRoom;
	}

	private void OnDisable()
	{
		// Remove methods from events to prevent future errors
		DoorEventManager.OnDoorEnter -= IncrementRoom;
		DoorEventManager.OnDoorExit -= DecrementRoom;
	}

	void Start()
	{
		SortRooms();
		GenerateLayout(_roomsToMake);
		//RenderCurrentRoom();
	}

	void Update()
	{
		// FOR DEBUGGING ONLY
		if (Input.GetKeyDown(KeyCode.Space))
		{
			IncrementRoom();
		}
	}

	/// <summary>
	/// Create a new layout linked list with non-empty rooms
	/// </summary>
	private void GenerateLayout(int roomsToMake) 
	{
		int roomNumber = 1;
		//First room
		_layout.AddFirst(
			Instantiate(
				_normalRooms[Random.Range(0, _normalRooms.Count)],
				new Vector3(10 * roomNumber - 10, 0, 0),
				Quaternion.identity));
		_layout.First.Value.GetComponent<RoomScript>().AssignDoors();
		roomNumber++;

		//Middle rooms
		while (roomNumber < roomsToMake)
		{
			GenerateMiddleRoom(roomNumber);
			roomNumber++;
		}

		//End room (boss)
		_layout.AddLast(
			Instantiate(
				_bossRooms[Random.Range(0, _bossRooms.Count)],
				new Vector3(10 * roomNumber - 10, 0, 0),
				Quaternion.identity));
		_layout.Last.Value.GetComponent<RoomScript>().AssignDoors();

		foreach (GameObject gameObject in _layout)
		{
			print(gameObject);
		}
	}

	/// <summary>
	/// Generates rooms inbetween first and last rooms.
	/// </summary>
	/// <exception cref="System.Exception">
	/// DoorDirections Enum somehow has more than 4 values.</exception>
	private void GenerateMiddleRoom(int roomNumber)
	{
		DoorDirections newRoomEntranceDoor =
			GetOppositeDoorDirection(
				_layout.Last.Value.GetComponent<RoomScript>().ExitDoor);

		List<GameObject> listToPickFrom;
		switch (newRoomEntranceDoor)
		{
			case DoorDirections.Top:
				listToPickFrom = _hasTopDoor;
				break;
			case DoorDirections.Bottom:
				listToPickFrom = _hasBottomDoor;
				break;
			case DoorDirections.Left:
				listToPickFrom = _hasLeftDoor;
				break;
			case DoorDirections.Right:
				listToPickFrom = _hasRightDoor;
				break;
			default:
				throw new System.Exception(
					"DoorDirections Enum somehow has more than 4 values??!!");
		}
		_layout.AddLast(
			Instantiate(
				listToPickFrom[Random.Range(0, listToPickFrom.Count)],
				new Vector3(10 * roomNumber - 10, 0, 0),
				Quaternion.identity));
		_layout.Last.Value.GetComponent<RoomScript>().AssignDoors();
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
	}

	private DoorDirections GetOppositeDoorDirection(
		DoorDirections doorDirection)
	{
		switch (doorDirection)
		{
			case DoorDirections.Top:
				return DoorDirections.Bottom;
			case DoorDirections.Bottom: 
				return DoorDirections.Top;
			case DoorDirections.Left:
				return DoorDirections.Right;
			case DoorDirections.Right:  
				return DoorDirections.Left;
			default:
				throw new System.Exception("DoorDirections Enum somehow has"  +
					"more than 4 values??!!");
		}
	}
	///// <summary>
	///// Render the room the player is currently in based on the roomIndex
	///// </summary>
	//public void RenderCurrentRoom() 
	//{
	//    _currentRoom = GameObject.Instantiate(_layout[roomIndex]);
	//}

	/// <summary>
	/// Destroy the room the player is currently in based on the roomIndex
	/// </summary>
	public void DestroyCurrentRoom()
	{
		GameObject.Destroy(_currentRoom);
	}

	/// <summary>
	/// Replaces current room with the next one in the linked list
	/// </summary>
	public void IncrementRoom()
	{
		DestroyCurrentRoom();
		roomIndex++;
		//RenderCurrentRoom();
	}

	/// <summary>
	/// Replaces the current room with the previous one in the linked list
	/// </summary>
	public void DecrementRoom()
	{
		DestroyCurrentRoom();
		roomIndex++;
		//RenderCurrentRoom();
	}

	public void ResetLayout()
	{
		_layout = null;
		roomIndex = 0;
	}
}
