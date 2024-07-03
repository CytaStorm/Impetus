using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class LevelManagerScript : MonoBehaviour
{
	public static LevelManagerScript Instance 
	{
		get; private set; 
	}

	#region Room Tagging/Sorting
	[SerializeField] private List<GameObject> _normalRooms;
	[SerializeField] private List<GameObject> _bossRooms;

	//Tagged rooms
	public List<GameObject> _hasTopDoor = new List<GameObject>();
	public List<GameObject> _hasBottomDoor = new List<GameObject>();
	public List<GameObject> _hasLeftDoor = new List<GameObject>();
	public List<GameObject> _hasRightDoor = new List<GameObject>();
	#endregion

	#region Actual Level
	[SerializeField] private int _roomsToMake;
	// current layout
	private LinkedList<RoomScript> _layout = new LinkedList<RoomScript>();

	// reference to room player is currently in
	private LinkedListNode<RoomScript> _currentRoomNode;
	private GameObject _currentRoom { get => _currentRoomNode.Value.gameObject; }
	#endregion

	[HideInInspector] public UnityEvent<int> ChangeRoom;

    private void Awake()
    {
        if (Instance != null &&
			Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
	{
		SortRooms();
		GenerateLayout(_roomsToMake);
		ChangeRoom.AddListener(LoadNewRoom);
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
        #region First Room
        //First room
        //_layout.AddFirst(
        //	Instantiate(
        //		_normalRooms[UnityEngine.Random.Range(0, _normalRooms.Count)],
        //		GetRoomOffset(roomNumber),
        //		Quaternion.identity).GetComponent<RoomScript>());
        _layout.AddFirst(Instantiate(_normalRooms[
			Random.Range(0, _normalRooms.Count)].GetComponent<RoomScript>()));
		_layout.First.Value.SetupFirstRoomDoor();
		_currentRoomNode = _layout.First;
		roomNumber++;
		#endregion

		#region Middle Rooms
		while (roomNumber < roomsToMake)
		{
			string newRoomEntranceDoor =
			GetOppositeDoorDirection(
				_layout.Last.Value.ExitDoorDirection);

			List<GameObject> listToPickFrom;
			switch (newRoomEntranceDoor)
			{
				case "Top Door":
					listToPickFrom = _hasTopDoor;
					break;
				case "Bottom Door":
					listToPickFrom = _hasBottomDoor;
					break;
				case "Left Door":
					listToPickFrom = _hasLeftDoor;
					break;
				case "Right Door":
					listToPickFrom = _hasRightDoor;
					break;
				default:
					throw new InvalidEnumArgumentException(
						"Invalid direction string supplied.");
			}
			//_layout.AddLast(Instantiate(
			//	listToPickFrom[Random.Range(0, listToPickFrom.Count)],
			//	GetRoomOffset(roomNumber),
			//	Quaternion.identity).GetComponent<RoomScript>());
			_layout.AddLast(Instantiate(
				listToPickFrom[Random.Range(
					0, listToPickFrom.Count)].GetComponent<RoomScript>()));
			_layout.Last.Value.SetupMiddleRoomDoors(
				newRoomEntranceDoor, _layout.Last.Previous.Value);
			_layout.Last.Value.gameObject.SetActive(false);
			roomNumber++;
		}
		#endregion

		#region End Room
		//End room (boss)
		//_layout.AddLast(
		//	Instantiate(
		//		_bossRooms[Random.Range(0, _bossRooms.Count)],
		//		GetRoomOffset(roomNumber),
		//		Quaternion.identity).GetComponent<RoomScript>());
		_layout.AddLast(Instantiate(
			_bossRooms[Random.Range(
				0, _bossRooms.Count)].GetComponent<RoomScript>()));
		_layout.Last.Value.SetupBossRoomDoor(
			GetOppositeDoorDirection(_layout.Last.Previous.Value.ExitDoorDirection),
			_layout.Last.Previous.Value);
		_layout.Last.Value.gameObject.SetActive(false);
		#endregion
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
			if (roomScript.HasLeftDoor)
			{
				_hasLeftDoor.Add(room);
			}
			if (roomScript.HasRightDoor)
			{
				_hasRightDoor.Add(room);
			}
			if (roomScript.HasTopDoor)
			{
				_hasTopDoor.Add(room);
			}
			if (roomScript.HasBottomDoor)
			{
				_hasBottomDoor.Add(room);
			}
		}
		return;
	}

	private string GetOppositeDoorDirection(string doorDirection)
	{
		switch (doorDirection)
		{
			case "Top Door":
				return "Bottom Door";
			case "Bottom Door": 
				return "Top Door";
			case "Left Door":
				return "Right Door";
			case "Right Door":  
				return "Left Door";
			default:
				throw new InvalidEnumArgumentException(
					"Invalid Direction string supplied.");
		}
	}

	private Vector3 GetRoomOffset (int roomNumber)
	{
		return new Vector3(20 * roomNumber - 20, 0, 0);
	}

	/// <summary>
	/// Changes the room the player is in.
	/// </summary>
	/// <param name="roomNumber">Number of rooms the player 
	/// changed through.</param>
	private void LoadNewRoom(int roomNumber)
	{
		if (roomNumber > 0)
		{
			_currentRoom.SetActive(false);
			for (int i = 0; i < roomNumber; i++)
			{
				_currentRoomNode = _currentRoomNode.Next;
			}
			_currentRoom.SetActive(true);
		}
		else if (roomNumber < 0) 
		{ 
			_currentRoom.SetActive(false);
			for (int i = 0; i > roomNumber; i--)
			{
				_currentRoomNode = _currentRoomNode.Previous;
			}
			_currentRoom.SetActive(true);
		}
	}
}
