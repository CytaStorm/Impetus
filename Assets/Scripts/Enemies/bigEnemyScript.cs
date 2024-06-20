using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bigEnemyScript : MonoBehaviour
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
    const float nextWayPointDistance = .5f;
    const float speed = 3;

    //Define objects
    target = GameObject.FindWithTag("Player");
        seeker = this.GetComponent<Seeker>();
        rb = this.GetComponent<Rigidbody2D>();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
