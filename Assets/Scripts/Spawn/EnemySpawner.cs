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

        if (waveManager == null)
        {
            Debug.LogError("WaveManager not found!");
        }
    }

    private void OnEnable()
    {
        waveManager = FindObjectOfType<WaveManager>();

        if (waveManager == null)
        {
            Debug.LogError("WaveManager not found!");
        }
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
                var enemyBase = enemy.GetComponent<EnemyBase>();
                if (enemyBase != null)
                {
                    if (waveManager != null)
                    {
                        waveManager.RegisterSpawnedEnemy(enemyBase);
                    }
                    else
                    {
                        Debug.LogError("WaveManager is null when trying to register spawned enemy.");
                    }
                }
                else
                {
                    Debug.LogError("Spawned enemy does not have an EnemyBase component.");
                }
            }

            enemiesToSpawn.RemoveAt(i);
        }
    }

    public void DisableSpawner()
    {
        enemiesToSpawn.Clear();
    }
}
