using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    // all possible room layouts
    public List<GameObject> rooms;
    // current layout
    public LevelLinkedList layout;
    // index of current room the player is in
    public int roomIndex;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary>
    /// Create a new layout linked list with empty rooms
    /// </summary>
    private void GenerateLayout() { }

    /// <summary>
    /// If the rooms in the layout are empty, fill them with the correct preset
    /// </summary>
    private void UpdateRooms() { }

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
