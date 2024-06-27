using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class bigEnemyScript : MonoBehaviour
{
    // Variable Declarations
    Path path;
    int currentWaypoint;
    bool reachedEndOfPath;
    Seeker seeker;
    Rigidbody2D rb;
    GameObject target;

    // Get the universal enemy script from our enemy
    EnemyScript enemyScript;

    // nextWaypointDistance represents the distance you CAN be from the target waypoint before
    // switching to the next waypoint. This helps curve the path and make it more natural.
    const float nextWayPointDistance = .5f;
    const float speed = 1.5f;

    // Area of Attack variables (AoE)
    public float aoeRadius = 2f;
    public float aoeDamage = 20f;
    public float aoeCooldown = 5f;
    private float aoeTimer;

    // Attack range variables
    // Distance to start the attack
    public float attackRange = 3f;
    // Minimum distance to perform the attack
    public float minAttackDistance = 1f;

    // Start is called before the first frame update
    void Start()
    {
        // Define objects
        target = GameObject.FindWithTag("Player");
        seeker = this.GetComponent<Seeker>();
        rb = this.GetComponent<Rigidbody2D>();
        enemyScript = this.GetComponent<EnemyScript>();

        // SPECIFIC TO BIG ENEMY
        enemyScript.EnemyHealth = 500;
        enemyScript.AetherIncrease = 50;
        enemyScript.EnemyDamage = 10;
        enemyScript.FlowWorth = 200;

        // Setup CalculatePath() to run every quarter second
        InvokeRepeating("CalculatePath", 0f, .25f);

        // Initialize AoE attack timer
        aoeTimer = aoeCooldown;
    }




    // Update is called once per frame
    void Update()
    {
        // First make sure the path is created
        if (path == null)
        {
            return;
        }

        // MOVE
        // Find direction of the next waypoint (vector2)
        Vector2 direction = (path.vectorPath[currentWaypoint] - this.transform.position);

        // Multiply direction by speed and move
        rb.velocity = direction.normalized * speed;

        // If the distance is small enough, switch to next waypoint
        if (direction.magnitude <= nextWayPointDistance)
        {
            currentWaypoint++;
        }

        // Are we at the end of the path?
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
        }

        // AoE Attack
        aoeTimer -= Time.deltaTime;
        if (aoeTimer <= 0f)
        {
            float distanceToPlayer = Vector2.Distance(this.transform.position, target.transform.position);
            if (distanceToPlayer <= attackRange && distanceToPlayer >= minAttackDistance)
            {
                PerformAoEAttack();
                aoeTimer = aoeCooldown;
            }
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
    /// Perform an area-of-effect attack.
    /// </summary>
    void PerformAoEAttack()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(this.transform.position, aoeRadius);
        foreach (var hitCollider in hitColliders)
        {
            // Check if the hit object is a player
            if (hitCollider.CompareTag("Player"))
            {
                // Apply damage to the player
                PlayerScript player = hitCollider.gameObject.GetComponent<PlayerScript>();
                if (player != null)
                {
                    player.Health -= aoeDamage;
                }
            }
        }

        /// <summary>
        /// Draw the AoE radius when selected
        /// </summary>
        void OnDrawGizmosSelected()
        {
            // Draw the AoE radius when selected
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.transform.position, aoeRadius);
        }
    }
}
