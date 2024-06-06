using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    public GameObject player;
    public GameObject weapon;

    [SerializeField] private float _enemyHealth;
    [SerializeField] private float _aetherIncrease;
    [SerializeField] private float _enemyDamage;
    [SerializeField] private float _flowWorth;    

    private PlayerAttackScript _playerAttack;
    private PlayerStats _playerStats;
    


    // Start is called before the first frame update
    void Start()
    {
        this._playerAttack = player.GetComponent<PlayerAttackScript>();
        this._enemyHealth = 100;
        this._playerStats = player.GetComponent<PlayerStats>();

        //Temporary for testing purposes

        this._aetherIncrease = 10;
        this._enemyDamage = 10;
        this._flowWorth = 50;


        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        print("Hitting with " + collision.tag);


        // Check if the colliding object has the tag "Weapon"
        // to see if the weapon is hitting the enemy

        if (collision.CompareTag("Weapon"))
        {
            print("Player hit enemy!");
            float playerDamage = _playerAttack.Damage;
            
            print("Damage: " + playerDamage);


            //Damage the enemy
            _enemyHealth -= playerDamage;
            if (_enemyHealth > 0)
            {
                print("Enemy remaining health: " + _enemyHealth);
            }
            else
            {
                print("Enemy has died");
            }

            //Regain aether on the player
            _playerStats.Aether += _aetherIncrease;

            //Increase flow
            if ((_playerStats.Flow + _flowWorth > 100) && (_playerStats.FlowState == _playerStats.MaxFlow))
            {
                _playerStats.Flow = 99;
            }
            else
            {
                _playerStats.Flow += _flowWorth;
            }
            


        }

        
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Check if it has the tag "player" to see if the player is physically touching the enemy
        if (collision.gameObject.CompareTag("Player"))
        {
            print("Player taking damage from touching enemy!");

            _playerStats.Health -= _enemyDamage;
        }
    }
}
