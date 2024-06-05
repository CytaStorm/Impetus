using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    private float _enemyHealth;
    public GameObject player;
    public GameObject weapon;

    private PlayerStats _playerStats;
    
    // Start is called before the first frame update
    void Start()
    {
        this._playerStats = player.GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("Collision Detected");
        
        if (collision.gameObject == player)
        {
            print("Enemy detected attack");
            float playerDamage = _playerStats.Damage;

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
        else
        {
            print(collision.gameObject.name);
        }
    }
}
