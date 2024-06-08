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
            switch(current.StartDir, current.EndDir)
            {
                case (Direction.Up, Direction.Down):
                    break;
                // ADD MISSING CASES
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
