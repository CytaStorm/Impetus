using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores what each dungeon room does
/// </summary>
public enum RoomType { Start, End, Normal, Special }
public class LevelGenScript : MonoBehaviour
{
    // holds all prefab dungeon rooms
    public List<Room> rooms = new List<Room>();

    // graphs that determine the order of rooms
    private List<LevelGraph> layouts = new List<LevelGraph>();

    // Start is called before the first frame update
    void Start()
    {
        // TO DO: initialize all possible layouts and room types
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Creates a new level in the scene based on a randomly-chosen layout graph
    /// </summary>
    public void GenerateLevel()
    {
        // choose a random layout
        LevelGraph layout = layouts[Random.Range(0, layouts.Count)];

        // generate rooms in path order
        foreach(LevelGraphVertex vertex in layout.Vertices)
        {
            Room currentRoom = rooms[Random.Range(0, rooms.Count)];
            // TO DO: set a position vector3 based on the previous room's exit
            Instantiate(currentRoom.Map);
        }

    }

    /// <summary>
    /// Removes all rooms onscreen
    /// </summary>
    public void DestroyLevel()
    {
        // rooms will have a "room" tag, and this method will find all the objects
        // with that tag and destroy them
    }
}





/*
/// <summary>
/// Stores what style the room is (Where the doors are)
/// L = left, T = top, R = right, B = bottom
/// </summary>
public enum RoomStyle { L, T, R, B, LT, LR, LB, TR, TB, RB, LTR, LTB, LRB, TRB, LTRB }
*/
