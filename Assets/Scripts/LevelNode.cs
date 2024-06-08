using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Node for doubly-linked list
/// </summary>
public class LevelNode : MonoBehaviour
{
    private GameObject room;
    private Direction startDir;
    private Direction endDir;
    private LevelNode previous;
    private LevelNode next;

    /// <summary>
    /// Returns the room at the current location in the list.
    /// </summary>
    public GameObject Room
    {
        get { return room; }
        set { room = value; }
    }
    /// <summary>
    /// The direction of the entrance door relative to the center of the room.
    /// </summary>
    public Direction StartDir
    {
        get { return startDir; }
        set { startDir = value; }
    }
    /// <summary>
    /// The direction of the exit door relative to the center of the room.
    /// </summary>
    public Direction EndDir
    {
        get { return endDir; }
        set { endDir = value; }
    }
    /// <summary>
    /// Returns the node before this one in the linked list.
    /// If the node is a head this will return null.
    /// </summary>
    public LevelNode Previous
    {
        get { return previous; }
        set { previous = value; }
    }
    /// <summary>
    /// Returns the node in front of this one in the linked list.
    /// If the node is a tail this will return null.
    /// </summary>
    public LevelNode Next
    {
        get { return next; }
        set { next = value; }
    }

    /// <summary>
    /// Create a new node
    /// </summary>
    public LevelNode(LevelNode previous, GameObject room) 
    { 
        this.previous = previous;
        this.room = room;
    }
}

public enum Direction 
{ 
    Down = 0, 
    Up = 1, 
    Left = 2, 
    Right = 3
};
