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
        // initialize all possible layouts and room types
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
        Graph layout = layouts[Random.Range(0, layouts.Count)];
        // generate rooms in path order

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

/// <summary>
/// Unweighted, directed graph class for storing dungeon layouts
/// </summary>
public class Graph
{
    private List<Vertex> verticies;
    private int[,] adjMatrix;
    private List<List<Vertex>> adjList;

    /// <summary>
    /// List of all verticies in the graph
    /// </summary>
    public List<Vertex> Vertices
    {
        get { return verticies; }
    }

    /// <summary>
    /// Adjacency list of all the verticies in the graph
    /// </summary>
    public List<List<Vertex>> AdjList
    {
        get { return adjList; }
    }

    /// <summary>
    /// Create a new graph given a list of verticies and an adjacency matrix, where the
    /// first vertex in the list is the starting room and the last is the ending room
    /// </summary>
    public Graph(List<Vertex> verticies, int[,] adjacencies)
    {
        this.verticies = verticies;
        this.adjMatrix = adjacencies;
        CreateAdjList();
    }

    /// <summary>
    /// Create an adjacency list using a list of verticies and an adjacency matrix
    /// </summary>
    private void CreateAdjList()
    {
        adjList = new List<List<Vertex>>();
        for (int i = 0; i < adjMatrix.GetLength(0); i++)
        {
            adjList.Add(new List<Vertex>());

            for (int j = 0; j < adjMatrix.GetLength(1); j++)
            {
                adjList[i].Add(verticies[j]);
            }
        }
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
