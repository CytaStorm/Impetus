using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class RangedEnemyScript : MonoBehaviour
{
    #region Variables
    // Variable Declarations
    private Path _path;                    // The calculated path for the enemy to follow
    private int _currentWaypoint;          // Index of the current waypoint in the path
    private bool _reachedEndOfPath;        // Indicates if the enemy has reached the end of the path
    [SerializeField] private Seeker _seeker; // Component responsible for pathfinding
    [SerializeField] private Rigidbody2D _rb; // Rigidbody component for handling physics
    [SerializeField] private GameObject _target; // Target the enemy is moving towards
    [SerializeField] private LayerMask _obstructionMask; // LayerMask for detecting obstructions
    [SerializeField] private GameObject _projectilePrefab; // Prefab for the projectile
    [SerializeField] private Transform _firePoint; // Point from where the projectile is fired
    #endregion

    #region Scripts
    // Get the universal enemy script from our enemy
    private EnemyScript _enemyScript;
    #endregion

    #region Constant Variables
    // NextWaypointDistance represents the distance you CAN be from the target waypoint before
    // switching to the next waypoint. This helps curve the path and make it more natural.
    private const float NextWaypointDistance = .5f;
    private const float Speed = 5f;
    [SerializeField] private float _detectionRange = 10f;
    [SerializeField] private float _evadeRange = 3f;
    [SerializeField] private float _approachRange = 5f;
    [SerializeField] private float _fireRate = 3f; // Time between shots
    private float _nextFireTime;
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

        // SPECIFIC TO MEDIUM ENEMY
        _enemyScript.EnemyHealth = 300;
        _enemyScript.AetherIncrease = 20;
        _enemyScript.EnemyDamage = 15;
        _enemyScript.FlowWorth = 100;

        // Setup CalculatePath() to run every half second
        InvokeRepeating(nameof(CalculatePath), 0f, .25f);
    }
    #endregion

    #region Update Method
    void Update()
    {
        // First make sure the path is created
        if (_path == null || _reachedEndOfPath) return;

        // Calculate distance to player
        float distanceToPlayer = Vector2.Distance(this.transform.position, _target.transform.position);

        // Check if player is within detection range and there are no obstructions
        if (distanceToPlayer <= _detectionRange &&
            !Physics2D.Linecast(transform.position, _target.transform.position, _obstructionMask))
        {
            if (distanceToPlayer <= _evadeRange)
            {
                // Evade player
                Vector2 direction = (this.transform.position - _target.transform.position).normalized;
                _rb.velocity = direction * Speed;
            }
            else if (distanceToPlayer <= _approachRange)
            {
                // Stop moving when in approach range and attack
                _rb.velocity = Vector2.zero;
                Attack();
            }
            else
            {
                // Move towards player
                MoveTowardsPlayer();
            }
        }
        else
        {
            _rb.velocity = Vector2.zero;
        }
    }
    #endregion

    #region Movement Functions
    /// <summary>
    /// Move towards the player.
    /// </summary>
    void MoveTowardsPlayer()
    {
        // Move towards the next waypoint
        Vector2 direction = (_path.vectorPath[_currentWaypoint] - this.transform.position);
        _rb.velocity = direction.normalized * Speed;

        // If the distance is small enough, switch to the next waypoint
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

    /// <summary>
    /// Calculate a new path (this is done every half second or second or so).
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

    #region Attack Functions
    /// <summary>
    /// Attack the player by firing a projectile.
    /// </summary>
    void Attack()
    {
        if (Time.time >= _nextFireTime)
        {
            // Instantiate projectile and set its direction
            GameObject projectile = Instantiate(_projectilePrefab, _firePoint.position, _firePoint.rotation);
            Rigidbody2D rbProjectile = projectile.GetComponent<Rigidbody2D>();
            Vector2 direction = (_target.transform.position - _firePoint.position).normalized;
            rbProjectile.velocity = direction * 10f;
            Debug.Log("Projectile Direction: " + direction);
            // Update next fire time
            _nextFireTime = Time.time + 1f / _fireRate;
        }
    }
    #endregion

    #region Gizmos
    /// <summary>
    /// Draw the detection and evade radius when selected.
    /// </summary>
    void OnDrawGizmosSelected()
    {
        // Draw the detection range
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, _detectionRange);

        // Draw the approach range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, _approachRange);

        // Draw the evade range
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, _evadeRange);
    }
    #endregion
}
