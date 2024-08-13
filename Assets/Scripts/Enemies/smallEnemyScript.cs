using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SmallEnemyScript : MonoBehaviour
{
    #region Variables
    // Variable Declarations
    private Path _path;                    // The calculated path for the enemy to follow
    private int _currentWaypoint;          // Index of the current waypoint in the path
    private bool _reachedEndOfPath;        // Indicates if the enemy has reached the end of the path
    [SerializeField] private Seeker _seeker; // Component responsible for pathfinding
    [SerializeField] private Rigidbody2D _rb; // Rigidbody component for handling physics
    [SerializeField] private GameObject _target; // Target the enemy is moving towards
    #endregion

    #region Scripts
    // Get the universal enemy script from our enemy
    private EnemyScript _enemyScript;
    #endregion

    #region Constant Variables
    // NextWaypointDistance represents the distance you CAN be from the target waypoint before
    // switching to the next waypoint. This helps curve the path and make it more natural.
    private const float NextWaypointDistance = .5f;
    #endregion

    #region Variable Speed
    // Variable speed for each small enemy instance
    [SerializeField] private float _minSpeed;
    [SerializeField] private float _maxSpeed;
    private float _speed;
    #endregion

    #region Start Method
    // Start is called before the first frame update
    void Start()
    {
        // Define objects
        _target = GameObject.FindWithTag("Player");
        _seeker = this.GetComponent<Seeker>();
        _rb = this.GetComponent<Rigidbody2D>();
        _enemyScript = this.GetComponent<EnemyScript>();

        // Set random speed between x and y
        _speed = Random.Range(_minSpeed, _maxSpeed);

        // Setup CalculatePath() to run every quarter second
        InvokeRepeating("CalculatePath", 0f, .25f);
    }
    #endregion

    #region Update Method
    // Update is called once per frame
    void Update()
    {
        // First make sure the path is created
        if (_path == null || _reachedEndOfPath)
        {
            return;
        }

        // MOVE
        // Find direction of the next waypoint (vector2)
        Vector2 direction = (_path.vectorPath[_currentWaypoint] - this.transform.position);

        // Multiply direction by speed and move
        _rb.velocity = direction.normalized * _speed;

        // If the distance is small enough, switch to next waypoint
        if (direction.magnitude <= NextWaypointDistance)
        {
            _currentWaypoint++;
        }

        // Are we at the end of the path?
        if (_currentWaypoint >= _path.vectorPath.Count)
        {
            _reachedEndOfPath = true;
        }
    }
    #endregion

    #region CalculatePath Method
    /// <summary>
    /// Calculate a new path (this is done every half second or second or so)
    /// </summary>
    void CalculatePath()
    {
        // First make sure the seeker is done calculating the last path
        if (_seeker.IsDone())
        {
            _path = _seeker.StartPath(this.transform.position, _target.transform.position);
            _reachedEndOfPath = false;
            _currentWaypoint = 0;
        }
    }
    #endregion
}
