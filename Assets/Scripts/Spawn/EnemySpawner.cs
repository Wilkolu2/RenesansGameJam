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

    private WaveManager waveManager;

    private void Start()
    {
        waveManager = FindObjectOfType<WaveManager>();
    }

    public void ResetSpawner(List<SpawnEntry> newSpawnEntries)
    {
        enemiesToSpawn = newSpawnEntries;
        SpawnEnemies();
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
    }

    public void DisableSpawner()
    {
        enemiesToSpawn.Clear();
    }
}
