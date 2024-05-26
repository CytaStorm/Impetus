using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenScript : MonoBehaviour
{
    // holds all possible types of dungeon rooms
    public List<GameObject> rooms = new List<GameObject>();

    // graphs that determine 
    private List<Graph> layouts = new List<Graph>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Creates a new level in the scene based on a randomly-chosen layout graph
    /// </summary>
    private void GenerateLevel()
    {

    }

    /// <summary>
    /// Removes all rooms onscreen
    /// </summary>
    private void DestroyLevel()
    {
        // rooms will have a "room" tag, and this method will find all the objects
        // with that tag and destroy them
    }
}

/// <summary>
/// Unweighted, directed graph class for storing dungeon layouts
/// </summary>
public class Graph
{
    private List<Vertex> verticies;
    private int[,] adjMatrix;
    private List<List<Vertex>> adjList;

    /// <summary>
    /// Create a new graph given a list of verticies and an adjacency matrix, where the
    /// first vertex in the list is the starting room and the last is the ending room
    /// </summary>
    public Graph(List<Vertex> verticies, int[,] adjacencies)
    {
        CreateAdjList();
    }

    private void CreateAdjList()
    {

    }

}

/// <summary>
/// Vertex for custom graph class
/// </summary>
public class Vertex
{
    private RoomType type;
}

/// <summary>
/// Stores what each dungeon room does
/// </summary>
public enum RoomType { Start, End, Normal, Special }
