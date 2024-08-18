using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawningScript : MonoBehaviour
{
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private Transform spawnPoint;
    private bool bossSpawned = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !bossSpawned)
        {
            SpawnBoss();
            bossSpawned = true;
        }
    }

    private void SpawnBoss()
    {
        Instantiate(bossPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
