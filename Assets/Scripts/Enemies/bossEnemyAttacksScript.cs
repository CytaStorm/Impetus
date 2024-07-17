using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossEnemyAttacksScript : MonoBehaviour
{
    #region Variables
    //The damage the ground smash attacks will do
    [SerializeField] private float _smashDamage = 50f;

    //Time between each ground smash attack(may change this when other attacks are implemented)
    [SerializeField] private float _smashCooldown = 10f;

    //Timer
    private float _smashTimer;


    [SerializeField] private float _attackRange = 5f;
    [SerializeField] private float _minAttackDistance = 1f;
    [SerializeField] private GameObject _target;
    [SerializeField] private GameObject hammer;
    [SerializeField] private GameObject AOE;
    #endregion
    void Start()
    {
        _smashTimer = _smashCooldown;
    }

    void Update()
    {

        _smashTimer -= Time.deltaTime;
        if (_smashTimer <= 0f)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, _target.transform.position);
            if (distanceToPlayer <= _attackRange && distanceToPlayer >= _minAttackDistance)
            {
                //NOT A METHOD just logic because we will need attack logic here
                //PerformAttack();
                _smashTimer = _smashCooldown;
            }
        }
    }

    #region PlusGroundSmashAttack
    void PerformPlusGroundSmashAttack()
    {
        // Define the directions for the '+' pattern
        Vector2[] attackDirections = new Vector2[]
        {
        Vector2.up, Vector2.down, Vector2.left, Vector2.right
        };

        foreach (Vector2 direction in attackDirections)
        {
            // Perform the raycast in the given direction
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, _attackRange);
            // Check if the raycast hit the player
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                // Get the PlayerScript component
                PlayerScript player = hit.collider.GetComponent<PlayerScript>();
                if (player != null)
                {
                    // Apply damage to the player
                    player.Health -= _smashDamage;
                }
                break;
            }
        }
    }
    #endregion

    #region XGroundSmashAttack
    void PerformXGroundSmashAttack()
    {
        // Define the directions for the 'x' pattern
        Vector2[] attackDirections = new Vector2[]
    {
        new Vector2(1, 1), new Vector2(-1, -1), new Vector2(1, -1), new Vector2(-1, 1)
    };

        foreach (Vector2 direction in attackDirections)
        {
            // Perform the raycast in the given direction
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, _attackRange);
            // Check if the raycast hit the player
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                // Get the PlayerScript component
                PlayerScript player = hit.collider.GetComponent<PlayerScript>();
                if (player != null)
                {
                    // Apply damage to the player
                    player.Health -= _smashDamage;
                }
            }
        }

    }
    #endregion

    #region DrawAttack
    /// <summary>
    /// Draws visual representations of the attack ranges for the ground smash attacks
    /// when the GameObject is selected in the editor.
    /// </summary>
    void OnDrawGizmosSelected()
    {
        // Set the color of the Gizmos to red
        Gizmos.color = Color.red;
        // Draw a ray upward from the current position with the length of the attack range
        Gizmos.DrawRay(transform.position, Vector2.up * _attackRange);
        // Draw a ray downward from the current position with the length of the attack range
        Gizmos.DrawRay(transform.position, Vector2.down * _attackRange);
        // Draw a ray to the left from the current position with the length of the attack range
        Gizmos.DrawRay(transform.position, Vector2.left * _attackRange);
        // Draw a ray to the right from the current position with the length of the attack range
        Gizmos.DrawRay(transform.position, Vector2.right * _attackRange);

        // Set the color of the Gizmos to blue
        Gizmos.color = Color.blue;
        // Draw a diagonal ray (top-right) from the current position with the length of the attack range
        Gizmos.DrawRay(transform.position, new Vector2(1, 1) * _attackRange);
        // Draw a diagonal ray (bottom-left) from the current position with the length of the attack range
        Gizmos.DrawRay(transform.position, new Vector2(-1, -1) * _attackRange);
        // Draw a diagonal ray (top-left) from the current position with the length of the attack range
        Gizmos.DrawRay(transform.position, new Vector2(1, -1) * _attackRange);
        // Draw a diagonal ray (bottom-right) from the current position with the length of the attack range
        Gizmos.DrawRay(transform.position, new Vector2(-1, 1) * _attackRange);
    }
    #endregion

}
