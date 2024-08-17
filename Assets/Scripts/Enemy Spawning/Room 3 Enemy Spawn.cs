using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room3EnemySpawn : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject smallEnemy; // Prefab for small enemy
    private int smallEnemyCount;   // Number of small enemies to spawn
    [SerializeField] private GameObject rangedEnemy; // Prefab for ranged enemy
    private int rangedEnemyCount;   // Number of ranged enemies to spawn
    [SerializeField] private GameObject bigEnemy;    // Prefab for big enemy
    private int bigEnemyCount;      // Number of big enemies to spawn

    [SerializeField] private Vector2 spawnAreaCenter1; // Center point of the first spawn area
    [SerializeField] private Vector2 spawnAreaSize1;   // Size (width and height) of the first spawn area
    [SerializeField] private Vector2 spawnAreaCenter2; // Center point of the second spawn area
    [SerializeField] private Vector2 spawnAreaSize2;   // Size (width and height) of the second spawn area
    [SerializeField] private Vector2 spawnAreaCenter3; // Center point of the third spawn area
    [SerializeField] private Vector2 spawnAreaSize3;   // Size (width and height) of the third spawn area
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //Do from 1-2 of each type of enemy in each room
        smallEnemyCount = UnityEngine.Random.Range(1, 2);
        rangedEnemyCount = UnityEngine.Random.Range(1, 2);
        bigEnemyCount = UnityEngine.Random.Range(1, 2);

        //For each small enemy we wanna spawn
        for (int i = 0; i < smallEnemyCount; i++)
        {
            int spawnLocaion = UnityEngine.Random.Range(0, 2);
            switch(spawnLocaion)
            {
                case 0:
                    SpawnEnemies(smallEnemy, smallEnemyCount, spawnAreaCenter1, spawnAreaSize1);
                    break;
                case 1:
                    SpawnEnemies(smallEnemy, smallEnemyCount, spawnAreaCenter2, spawnAreaSize2);
                    break;
                case 2:
                    SpawnEnemies(smallEnemy, smallEnemyCount, spawnAreaCenter3, spawnAreaSize3);
                    break;
            }
        }

        //For each ranged enemy we wanna spawn
        for (int i = 0; i < rangedEnemyCount; i++)
        {
            int spawnLocaion = UnityEngine.Random.Range(0, 2);
            switch (spawnLocaion)
            {
                case 0:
                    SpawnEnemies(rangedEnemy, smallEnemyCount, spawnAreaCenter1, spawnAreaSize1);
                    break;
                case 1:
                    SpawnEnemies(rangedEnemy, smallEnemyCount, spawnAreaCenter2, spawnAreaSize2);
                    break;
                case 2:
                    SpawnEnemies(rangedEnemy, smallEnemyCount, spawnAreaCenter3, spawnAreaSize3);
                    break;
            }
        }

        //For each big enemy we wanna spawn
        for (int i = 0; i < bigEnemyCount; i++)
        {
            int spawnLocaion = UnityEngine.Random.Range(0, 2);
            switch (spawnLocaion)
            {
                case 0:
                    SpawnEnemies(bigEnemy, smallEnemyCount, spawnAreaCenter1, spawnAreaSize1);
                    break;
                case 1:
                    SpawnEnemies(bigEnemy, smallEnemyCount, spawnAreaCenter2, spawnAreaSize2);
                    break;
                case 2:
                    SpawnEnemies(bigEnemy, smallEnemyCount, spawnAreaCenter3, spawnAreaSize3);
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Enemies Spawn Method
    /// <summary>
    /// Spawns a specific number of differnt enemies inside a defined spawn area.
    /// </summary>
    /// <param name="enemyPrefab">The prefab of the enemy to spawn.</param>
    /// <param name="enemyCount">The number of enemies to spawn.</param>
    /// <param name="areaCenter">The center point of the spawn area.</param>
    /// <param name="areaSize">The size of the spawn area (width and height).</param>
    void SpawnEnemies(GameObject enemyPrefab, int enemyCount, Vector2 areaCenter, Vector2 areaSize)
    {
        for (int i = 0; i < enemyCount; i++)
        {
            // Generate a random position within the spawn area bounds
            float randomX = Random.Range(areaCenter.x - areaSize.x / 2, areaCenter.x + areaSize.x / 2);
            float randomY = Random.Range(areaCenter.y - areaSize.y / 2, areaCenter.y + areaSize.y / 2);
            Vector2 randomPosition = new Vector2(randomX, randomY);

            // Instantiate the enemy at the random position with no rotation (Quaternion.identity)
            Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
        }
    }
    #endregion
}
