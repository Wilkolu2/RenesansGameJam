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
    private WaveManager waveManager;

    private void Start()
    {
        spawnTimer = spawnInterval;
        waveManager = FindObjectOfType<WaveManager>();
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
        for (int i = enemiesToSpawn.Count - 1; i >= 0; i--)
        {
            var entry = enemiesToSpawn[i];
            for (int j = 0; j < entry.amount; j++)
            {
                var enemy = Instantiate(entry.enemyPrefab, transform.position, transform.rotation);
                waveManager.RegisterSpawnedEnemy(enemy.GetComponent<EnemyBase>());
            }

            enemiesToSpawn.RemoveAt(i);
        }

        if (enemiesToSpawn.Count == 0)
        {
            spawningEnabled = false;
        }
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
