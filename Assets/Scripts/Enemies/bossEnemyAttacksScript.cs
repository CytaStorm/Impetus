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

    //Attack stats
    [SerializeField, Range(0f, 5f)] private float _meleeRange;
    [SerializeField] private float _attackRange = 5f;
    [SerializeField] private float _minAttackDistance = 1f;

    //GameObjects
    [SerializeField] private GameObject _target;
    [SerializeField] private GameObject hammer;
    [SerializeField] private GameObject AOE;
    [SerializeField] private Sprite hammerSprite;
    [SerializeField] private Sprite AOESprite;

    //useful values
    private float hammerWidth;

    #endregion
    void Start()
    {
        //Instantiate variables
        _smashTimer = _smashCooldown;
        hammerWidth = hammerSprite.rect.width / hammerSprite.pixelsPerUnit;

        //Set weapons to inactive
        hammer.SetActive(false);
        AOE.SetActive(false);

        StartCoroutine("PerformBasicMeleeAttack");
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

    #region BasicMeleeAttack
    IEnumerator PerformBasicMeleeAttack()
    {
        hammer.SetActive(true);
        //Get direction, rotation, and magnitude of the range of motion
        Vector2 direction = (_target.transform.position - this.transform.position).normalized;
        hammer.transform.right = direction;
        Vector2 attackVector = direction * _meleeRange;

        //Set position so that tip of hammer is at center of boss, pointed at the target
        hammer.transform.position -= 
            new Vector3(direction.x, direction.y, 0) * hammerWidth;
        
        //Get rigidbody2D
        Rigidbody2D hammerRB = hammer.GetComponent<Rigidbody2D>();

        //Take .5 seconds to move forward and again to go backwards
        hammerRB.velocity = attackVector * 2f;
        yield return new WaitForSeconds(.5f);
        hammerRB.velocity = attackVector * -2f;
        yield return new WaitForSeconds(.5f);
        hammerRB.velocity = Vector2.zero;
        hammer.SetActive(false);
        print("doneMeleeing");
    }
    #endregion

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
