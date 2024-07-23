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
    [SerializeField, Range(0f, 3f)] private float _spinRadiusOffset;
    [SerializeField, Range(1f, 5f)] private float _spinAttackDuration;

    //GameObjects
    [SerializeField] private GameObject _target;
    [SerializeField] private GameObject pivot;
    [SerializeField] private GameObject hammer;
    [SerializeField] private GameObject AOE;
    [SerializeField] private Sprite hammerSprite;
    private Rigidbody2D pivotRB;

    //useful values
    private float hammerLength;
    private Vector3 bossPos;
    private Vector3 lastBossPos;
    private bool rotating;

    #endregion
    void Start()
    {
        //Instantiate variables
        _smashTimer = _smashCooldown;
        hammerLength = hammerSprite.rect.width / hammerSprite.pixelsPerUnit;
        lastBossPos = this.transform.position;
        bossPos = this.transform.position;
        pivotRB = pivot.GetComponent<Rigidbody2D>();
        rotating = false;

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
                PerformAnyAttack(2, 20);
            }
        }

        //Keep the hammer moving with the boss, while keeping its velocities seperate.
        //This allows me to use rigidbody2d instead of transform for the basic melee attack
        if (hammer.activeInHierarchy)
        {
            //Update the boss positions and move based on their movement each frame
            lastBossPos = bossPos;
            bossPos = this.transform.position;
            pivot.transform.position += bossPos - lastBossPos;

            //While the spin attack keeps rotating true, rotate
            if (rotating)
            {
                pivot.transform.Rotate(0, 0, 360f / _spinAttackDuration * Time.deltaTime);
            }
        }
    }

    #region PerformAnyAttack

    /// <summary>
    /// Performs a (AoE) attack by randomly selecting one of the available attack types:
    /// Basic Melee Attack, Spin Attack, Plus Ground Smash Attack, or X Ground Smash Attack.(more will be added)
    /// </summary>
    /// <param name="_aoeRadius">The radius of the AoE attack.</param>
    /// <param name="_aoeDamage">The damage dealt by the AoE attack.</param>
    public void PerformAnyAttack(float _aoeRadius, float _aoeDamage)
    {
        // Generate a random number between 0 and 3 to select the type of attack
        int attackType = Random.Range(0, 3);

        // Perform the selected attack based on the random number
        switch (attackType)
        {
            case 0:
                // Perform a basic melee attack
                //StartCoroutine(PerformBasicMeleeAttack());
                PerformXGroundSmashAttack();
                break;
            case 1:
                // Perform a spin attack
                // StartCoroutine(SpinAttack());
                PerformPlusGroundSmashAttack();
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
        bossPos = this.transform.position;

        //Get direction, rotation, and magnitude of the range of motion
        Vector3 direction = (_target.transform.position - this.transform.position).normalized;
        pivot.transform.right = direction;
        Vector3 attackVector = direction * _meleeRange;

        //Set position so that tip of hammer is at center of boss, pointed at the target
        pivot.transform.position = bossPos - direction * hammerLength;

        //Get rigidbody2D
        Rigidbody2D hammerRB = hammer.GetComponent<Rigidbody2D>();

        //Take half the duration to move forward and again to go backwards
        float halfAttackDuration = _meleeAttackDuration / 2f;
        hammerRB.velocity = attackVector / halfAttackDuration;
        yield return new WaitForSeconds(halfAttackDuration);
        hammerRB.velocity = attackVector / halfAttackDuration * -1;
        yield return new WaitForSeconds(halfAttackDuration);
        hammerRB.velocity = Vector2.zero;
        _smashTimer = _smashCooldown;

        hammer.SetActive(false);
        print("doneMeleeing");
    }
    #endregion

    #region SpinAttack
    IEnumerator SpinAttack()
    {
        hammer.SetActive(true);
        bossPos = this.transform.position;

        //Get direction and get the direction 90 degrees from it
        Vector3 direction = (_target.transform.position - this.transform.position).normalized;
        Vector3 orthogonalDirection = new Vector3(
            direction.y,
            -direction.x,
            0f);

        //Set the direction and position of the hammer
        pivot.transform.right = orthogonalDirection;
        pivot.transform.position = bossPos + orthogonalDirection;
        hammer.transform.position += orthogonalDirection * _spinRadiusOffset;

        //Rotate in update for the duration, then stop
        rotating = true;
        yield return new WaitForSeconds(_spinAttackDuration);
        rotating = false;
        hammer.SetActive(false);
        _smashTimer = _smashCooldown;
    }
    #endregion

    #region PlusGroundSmashAttack
    void PerformPlusGroundSmashAttack()
    {
        Debug.Log("Performing Plus Ground Smash Attack");
        Vector2[] attackDirections = new Vector2[]
        {
        Vector2.up, Vector2.down, Vector2.left, Vector2.right
        };

        float boxWidth = 15f; // Adjust the width as necessary
        float boxHeight = _attackRange;

        foreach (Vector2 direction in attackDirections)
        {
            Vector2 boxSize = (direction == Vector2.left || direction == Vector2.right) ?
                new Vector2(_attackRange, boxWidth) :
                new Vector2(boxWidth, _attackRange);

            RaycastHit2D hit = Physics2D.BoxCast(transform.position, boxSize, 0, direction, _attackRange);
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                PlayerScript player = hit.collider.GetComponent<PlayerScript>();
                if (player != null)
                {
                    Debug.Log("Player hit in Plus Ground Smash!");
                    player.Health -= _smashDamage;
                }
                break;
            }
        }
        _smashTimer = _smashCooldown;
    }
    #endregion

    #region XGroundSmashAttack
    void PerformXGroundSmashAttack()
    {
        Debug.Log("Performing X Ground Smash Attack");
        Vector2[] attackDirections = new Vector2[]
        {
        new Vector2(1, 1), new Vector2(-1, -1), new Vector2(1, -1), new Vector2(-1, 1)
        };

        float boxWidth = 15f; // Adjust the width as necessary

        foreach (Vector2 direction in attackDirections)
        {
            Vector2 boxSize = new Vector2(boxWidth, _attackRange);
            RaycastHit2D hit = Physics2D.BoxCast(transform.position, boxSize, 0, direction, _attackRange);
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                PlayerScript player = hit.collider.GetComponent<PlayerScript>();
                if (player != null)
                {
                    Debug.Log("Player hit in X Ground Smash!");
                    player.Health -= _smashDamage;
                }
            }
        }
        _smashTimer = _smashCooldown;
    }
    #endregion

    #region DrawAttack
    /// <summary>
    /// Draws visual representations of the attack ranges for the ground smash attacks
    /// when the GameObject is selected in the editor.
    /// </summary>
    void OnDrawGizmosSelected()
    {
        if(_attackRange > 0)
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
        
    }
    #endregion

}
