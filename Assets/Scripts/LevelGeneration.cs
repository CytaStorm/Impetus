using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    // all possible prefab rooms
    public List<GameObject> upRightRooms = new List<GameObject>();
    public List<GameObject> upLeftRooms = new List<GameObject>();
    public List<GameObject> downLeftRooms = new List<GameObject>();
    public List<GameObject> downRightRooms = new List<GameObject>();
    public List<GameObject> upDownRooms = new List<GameObject>();
    public List<GameObject> leftRightRooms = new List<GameObject>();
    public List<GameObject> upRooms = new List<GameObject>();
    public List<GameObject> downRooms = new List<GameObject>();
    public List<GameObject> leftRooms = new List<GameObject>();
    public List<GameObject> rightRooms = new List<GameObject>();   

    // current layout
    private LevelLinkedList layout;
    // index of current room the player is in
    public int roomIndex;
    
    void Start()
    {

    }

    void Update()
    {
        
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
                case (Direction.Up, Direction.Right):
                    rng = Random.Range(0, upRightRooms.Count);
                    current.Room = upRightRooms[rng];
                    break;
                case (Direction.Up, Direction.Left):
                    rng = Random.Range(0, upLeftRooms.Count);
                    current.Room = upLeftRooms[rng];
                    break;
                case (Direction.Down, Direction.Left):
                    rng = Random.Range(0, downLeftRooms.Count);
                    current.Room = downLeftRooms[rng];
                    break;
                case (Direction.Down, Direction.Right):
                    rng = Random.Range(0, downRightRooms.Count);
                    current.Room = downRightRooms[rng];
                    break;
                case (Direction.Up, Direction.Down):
                    rng = Random.Range(0, upDownRooms.Count);
                    current.Room = upDownRooms[rng];
                    break;
                case (Direction.Left, Direction.Right):
                    rng = Random.Range(0, leftRightRooms.Count);
                    current.Room = leftRightRooms[rng];
                    break;

                // rooms with one exit (starting rooms)
                case (Direction.None, Direction.Up):
                    rng = Random.Range(0, upRooms.Count);
                    current.Room = upRooms[rng];
                    break;
                case (Direction.None, Direction.Down):
                    rng = Random.Range(0, downRooms.Count);
                    current.Room = downRooms[rng];
                    break;
                case (Direction.None, Direction.Left):
                    rng = Random.Range(0, leftRooms.Count);
                    current.Room = leftRooms[rng];
                    break;
                case (Direction.None, Direction.Right):
                    rng = Random.Range(0, rightRooms.Count);
                    current.Room = rightRooms[rng];
                    break;

                // rooms with one entrance (boss rooms)
                case (Direction.Up, Direction.None):
                    rng = Random.Range(0, upRooms.Count);
                    current.Room = upRooms[rng];
                    break;
                case (Direction.Down, Direction.None):
                    rng = Random.Range(0, downRooms.Count);
                    current.Room = downRooms[rng];
                    break;
                case (Direction.Left, Direction.None):
                    rng = Random.Range(0, leftRooms.Count);
                    current.Room = leftRooms[rng];
                    break;
                case (Direction.Right, Direction.None):
                    rng = Random.Range(0, rightRooms.Count);
                    current.Room = rightRooms[rng];
                    break;

            }
        }
    }

    /// <summary>
    /// Render the room the player is currently in based on the roomIndex
    /// </summary>
    public void RenderCurrentRoom() { }

    public void ResetLayout()
    {
        layout = null;
        roomIndex = 0;
    }
}
