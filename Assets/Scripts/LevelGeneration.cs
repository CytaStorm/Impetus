using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    // all possible prefab rooms
    // the first word is the entrance direction and the second is the exit direction
    public List<GameObject> downUpRooms = new List<GameObject>();
    public List<GameObject> downLeftRooms = new List<GameObject>();
    public List<GameObject> downRightRooms = new List<GameObject>();
    public List<GameObject> downNullRooms = new List<GameObject>();

    public List<GameObject> upDownRooms = new List<GameObject>();
    public List<GameObject> upLeftRooms = new List<GameObject>();
    public List<GameObject> upRightRooms = new List<GameObject>();
    public List<GameObject> upNullRooms = new List<GameObject>();

    public List<GameObject> leftDownRooms = new List<GameObject>();
    public List<GameObject> leftUpRooms = new List<GameObject>();
    public List<GameObject> leftRightRooms = new List<GameObject>();
    public List<GameObject> leftNullRooms = new List<GameObject>();

    public List<GameObject> rightDownRooms = new List<GameObject>();
    public List<GameObject> rightUpRooms = new List<GameObject>();
    public List<GameObject> rightLeftRooms = new List<GameObject>();
    public List<GameObject> rightNullRooms = new List<GameObject>();

    public List<GameObject> nullDownRooms = new List<GameObject>();
    public List<GameObject> nullUpRooms = new List<GameObject>();
    public List<GameObject> nullLeftRooms = new List<GameObject>();
    public List<GameObject> nullRightRooms = new List<GameObject>();

    // current layout
    private LevelLinkedList layout;
    // index of current room the player is in
    public int roomIndex;
    // reference to room player is currently in
    private GameObject currentRoom;

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
        layout = new LevelLinkedList();
        roomIndex = 0;
        GenerateLayout(5);
        RenderCurrentRoom();
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
    private void GenerateLayout(int totalRooms) 
    {
        layout = new LevelLinkedList(totalRooms);
        UpdateRooms();
    }

    /// <summary>
    /// Assign the room value of every node to the correct preset
    /// </summary>
    private void UpdateRooms() 
    {
        LevelNode current;
        for (int i = 0; i < layout.Count; i++)
        {
            // check the node's enrance and exit directions to assign the correct
            // room prefab
            current = layout.GetNodeAt(i);
            int rng;
            switch(current.StartDir, current.EndDir)
            {
                // rooms with an entrance and an exit (hallways)
                case (Direction.Up, Direction.Down):
                    rng = Random.Range(0, upDownRooms.Count);
                    current.Room = upDownRooms[rng];
                    break;
                case (Direction.Up, Direction.Left):
                    rng = Random.Range(0, upLeftRooms.Count);
                    current.Room = upLeftRooms[rng];
                    break;
                case (Direction.Up, Direction.Right):
                    rng = Random.Range(0, upRightRooms.Count);
                    current.Room = upRightRooms[rng];
                    break;

                case (Direction.Down, Direction.Up):
                    rng = Random.Range(0, downUpRooms.Count);
                    current.Room = downUpRooms[rng];
                    break;
                case (Direction.Down, Direction.Left):
                    rng = Random.Range(0, downLeftRooms.Count);
                    current.Room = downLeftRooms[rng];
                    break;
                case (Direction.Down, Direction.Right):
                    rng = Random.Range(0, downRightRooms.Count);
                    current.Room = downRightRooms[rng];
                    break;

                case (Direction.Left, Direction.Down):
                    rng = Random.Range(0, leftDownRooms.Count);
                    current.Room = leftDownRooms[rng];
                    break;
                case (Direction.Left, Direction.Up):
                    rng = Random.Range(0, leftUpRooms.Count);
                    current.Room = leftUpRooms[rng];
                    break;
                case (Direction.Left, Direction.Right):
                    rng = Random.Range(0, leftRightRooms.Count);
                    current.Room = leftRightRooms[rng];
                    break;

                case (Direction.Right, Direction.Down):
                    rng = Random.Range(0, rightDownRooms.Count);
                    current.Room = rightDownRooms[rng];
                    break;
                case (Direction.Right, Direction.Up):
                    rng = Random.Range(0, rightUpRooms.Count);
                    current.Room = rightUpRooms[rng];
                    break;
                case (Direction.Right, Direction.Left):
                    rng = Random.Range(0, rightLeftRooms.Count);
                    current.Room = rightLeftRooms[rng];
                    break;

                // rooms with one exit (starting rooms)
                case (Direction.None, Direction.Up):
                    rng = Random.Range(0, nullUpRooms.Count);
                    current.Room = nullUpRooms[rng];
                    break;
                case (Direction.None, Direction.Down):
                    rng = Random.Range(0, nullDownRooms.Count);
                    current.Room = nullDownRooms[rng];
                    break;
                case (Direction.None, Direction.Left):
                    rng = Random.Range(0, nullLeftRooms.Count);
                    current.Room = nullLeftRooms[rng];
                    break;
                case (Direction.None, Direction.Right):
                    rng = Random.Range(0, nullRightRooms.Count);
                    current.Room = nullRightRooms[rng];
                    break;

                // rooms with one entrance (boss rooms)
                case (Direction.Up, Direction.None):
                    rng = Random.Range(0, upNullRooms.Count);
                    current.Room = upNullRooms[rng];
                    break;
                case (Direction.Down, Direction.None):
                    rng = Random.Range(0, downNullRooms.Count);
                    current.Room = downNullRooms[rng];
                    break;
                case (Direction.Left, Direction.None):
                    rng = Random.Range(0, leftNullRooms.Count);
                    current.Room = leftNullRooms[rng];
                    break;
                case (Direction.Right, Direction.None):
                    rng = Random.Range(0, rightNullRooms.Count);
                    current.Room = rightNullRooms[rng];
                    break;
                
                // default in case a door combo wasn't accounted for
                default:
                    rng = Random.Range(0, upRightRooms.Count);
                    current.Room = upRightRooms[rng];
                    break;
            }
        }
    }

    /// <summary>
    /// Render the room the player is currently in based on the roomIndex
    /// </summary>
    public void RenderCurrentRoom() 
    {
        currentRoom = GameObject.Instantiate(layout[roomIndex]);
    }

    /// <summary>
    /// Destroy the room the player is currently in based on the roomIndex
    /// </summary>
    public void DestroyCurrentRoom()
    {
        GameObject.Destroy(currentRoom);
    }

    /// <summary>
    /// Replaces current room with the next one in the linked list
    /// </summary>
    public void IncrementRoom()
    {
        DestroyCurrentRoom();
        roomIndex++;
        RenderCurrentRoom();
    }

    /// <summary>
    /// Replaces the current room with the previous one in the linked list
    /// </summary>
    public void DecrementRoom()
    {
        DestroyCurrentRoom();
        roomIndex++;
        RenderCurrentRoom();
    }

    public void ResetLayout()
    {
        layout = null;
        roomIndex = 0;
    }
}
