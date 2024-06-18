using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEditor.Progress;

/// <summary>
/// Linked list class for storing dungeon layouts
/// </summary>
public class LevelLinkedList : MonoBehaviour
{
    private LevelNode head;
    private LevelNode tail;
    private int count;

    /// <summary>
    /// Number of nodes in the list
    /// </summary>
    public int Count {  get { return count; } }

    /// <summary>
    /// Create an empty linked list
    /// </summary>
    public LevelLinkedList()
    {
        count = 0;
    }

    /// <summary>
    /// Create a linked list of nodes with no level data
    /// </summary>
    /// <param name="count">Number of nodes in the list</param>
    public LevelLinkedList(int count)
    {
        this.count = 0;
        for (int i = 0; i < count; i++)
        {
            Add(null);
        }
        SetDirections();
    }

    /// <summary>
    /// Remove all data from the list
    /// </summary>
    public void Clear()
    {
        head = null;
        tail = null;
        count = 0;
    }

    /// <summary>
    /// Adds a node to the end of the list
    /// </summary>
    public void Add(GameObject room)
    {
        if (count == 0)
        {
            Debug.Log("adding room to empty list");
            count++;
            head = new LevelNode(null, room);
            tail = head;
        }
        else
        {

            Debug.Log("adding next room to ist");
            count++;
            tail.Next = new LevelNode(tail, room);
            tail = tail.Next;
        }
    }

    /// <summary>
    /// Insert a node at a given index
    /// </summary>
    public void Insert(GameObject room, int index) 
    {
        // Validate index
        if ((index > count) || (index < 0))
        {
            throw new IndexOutOfRangeException(
                $"Error: Cannot insert into invalid index {index}.");
        }

        // Add data to the end
        if (index == count)
        {
            Add(room); // Takes care of empty lists
        }
        // Add data to the start
        else if (index == 0)
        {
            head.Previous = new LevelNode(null, room);
            head.Previous.Next = head;
            head = head.Previous;
            count++;
        }
        // Add data to the middle
        else
        {
            LevelNode current = GetNodeAt(index);
            LevelNode newNode = new LevelNode(current.Previous, room);
            current.Previous.Next = newNode;
            newNode.Next = current;
            current.Previous = newNode;
            count++;
        }
    }

    /// <summary>
    /// Remove a node at the given index
    /// </summary>
    public void RemoveAt(int index) 
    {
        // Validate index
        if ((index >= count) || (index < 0))
        {
            throw new IndexOutOfRangeException(
                $"Error: Cannot remove invalid index {index}.");
        }

        // Remove head
        if (index == 0)
        {

            // Null tail if head was the only node
            if (count == 1)
            {
                head = null;
                tail = null;
            }
            else
            {
                head = head.Next;
                head.Previous = null;
            }
            count--;
        }
        // Remove tail
        else if (index == count - 1)
        {
            tail = tail.Previous;
            tail.Next = null;
            count--;
        }
        // Remove in the middle
        else
        {
            // See what end of the list the index is closest to
            LevelNode current = GetNodeAt(index);
            current.Next.Previous = current.Previous;
            current.Previous.Next = current.Next;
            count--;
        }
    }

    /// <summary>
    /// Set the entrance and exit directions for every node, starting at the head.
    /// </summary>
    private void SetDirections()
    {
        // return early if list is empty
        if (count <= 0)
        {
            return;
        }

        int numUnset = count;

        // set the exit direction of the head node if it exists
        head.EndDir = (Direction)UnityEngine.Random.Range(1, 4);
        LevelNode current = head.Next;
        numUnset--;

        // loop while there are nodes with unset directions in the list
        while (numUnset > 0)
        {
            current.StartDir = GetOppositeDir(current.Previous.EndDir);
            current.EndDir = (Direction)UnityEngine.Random.Range(1, 4);
        }
    }

    /// <summary>
    /// Indexer for the linked list
    /// </summary>
    /// <param name="index">Index of data to access</param>
    /// <returns>Room at specified index</returns>
    public GameObject this[int index]
    {
        get
        {
            // Check if index is invalid in either direction
            if ((index < 0) || (index >= count))
            {
                throw new IndexOutOfRangeException(
                    $"Error: Cannot get data from invalid index {index}.");
            }

            // See what end of the list the index is closest to
            LevelNode current = GetNodeAt(index);
            return current.Room;
        }

        set
        {
            // Check if index is invalid in either direction
            if ((index < 0) || (index >= count))
            {
                throw new IndexOutOfRangeException(
                    $"Error: Cannot set data at invalid index {index}.");
            }

            // See what end of the list the index is closest to
            LevelNode current = GetNodeAt(index);
            current.Room = value;
        }
    }

    /// <summary>
    /// Gets a node using the most efficient traversal possible
    /// </summary>
    /// <param name="index">Index of the node to get</param>
    /// <returns>A reference to the node at the given index</returns>
    public LevelNode GetNodeAt(int index)
    {
        // See what end of the list the index is closest to
        LevelNode current;
        if (count / 2 < index)
        {
            // Loop through list from the end
            current = tail;
            for (int i = count - 1; i > index; i--)
            {
                current = current.Previous;
            }
        }
        else
        {
            // Loop through list from the start
            current = head;
            for (int i = 0; i < index; i++)
            {
                current = current.Next;
            }
        }
        return current;
    }

    /// <summary>
    /// Returns the direction opposite to the one entered
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    private Direction GetOppositeDir(Direction direction)
    {
        switch (direction)
        {
            case Direction.Down:
                return Direction.Up;

            case Direction.Up:
                return Direction.Down;

            case Direction.Left:
                return Direction.Right;

            case Direction.Right:
                return Direction.Left;
        }

        return direction;
    }
}
