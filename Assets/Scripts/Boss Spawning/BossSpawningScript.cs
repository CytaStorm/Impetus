using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawningScript : MonoBehaviour
{
    [SerializeField] public GameObject bossPrefab;
    [SerializeField] public Transform spawnPoint; 
    private bool bossSpawned = false;

    // This function is called when another collider enters the trigger collider
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !bossSpawned)
        {
            SpawnBoss();
            bossSpawned = true; // Ensure the boss only spawns once
        }
    }

    void SpawnBoss()
    {
        Instantiate(bossPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
