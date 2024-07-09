using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class walkingEnemyScript : MonoBehaviour
{
    public GameObject _player;
    public GameObject _enemy;

    public Vector2 playerPosition;
    public Vector2 enemyPosition;

    [SerializeField] public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerPosition = (Vector2)_player.transform.position;
        enemyPosition = (Vector2)_enemy.transform.position;
        moveTowardsPlayer();
    }



    private void moveTowardsPlayer()
    {
        Vector2 direction = (playerPosition - enemyPosition).normalized;
        _enemy.transform.position = Vector2.MoveTowards(enemyPosition, playerPosition, speed * Time.deltaTime);

    }
}
