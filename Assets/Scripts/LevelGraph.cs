using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Unweighted, directed graph class for storing dungeon layouts
/// </summary>
public class LevelGraph : MonoBehaviour
{
    private List<LevelGraphVertex> verticies;
    private int[,] adjMatrix;
    private List<List<LevelGraphVertex>> adjList;

    /// <summary>
    /// List of all verticies in the graph
    /// </summary>
    public List<LevelGraphVertex> Vertices
    {
        get { return verticies; }
    }

    /// <summary>
    /// Adjacency list of all the verticies in the graph
    /// </summary>
    public List<List<LevelGraphVertex>> AdjList
    {
        get { return adjList; }
    }

    /// <summary>
    /// Create a new graph given a list of verticies and an adjacency matrix, where the
    /// first vertex in the list is the starting room and the last is the ending room
    /// </summary>
    public LevelGraph(List<LevelGraphVertex> verticies, int[,] adjacencies)
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
        adjList = new List<List<LevelGraphVertex>>();
        for (int i = 0; i < adjMatrix.GetLength(0); i++)
        {
            adjList.Add(new List<LevelGraphVertex>());

            for (int j = 0; j < adjMatrix.GetLength(1); j++)
            {
                adjList[i].Add(verticies[j]);
            }
        }
    }
}
