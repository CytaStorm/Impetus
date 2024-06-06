using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private int width;
    private int height;
    private Vector2 entrance;
    private Vector2 exit;
    private GameObject map;

    /// <summary>
    /// All the gameobjects contained within the room
    /// </summary>
    public GameObject Map
    {
        get { return map; }
    }

    /// <summary>
    /// Create a new dungeon room
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="entrance"></param>
    /// <param name="exit"></param>
    /// <param name="map"></param>
    public Room (int width, int height, Vector2 entrance, Vector2 exit, GameObject map)
    {
        this.width = width;
        this.height = height;
        this.entrance = entrance;
        this.exit = exit;
        this.map = map;
    }
}
