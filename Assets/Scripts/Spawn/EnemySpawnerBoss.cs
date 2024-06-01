using UnityEngine;
using System.Collections;

public class EnemySpawnerBoss : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform spawnPoint;
    public float spawnInterval = 5f;
    public float spawnDelay = 3f;
    public GameObject bossObject;

    private bool canSpawn = true;

    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(spawnDelay);

        while (bossObject != null)
        {
            if (canSpawn)
            {
                SpawnEnemy();
                canSpawn = false;
                yield return new WaitForSeconds(spawnInterval);
                canSpawn = true;
            }
            yield return null;
        }
    }

    void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}