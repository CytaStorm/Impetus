using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class smallEnemyScript : MonoBehaviour
{
    //Variable Declarations
    Path path;
    int currentWaypoint;
    bool reachedEndOfPath;
    Seeker seeker;
    Rigidbody2D rb;
    GameObject target;

    //nextWaypointDistance represents the distance you CAN be from the target waypoint before
    // switching to the next waypoint. This helps curve the path and make it more natural.
    const float nextWaypointDistance = 1;
    const float speed = 3;

    // Start is called before the first frame update
    void Start()
    {
        //Define objects
        target = GameObject.FindWithTag("Player");
        seeker = this.GetComponent<Seeker>();
        rb = this.GetComponent<Rigidbody2D>();

        //Setup CalculatePath() to run every half second
        InvokeRepeating("CalculatePath", 0f, .5f);
    }

    // Update is called once per frame
    void Update()
    {
        //First make sure the path is created
        if (path == null)
        {
            Debug.Log("yo shit null cuh");
            return;
        }
            

        //MOVE
        //find direction of the next waypoint (vector2)
        Debug.Log(path.vectorPath[currentWaypoint].magnitude);
        Vector2 direction = (path.vectorPath[currentWaypoint] - this.transform.position);

        //Multiply direction by speed and move
        rb.velocity = direction.normalized * speed;

        //If the distance is small enough, switch to next waypoint
        if (direction.magnitude < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        //Are we at the end of the path?
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
        }
    }

    /// <summary>
    /// Calculate a new path (this is done every half second or second or so)
    /// </summary>
    void CalculatePath()
    {
        //First make sure the seeker is done calculating the last path
        if (seeker.IsDone())
        {
            path = seeker.StartPath(this.transform.position, target.transform.position);
            reachedEndOfPath = false;
            currentWaypoint = 0;
        }
    }
}
