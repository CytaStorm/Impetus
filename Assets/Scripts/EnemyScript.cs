using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    private float _enemyHealth;
    public GameObject player;
    public GameObject weapon;

    private PlayerAttackScript _playerAttack;
    
    // Start is called before the first frame update
    void Start()
    {
        this._playerAttack = player.GetComponent<PlayerAttackScript>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("Hitting with " + collision.collider.tag);


        // Check if the colliding object has the tag "Weapon"
        if (collision.collider.CompareTag("Weapon"))
        {
            print("Player hit enemy!");
            float playerDamage = _playerAttack.Damage;
            print("Damage: " + playerDamage);

            _enemyHealth -= playerDamage;
            if (_enemyHealth > 0)
            {
                print("Enemy remaining health: " + _enemyHealth);
            }
            else
            {
                print("Enemy has died");
            }
        }
        
    }
}
