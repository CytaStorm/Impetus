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
    [SerializeField, Range(0f, 2f)] private float _meleeAttackDuration;
    [SerializeField] private float _spinRadiusOffset;
    [SerializeField] private float _spinAttackDuration;

    //GameObjects
    [SerializeField] private GameObject _target;
    [SerializeField] private GameObject pivot;
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

        //StartCoroutine("PerformBasicMeleeAttack");
        StartCoroutine("SpinAttack");
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

    #region PerformAoEAttack

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_aoeRadius"></param>
    /// <param name="_aoeDamage"></param>
    public void PerformAoEAttack(float _aoeRadius, float _aoeDamage)
    {
        int attackType = Random.Range(0, 3); 

        switch (attackType)
        {
            case 0:
                StartCoroutine(PerformBasicMeleeAttack());
                break;
            case 1:
                StartCoroutine(SpinAttack());
                break;
            case 2:
                PerformPlusGroundSmashAttack();
                break;
            case 3:
                PerformXGroundSmashAttack();
                break;
        }
    }// <summary>
    /// Performs a (AoE) attack by randomly selecting one of the available attack types:
    /// Basic Melee Attack, Spin Attack, Plus Ground Smash Attack, or X Ground Smash Attack.(more will be added)
    /// </summary>
    /// <param name="_aoeRadius">The radius of the AoE attack.</param>
    /// <param name="_aoeDamage">The damage dealt by the AoE attack.</param>
    public void PerformAoEAttack(float _aoeRadius, float _aoeDamage)
    {
        // Generate a random number between 0 and 3 to select the type of attack
        int attackType = Random.Range(0, 3);

        // Perform the selected attack based on the random number
        switch (attackType)
        {
            case 0:
                // Perform a basic melee attack
                StartCoroutine(PerformBasicMeleeAttack());
                break;
            case 1:
                // Perform a spin attack
                StartCoroutine(SpinAttack());
                break;
            case 2:
                // Perform a plus ground smash attack
                PerformPlusGroundSmashAttack();
                break;
            case 3:
                // Perform an X ground smash attack
                PerformXGroundSmashAttack();
                break;
        }
    }
    #endregion

    #region BasicMeleeAttack
    IEnumerator PerformBasicMeleeAttack()
    {
        hammer.SetActive(true);

        //Get direction, rotation, and magnitude of the range of motion
        Vector3 direction = (_target.transform.position - this.transform.position).normalized;
        hammer.transform.right = direction;
        Vector3 attackVector = direction * _meleeRange;

        //Set position so that tip of hammer is at center of boss, pointed at the target
        hammer.transform.position = -direction * (hammerWidth / 2f);
        
        //Get rigidbody2D
        Rigidbody2D hammerRB = hammer.GetComponent<Rigidbody2D>();

        //Take half the duration to move forward and again to go backwards
        float halfAttackDuration = _meleeAttackDuration / 2f;
        hammerRB.velocity = attackVector / halfAttackDuration;
        yield return new WaitForSeconds(halfAttackDuration);
        hammerRB.velocity = attackVector / halfAttackDuration * -1;
        yield return new WaitForSeconds(halfAttackDuration);
        hammerRB.velocity = Vector2.zero;

        hammer.SetActive(false);
        print("doneMeleeing");
    }
    #endregion

    #region SpinAttack
    IEnumerator SpinAttack()
    {
        hammer.SetActive(true);

        //Get direction and get the direction 90 degrees from it
        Vector3 direction = (_target.transform.position - this.transform.position).normalized;
        Vector3 orthogonalDirection = new Vector3(
            direction.y,
            -direction.x,
            0f);

        //Set the direction and position of the hammer
        hammer.transform.right = orthogonalDirection;
        hammer.transform.position = orthogonalDirection * (hammerWidth / 2f + _spinRadiusOffset);

        //Set the angular velocity of the hammer to rotate according to the speed
        Rigidbody2D pivotRB = pivot.GetComponent<Rigidbody2D>();
        pivotRB.angularVelocity = 360f / _spinAttackDuration;

        //Wait the duration and then put the hammer away
        yield return new WaitForSeconds(_spinAttackDuration);
        pivotRB.angularVelocity = 0;
        hammer.SetActive(false);
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
