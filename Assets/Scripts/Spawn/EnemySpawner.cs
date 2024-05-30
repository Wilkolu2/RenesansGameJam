using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnEntry
    {
        public GameObject enemyPrefab;
        public int amount;
    }

    [Header("Spawn Settings")]
    public List<SpawnEntry> enemiesToSpawn;
    [SerializeField] private float spawnInterval = 2f;

    private float spawnTimer;
    private bool spawningEnabled = false;

    private void Start()
    {
        spawnTimer = spawnInterval;
    }

    private void Update()
    {
        if (!spawningEnabled) return;

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            SpawnEnemies();
            spawnTimer = spawnInterval;
        }
    }

    private void SpawnEnemies()
    {
        foreach (var entry in enemiesToSpawn)
        {
            for (int i = 0; i < entry.amount; i++)
            {
                Instantiate(entry.enemyPrefab, transform.position, transform.rotation);
            }
        }

        enemiesToSpawn.Clear(); // Clear the list after spawning
    }

    public void ResetSpawner(List<SpawnEntry> newSpawnEntries)
    {
        spawnTimer = spawnInterval;
        enemiesToSpawn = newSpawnEntries;
        spawningEnabled = true;
    }

    public void DisableSpawner()
    {
        spawningEnabled = false;
    }
}
