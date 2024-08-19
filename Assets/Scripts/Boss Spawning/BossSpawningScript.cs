using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawningScript : MonoBehaviour
{
    [SerializeField] private GameObject Boss;
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
        Debug.Log("Starting Boss Room");
        Instantiate(Boss, spawnPoint.position, spawnPoint.rotation);
    }
}
