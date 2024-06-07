using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


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

    private UnityEvent _enemyDied;
    


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

        if(_enemyDied == null)
        {
            _enemyDied = new UnityEvent();
        }
        _enemyDied.AddListener(killEnemy);


    }

    /**
     *      Listens to the event of an enemy dying 
     *      - kills the enemy, increases flow
     */
    private void killEnemy()
    {
        //Kill the enemy
        Destroy(this.gameObject);

        //Gain flow on hit based on "flow worth" of the enemy
        _playerStats.Flow += _flowWorth;

    }

    // Update is called once per frame
    void Update()
    {
        if (this._enemyHealth<= 0)
        {
            _enemyDied.Invoke();
        }
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
