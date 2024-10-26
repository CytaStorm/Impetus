using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawningScript : MonoBehaviour
{
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private GameObject pivotPrefab;
    [SerializeField] private Transform spawnPoint;
    //private bool bossSpawned = false;

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player") && !bossSpawned)
    //    {
    //        SpawnBoss();
    //        bossSpawned = true;
    //    }
    //}

    //private void SpawnBoss()
    //{
    //    Debug.Log("Starting Boss Room");
    //    Instantiate(Boss, spawnPoint.position, spawnPoint.rotation);
    //}

    private void Start()
    {
        Debug.Log("Starting Boss Room from start");
        GameObject boss = Instantiate(bossPrefab, spawnPoint.position, spawnPoint.rotation);
        GameObject pivot = Instantiate(pivotPrefab, spawnPoint.position, spawnPoint.rotation);

        boss.GetComponent<bossEnemyAttacksScript>().pivot = pivot;
        boss.GetComponent<bossEnemyAttacksScript>().hammer = pivot.transform.GetChild(0).gameObject;

    }
}
