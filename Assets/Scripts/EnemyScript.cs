using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


public class EnemyScript : MonoBehaviour
{

    public GameObject player;
    public GameObject weapon;

    [SerializeField] private float _enemyHealth = 100f;
    [SerializeField] private float _aetherIncrease = 10f;
    [SerializeField] private float _enemyDamage = 10f;
    [SerializeField] private float _flowWorth = 50f;

    private PlayerAttackScript _playerAttack;
    private PlayerStats _playerStats;

    private UnityEvent _enemyDied;

    private Camera mainCamera;
    private float screenLeft;
    private float screenRight;
    private float screenTop;
    private float screenBottom;

    // Start is called before the first frame update
    void Start()
    {
        // this._enemyHealth = 100;

        //Temporary for testing purposes

        //this._aetherIncrease = 10;
        //this._enemyDamage = 10;
        //this._flowWorth = 50;

        // Initialize screen boundaries
        mainCamera = Camera.main;
        float verticalExtent = mainCamera.orthographicSize;
        float horizontalExtent = verticalExtent * Screen.width / Screen.height;

        screenLeft = mainCamera.transform.position.x - horizontalExtent;
        screenRight = mainCamera.transform.position.x + horizontalExtent;
        screenBottom = mainCamera.transform.position.y - verticalExtent;
        screenTop = mainCamera.transform.position.y + verticalExtent;

        //Safety check to make sure 
        if (player != null)
        {
            _playerAttack = player.GetComponent<PlayerAttackScript>();
            _playerStats = player.GetComponent<PlayerStats>();

            //Checks if either player attack or player stats are missing
            if (_playerAttack == null || _playerStats == null)
            {
                Debug.LogError("Player components are missing.");
                return;
            }
        }
        else
        {
            Debug.LogError("Player GameObject is not assigned.");
            return;
        }

        if (_enemyDied == null)
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
        if (this._enemyHealth <= 0)
        {
            _enemyDied.Invoke();
        }

        // Keeps enemy on the screen
        Vector3 enemyPos = transform.position;
        enemyPos.x = Mathf.Clamp(enemyPos.x, screenLeft, screenRight);
        enemyPos.y = Mathf.Clamp(enemyPos.y, screenBottom, screenTop);
        transform.position = enemyPos;
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

    /// <summary>
    /// This method is called when a gameObject is destroyed
    /// </summary>
    private void OnDestroy()
    {
        // Check if the _enemyDied event is not null

        if (_enemyDied != null)
        {
            // Remove the KillEnemy method from the _enemyDied event
            _enemyDied.RemoveListener(killEnemy);
        }
    }
}
