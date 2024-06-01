using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public List<EnemySpawner.SpawnEntry> enemiesToSpawn;
        public int waveMultiplier = 1;
    }

    [Header("Waves")]
    [SerializeField] private List<Wave> waves;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private int bossWaveFrequency = 3;

    private int currentWaveIndex = 0;
    private int waveMultiplier = 1;
    private bool waveInProgress = false;
    private List<EnemyBase> activeEnemies = new List<EnemyBase>();

    private void Start()
    {
        StartNextWave();
    }

    private void StartNextWave()
    {
        if (currentWaveIndex % bossWaveFrequency == 0 && currentWaveIndex != 0)
            SpawnBoss();
        else
            SpawnWave();
    }

    private void SpawnWave()
    {
        waveInProgress = true;
        var currentWave = waves[currentWaveIndex % waves.Count];

        foreach (var spawnPoint in spawnPoints)
        {
            var spawner = spawnPoint.GetComponent<EnemySpawner>();
            var modifiedSpawns = new List<EnemySpawner.SpawnEntry>(currentWave.enemiesToSpawn);
            foreach (var entry in modifiedSpawns)
            {
                entry.amount *= waveMultiplier;
            }
            spawner.ResetSpawner(modifiedSpawns);
        }

        waveMultiplier++;
        waveInProgress = false;
    }

    private void SpawnBoss()
    {
        foreach (var spawnPoint in spawnPoints)
        {
            var boss = Instantiate(bossPrefab, spawnPoint.position, spawnPoint.rotation);
            RegisterSpawnedEnemy(boss.GetComponent<EnemyBase>());
        }
    }

    public void RegisterSpawnedEnemy(EnemyBase enemy)
    {
        if (enemy != null)
        {
            activeEnemies.Add(enemy);
            enemy.OnDeath += OnEnemyKilled;
        }
    }

    private void OnEnemyKilled(EnemyBase enemy)
    {
        activeEnemies.Remove(enemy);
        if (!waveInProgress && activeEnemies.Count == 0)
        {
            currentWaveIndex++;
            StartNextWave();
        }
    }

    public void OnPlayerDeath()
    {
        StopAllCoroutines();
        currentWaveIndex = 0;
        waveMultiplier = 1;
        waveInProgress = false;
        foreach (var spawnPoint in spawnPoints)
        {
            var spawner = spawnPoint.GetComponent<EnemySpawner>();
            spawner.DisableSpawner();
        }

        foreach (var enemy in activeEnemies)
        {
            if (enemy != null)
                Destroy(enemy.gameObject);
        }
        activeEnemies.Clear();

        StartNextWave();
    }
}
