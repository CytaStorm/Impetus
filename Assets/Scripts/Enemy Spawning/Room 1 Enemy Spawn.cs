using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room1EnemySpawn : MonoBehaviour
{
    #region Variables
    // Serialized fields allow these variables to be set in the Unity Editor
    [SerializeField] private GameObject smallEnemy; // Prefab for small enemy
    private int smallEnemyCount;   // Number of small enemies to spawn
    [SerializeField] private GameObject rangedEnemy; // Prefab for ranged enemy
    private int rangedEnemyCount;   // Number of ranged enemies to spawn
    [SerializeField] private GameObject bigEnemy;    // Prefab for big enemy
    private int bigEnemyCount;      // Number of big enemies to spawn

    [SerializeField] private Vector2 spawnAreaCenter; // Center point of the spawn area rectangle
    [SerializeField] private Vector2 spawnAreaSize;   // Size (width and height) of the spawn area rectangle
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        smallEnemyCount = UnityEngine.Random.Range(1, 2);
        rangedEnemyCount = UnityEngine.Random.Range(0, 2);

        SpawnEnemies(smallEnemy, smallEnemyCount);
        SpawnEnemies(rangedEnemy, rangedEnemyCount);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region SpawnEnemiesMethod
    /// <summary>
    /// Spawns a specific number of differnt enemies inside a defined spawn area.
    /// </summary>
    /// <param name="enemyPrefab">The prefab of the enemy to spawn.</param>
    /// <param name="enemyCount">The number of enemies to spawn.</param>
    void SpawnEnemies(GameObject enemyPrefab, int enemyCount)
    {
        for (int i = 0; i < enemyCount; i++)
        {
            // Generate a random position within the spawn rectangle bounds
            float randomX = Random.Range(spawnAreaCenter.x - spawnAreaSize.x / 2, spawnAreaCenter.x + spawnAreaSize.x / 2);
            float randomY = Random.Range(spawnAreaCenter.y - spawnAreaSize.y / 2, spawnAreaCenter.y + spawnAreaSize.y / 2);
            Vector2 randomPosition = new Vector2(randomX, randomY);

            // Instantiate the enemy at the random position with no rotation (Quaternion.identity)
            Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
        }
    }
    #endregion
}
