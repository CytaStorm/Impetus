using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BigEnemyScript : MonoBehaviour
{
    #region Variables
    // Variable Declarations
    private Path _path;                    // The calculated path for the enemy to follow
    private int _currentWaypoint;          // Index of the current waypoint in the path
    private bool _reachedEndOfPath;        // Indicates if the enemy has reached the end of the path
    [SerializeField] private Seeker _seeker;                // Component responsible for pathfinding
    [SerializeField] private Rigidbody2D _rb;               // Rigidbody component for handling physics
    [SerializeField] private GameObject _target;            // Target the enemy is moving towards

    // Area of Attack (AoE) variables
    [SerializeField] private float _aoeRadius = 2f;  // Radius of the AoE effect
    [SerializeField] private float _aoeDamage = 20f; // Damage dealt by the AoE effect
    [SerializeField] private float _aoeCooldown = 5f;// Cooldown time for the AoE effect
    private float _aoeTimer;       // Timer to track the AoE cooldown

    // Attack range variables
    [SerializeField] private float _attackRange = 3f; // Distance to start the attack
    [SerializeField] private float _minAttackDistance = 0f; // Minimum distance to perform the attack
    #endregion

    #region Constant Variables
    // NextWaypointDistance represents the distance you CAN be from the target waypoint before
    // switching to the next waypoint. This helps curve the path and make it more natural.
    private const float NextWaypointDistance = .5f;
    private float Speed;
    #endregion

    #region Scripts
    // Get the universal enemy script from our enemy
    private EnemyScript _enemyScript;
    #endregion

    #region Enemy Movement Properties
    [SerializeField] private float minSpeed = 0.05f;
    [SerializeField] private float maxSpeed = 10.0f;
    private float enemySpeed;
    private Vector3 targetPosition;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // Define objects
        _target = GameObject.FindWithTag("Player");
        _seeker = this.GetComponent<Seeker>();
        _rb = this.GetComponent<Rigidbody2D>();
        _enemyScript = this.GetComponent<EnemyScript>();

        // SPECIFIC TO BIG ENEMY
        _enemyScript.EnemyHealth = 250;
        _enemyScript.AetherIncrease = 50;
        _enemyScript.EnemyDamage = 10;
        _enemyScript.FlowWorth = 200;

        // Setup CalculatePath() to run every quarter second
        InvokeRepeating("CalculatePath", 0f, .25f);

        // Initialize AoE attack timer
        _aoeTimer = _aoeCooldown;
        Speed = Random.Range(1.5f, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        // First make sure the path is created
        if (_path == null)
        {
            return;
        }

        // MOVE
        // Find direction of the next waypoint (vector2)
        Vector2 direction = (_path.vectorPath[_currentWaypoint] - this.transform.position);

        // Multiply direction by speed and move
        _rb.velocity = direction.normalized * Speed;

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

        // AoE Attack
        _aoeTimer -= Time.deltaTime;
        if (_aoeTimer <= 0f)
        {
            float distanceToPlayer = Vector2.Distance(this.transform.position, _target.transform.position);
            if (distanceToPlayer <= _attackRange && distanceToPlayer >= _minAttackDistance)
            {
                PerformAoEAttack();
                _aoeTimer = _aoeCooldown;
            }
        }

        // Move the enemy towards the target position
        if (targetPosition != null)
        {
            float step = enemySpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
        }
    }

    #region CalculatePath Function
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

    #region Perform Big Enemy Attack
    /// <summary>
    /// Perform an area-of-effect attack.
    /// </summary>
    void PerformAoEAttack()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(this.transform.position, _aoeRadius);

        foreach (var hitCollider in hitColliders)
        {
            // Check if the hit object is a player
            if (!hitCollider.CompareTag("Player"))
            {
                continue; // Skip non-player colliders
            }
            // Apply damage to the player
            PlayerScript player = hitCollider.gameObject.GetComponent<PlayerScript>();
            if (player != null)
            {
                player.Health -= _aoeDamage;
            }
        }
    }
    #endregion

    #region Draw Attack Radius
    /// <summary>
    /// Draw the AoE radius when selected.
    /// </summary>
    void OnDrawGizmosSelected()
    {
        // Draw the AoE radius when selected
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, _aoeRadius);
    }
    #endregion
}