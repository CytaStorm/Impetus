using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy2Script : MonoBehaviour
{
    // Variable Declarations
    Path path;
    int currentWaypoint;
    bool reachedEndOfPath;
    Seeker seeker;
    Rigidbody2D rb;
    GameObject target;
    public LayerMask obstructionMask;

    // Get the universal enemy script from our enemy
    EnemyScript enemyScript;

    // nextWaypointDistance represents the distance you CAN be from the target waypoint before
    // switching to the next waypoint. This helps curve the path and make it more natural.
    const float nextWayPointDistance = .5f;
    const float speed = 5f;
    public float detectionRange = 10f;
    public float evadeRange = 3f;
    public float approachRange = 5f;

    // Attack variables
    public GameObject ProjectilePrefab;
    public Transform firePoint;
    public float fireRate = 3f; // Time between shots
    private float nextFireTime;


    // Weakness variable
    //Depending on if they use enums for weakness or not
    //If they dont use enums we need to change the weakness logic
    //public AttackType Weakness = AttackType.Piercing;

    // Start is called before the first frame update
    void Start()
    {
        // Define objects
        target = GameObject.FindWithTag("Player");
        seeker = this.GetComponent<Seeker>();
        rb = this.GetComponent<Rigidbody2D>();
        enemyScript = this.GetComponent<EnemyScript>();

        // SPECIFIC TO MEDIUM ENEMY
        enemyScript.EnemyHealth = 300;
        enemyScript.AetherIncrease = 20;
        enemyScript.EnemyDamage = 15;
        enemyScript.FlowWorth = 100;

        // Setup CalculatePath() to run every half second
        InvokeRepeating("CalculatePath", 0f, .25f);
    }

    void Update()
    {
        // First make sure the path is created
        if (path == null || reachedEndOfPath)
        {
            return;
        }

        // Calculate distance to player
        float distanceToPlayer = Vector2.Distance(this.transform.position, target.transform.position);

        // Check if player is within detection range and there are no obstructions
        if (distanceToPlayer <= detectionRange && !Physics2D.Linecast(transform.position, target.transform.position, obstructionMask))
        {
            if (distanceToPlayer <= evadeRange)
            {
                // Evade player
                Vector2 direction = (this.transform.position - target.transform.position).normalized;
                rb.velocity = direction * speed;
            }
            else if (distanceToPlayer <= approachRange)
            {
                // Stop moving when in approach range and attack
                rb.velocity = Vector2.zero;
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
            rb.velocity = Vector2.zero;
        }
    }

    void MoveTowardsPlayer()
    {
        // Move towards the next waypoint
        Vector2 direction = (path.vectorPath[currentWaypoint] - this.transform.position);
        rb.velocity = direction.normalized * speed;

        // If the distance is small enough, switch to the next waypoint
        if (direction.magnitude <= nextWayPointDistance)
        {
            currentWaypoint++;
        }

        // Are we at the end of the path?
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
        }
    }


    void Attack()
    {
        if (Time.time >= nextFireTime)
        {
            // Instantiate projectile and set its direction
            GameObject projectile = Instantiate(ProjectilePrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rbProjectile = projectile.GetComponent<Rigidbody2D>();
            Vector2 direction = (target.transform.position - firePoint.position).normalized;
            rbProjectile.velocity = direction * 10f;
            Debug.Log("Projectile Direction: " + direction);
            // Update next fire time
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    /// <summary>
    /// Calculate a new path (this is done every half second or second or so)
    /// </summary>
    void CalculatePath()
    {
        // First make sure the seeker is done calculating the last path
        if (seeker.IsDone())
        {
            path = seeker.StartPath(this.transform.position, target.transform.position);
            reachedEndOfPath = false;
            currentWaypoint = 0;
        }
    }

    /// <summary>
    /// Draw the detection and evade radius when selected
    /// </summary>
    void OnDrawGizmosSelected()
    {
        // Draw the detection range
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, detectionRange);

        // Draw the approach range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, approachRange);

        // Draw the evade range
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, evadeRange);
    }
}
