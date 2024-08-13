using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class LevelManagerScript : MonoBehaviour
{
	public static LevelManagerScript LevelManager 
	{
		get; private set; 
	}

	#region Room Tagging/Sorting
	[SerializeField] private List<GameObject> _normalRooms;
	[SerializeField] private List<GameObject> _bossRooms;

	//Tagged rooms
	private List<GameObject> _hasTopDoor = new List<GameObject>();
	private List<GameObject> _hasBottomDoor = new List<GameObject>();
	private List<GameObject> _hasLeftDoor = new List<GameObject>();
	private List<GameObject> _hasRightDoor = new List<GameObject>();
	private List<GameObject> _bossRoomHasTopDoor = new List<GameObject>();
	private List<GameObject> _bossRoomHasBottomDoor = new List<GameObject>();
	private List<GameObject> _bossRoomHasLeftDoor = new List<GameObject>();
	private List<GameObject> _bossRoomHasRightDoor = new List<GameObject>();
	#endregion

	#region Actual Level
	// current layout
	private LinkedList<RoomScript> _layout = new LinkedList<RoomScript>();
	[SerializeField] private int _roomsToMake;
	public int RoomsToMake
	{
		get => _roomsToMake;
	}

	// references to room player is currently in
	private LinkedListNode<RoomScript> _currentRoomNode;
	private GameObject _currentRoom { get => _currentRoomNode.Value.gameObject; }
	//Number of room player is in.
	[HideInInspector] public int RoomNumber = 1;
	#endregion

	[HideInInspector] public UnityEvent<int> ChangeRoom;

	private void Awake()
	{
		if (LevelManager != null &&
			LevelManager != this)
		{
			Destroy(gameObject);
		}
		else LevelManager = this;
	}

	void Start()
	{
		SortRooms();
		GenerateLayout(RoomsToMake);
		ChangeRoom.AddListener(LoadNewRoom);
	}

	void Update()
	{
	}

 //   private void SortRooms()
	//{
	//	foreach (GameObject room in _normalRooms)
	//	{
	//		RoomScript roomScript = room.GetComponent<RoomScript>();
	//		//Boss rooms are separated into their own category.
	//		if (roomScript.HasLeftDoor)
	//		{
	//			_hasLeftDoor.Add(room);
	//		}
	//		if (roomScript.HasRightDoor)
	//		{
	//			_hasRightDoor.Add(room);
	//		}
	//		if (roomScript.HasTopDoor)
	//		{
	//			_hasTopDoor.Add(room);
	//		}
	//		if (roomScript.HasBottomDoor)
	//		{
	//			_hasBottomDoor.Add(room);
	//		}
	//	}

	//	//Sort boss rooms
	//	foreach (GameObject room in _bossRooms)
	//	{
	//		RoomScript roomScript = room.GetComponent<RoomScript>();
	//		if (roomScript.HasLeftDoor)
	//		{
	//			_bossRoomHasLeftDoor.Add(room);
	//		}
	//		if (roomScript.HasRightDoor)
	//		{
	//			_bossRoomHasRightDoor.Add(room);
	//		}
	//		if (roomScript.HasTopDoor)
	//		{
	//			_bossRoomHasTopDoor.Add(room);
	//		}
	//		if (roomScript.HasBottomDoor)
	//		{
	//			_bossRoomHasBottomDoor.Add(room);
	//		}
	//	}
	//	return;
	//}

    //private void SortRooms()
    //{
    //    if (_normalRooms == null)
    //    {
    //        Debug.LogError("Normal rooms list is not initialized.");
    //        return;
    //    }

    //    foreach (GameObject room in _normalRooms)
    //    {
    //        if (room == null)
    //        {
    //            Debug.LogWarning("A room in the normal rooms list is null.");
    //            continue;
    //        }

    //        RoomScript roomScript = room.GetComponent<RoomScript>();
    //        if (roomScript == null)
    //        {
    //            Debug.LogWarning($"Room {room.name} does not have a RoomScript component.");
    //            continue;
    //        }

    //        if (roomScript.HasLeftDoor)
    //        {
    //            _hasLeftDoor.Add(room);
    //        }
    //        if (roomScript.HasRightDoor)
    //        {
    //            _hasRightDoor.Add(room);
    //        }
    //        if (roomScript.HasTopDoor)
    //        {
    //            _hasTopDoor.Add(room);
    //        }
    //        if (roomScript.HasBottomDoor)
    //        {
    //            _hasBottomDoor.Add(room);
    //        }
    //    }
    //}

    private void SortRooms()
    {
        if (_normalRooms == null)
        {
            Debug.LogError("Normal rooms list is not initialized.");
            return;
        }

        foreach (GameObject room in _normalRooms)
        {
            if (room == null)
            {
                Debug.LogWarning("A room in the normal rooms list is null.");
                continue;
            }

            RoomScript roomScript = room.GetComponent<RoomScript>();
            if (roomScript == null)
            {
                Debug.LogWarning($"Room {room.name} does not have a RoomScript component. Adding it now.");
                roomScript = room.AddComponent<RoomScript>();
            }

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
    }



    private void GenerateLayout(int roomsToMake)
	{
		GenerateFirstRoom();
		GenerateMiddleRooms(roomsToMake - 2);
		GenerateBossRoom();
		SetPlayerSpawn();
	}
	private void GenerateFirstRoom()
	{
		_layout.AddFirst(Instantiate(_normalRooms[Random.Range(
			0, _normalRooms.Count)], transform).GetComponent<RoomScript>());
		_layout.First.Value.SetupFirstRoomDoor();
		_currentRoomNode = _layout.First;
	}
	private void GenerateMiddleRooms(int roomsToMake)
	{
		for (int i = 0; i < roomsToMake; i++)
		{
			string newRoomEntranceDoorDirection =
			GetOppositeDoorDirection(
				_layout.Last.Value.ExitDoor.tag);

			List<GameObject> listToPickFrom;
			switch (newRoomEntranceDoorDirection)
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
					0, listToPickFrom.Count)], transform).GetComponent<RoomScript>());
			_layout.Last.Value.SetupMiddleRoomDoors(
				newRoomEntranceDoorDirection,
				_layout.Last.Previous.Value);
			_layout.Last.Value.gameObject.SetActive(false);
		}
	}
	private void GenerateBossRoom()
	{
		string newRoomEntranceDoor = GetOppositeDoorDirection(
					_layout.Last.Value.ExitDoor.tag);
		List<GameObject> listToPickFrom;
		switch (newRoomEntranceDoor)
		{
			case "Top Door":
				listToPickFrom = _bossRoomHasTopDoor;
				break;
			case "Bottom Door":
				listToPickFrom = _bossRoomHasBottomDoor;
				break;
			case "Left Door":
				listToPickFrom = _bossRoomHasLeftDoor;
				break;
			case "Right Door":
				listToPickFrom = _bossRoomHasRightDoor;
				break;
			default:
				throw new InvalidEnumArgumentException(
					"Invalid direction string supplied.");
		}
		_layout.AddLast(Instantiate(
			listToPickFrom[Random.Range(
				0, listToPickFrom.Count)], transform).GetComponent<RoomScript>());
		_layout.Last.Value.SetupBossRoomDoors(
			GetOppositeDoorDirection(_layout.Last.Previous.Value.ExitDoor.tag),
			_layout.Last.Previous.Value);
		_layout.Last.Value.gameObject.SetActive(false);
	}
	private void SetPlayerSpawn()
	{
		Vector3 DirectionToSendPlayer;
		float nextDoorOffset = 1.25f;
		switch (_layout.First.Value.EntranceDoor.tag)
		{
		    case "Top Door":
		        DirectionToSendPlayer = Vector3.down;
		        break;
		    case "Bottom Door":
		        DirectionToSendPlayer = Vector3.up;
		        break;
		    case "Left Door":
		        DirectionToSendPlayer = Vector3.right;
		        break;
		    case "Right Door":
		        DirectionToSendPlayer = Vector3.left;
		        break;
		    default:
		        throw new UnityException("Untagged Door!");
		}
		PlayerScript.Player.transform.position = 
			_layout.First.Value.EntranceDoor.transform.position + 
			DirectionToSendPlayer * nextDoorOffset;
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
				RoomNumber++;
			}
			_currentRoom.SetActive(true);
		}
		else if (roomNumber < 0) 
		{ 
			_currentRoom.SetActive(false);
			for (int i = 0; i > roomNumber; i--)
			{
				_currentRoomNode = _currentRoomNode.Previous;
				RoomNumber--;
			}
			_currentRoom.SetActive(true);
		}
	}
}

