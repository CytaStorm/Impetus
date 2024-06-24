using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


public class EnemyScript : MonoBehaviour
{
    //GameObjects
    public GameObject player;
    public GameObject weapon;

    //SpriteRenderer
    SpriteRenderer spriteRenderer;

    //Visible stat variables
    [SerializeField] private float _enemyHealth;
    [SerializeField] private float _aetherIncrease;
    [SerializeField] private float _enemyDamage;
    [SerializeField] private float _flowWorth;

    //Script
    private PlayerScript _player;

    //Event
    private UnityEvent _enemyDied;

    //Camera stuff (not using rn)
    private Camera mainCamera;
    private float screenLeft;
    private float screenRight;
    private float screenTop;
    private float screenBottom;

    //PROPERTIES - for values that will be different across dif enemy types (health, damage, etc)
    public float EnemyHealth
    {
        get { return _enemyHealth; }
        set { _enemyHealth = value; }
    }
    public float AetherIncrease
    {
        set { _aetherIncrease = value; }
    }
    public float EnemyDamage
    {
        set { _enemyDamage = value; }
    }
    public float FlowWorth
    {
        set { _flowWorth = value; }
    }


    // Start is called before the first frame update
    void Start()
    {
        //Get the player via tag instead of relying on dragging him into the public box in unity UI
        player = GameObject.FindWithTag("Player");
        spriteRenderer = this.GetComponent<SpriteRenderer>();

        // this._enemyHealth = 100;

        //Temporary for testing purposes

        //this._aetherIncrease = 10;
        //this._enemyDamage = 10;
        //this._flowWorth = 50;

        // Initialize screen boundaries
        //mainCamera = Camera.main;
        //float verticalExtent = mainCamera.orthographicSize;
        //float horizontalExtent = verticalExtent * Screen.width / Screen.height;

        //screenLeft = mainCamera.transform.position.x - horizontalExtent;
        //screenRight = mainCamera.transform.position.x + horizontalExtent;
        //screenBottom = mainCamera.transform.position.y - verticalExtent;
        //screenTop = mainCamera.transform.position.y + verticalExtent;

        //Get the script for this individual enemy type
        

        //Safety check to make sure 
        if (player != null)
        {
            _player = player.GetComponent<PlayerScript>();

            //Checks if PlayerScript is missing
            if (_player == null)
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
        _player.Flow += _flowWorth;

    }

    // Update is called once per frame
    void Update()
    {
        if (this._enemyHealth <= 0)
        {
            _enemyDied.Invoke();
        }

        // Keeps enemy on the screen
        //Vector3 enemyPos = transform.position;
        //enemyPos.x = Mathf.Clamp(enemyPos.x, screenLeft, screenRight);
        //enemyPos.y = Mathf.Clamp(enemyPos.y, screenBottom, screenTop);
        //transform.position = enemyPos;
    }


    /// <summary>
    /// When a collision with a trigger object occurs, test to see what collided with us.
    /// If the collision is with a weapon, take damage.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        print("Hitting with " + collision.tag);


        // Check if the colliding object has the tag "Weapon"
        // to see if the weapon is hitting the enemy

        if (collision.CompareTag("Weapon"))
        {
            print("Player hit enemy!");
            float playerDamage = _player.AttackDamage;
            
            print("Damage: " + playerDamage);


            //Damage the enemy
            _enemyHealth -= playerDamage;
            if (_enemyHealth > 0)
            {
                print("Enemy remaining health: " + _enemyHealth);
                StartCoroutine("FlashRed");
            }
            else
            {
                print("Enemy has died");
            }

            //Regain aether on the player
            _player.Aether += _aetherIncrease;
        }     
    }

    /// <summary>
    /// When colliding with another collider, if that collider belongs to the player, 
    /// damage the player
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Check if it has the tag "player" to see if the player is physically touching the enemy
        if (collision.gameObject.CompareTag("Player"))
        {
            print("Player taking damage from touching enemy!");

            _player.Health -= _enemyDamage;
        }
    }

    /// <summary>
    /// Flash the enemy red for less than a second
    /// </summary>
    /// <returns>wait for less than a second before continuing</returns>
    private IEnumerator FlashRed()
    {
        //Make the enemy texture red
        spriteRenderer.color = Color.red;

        //Wait .3 seconds and then return color to normal untinted white
        yield return new WaitForSeconds(.3f);
        spriteRenderer.color = Color.white;
    }
}
