using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public List<EnemySpawner.SpawnEntry> enemiesToSpawn;
        public int waveIncrement = 1;
    }

    [Header("Waves")]
    [SerializeField] private List<Wave> waves;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private int bossWaveFrequency = 3;

    private static int currentWaveIndex = 0; // Static variable to persist wave index between scene loads
    private int waveMultiplier = 1;
    private bool waveInProgress = false;
    private List<EnemyBase> activeEnemies = new List<EnemyBase>();
    private bool isBossWave = false;

    private void Start()
    {
        StartNextWave();
    }

    private void StartNextWave()
    {
        if (currentWaveIndex % bossWaveFrequency == 0 && currentWaveIndex != 0)
        {
            isBossWave = true;
            SpawnBoss();
        }
        else
        {
            isBossWave = false;
            SpawnWave();
        }
    }

    private void SpawnWave()
    {
        waveInProgress = true;
        var currentWave = waves[currentWaveIndex % waves.Count];

        foreach (var spawnPoint in spawnPoints)
        {
            var spawner = spawnPoint.GetComponent<EnemySpawner>();
            var modifiedSpawns = new List<EnemySpawner.SpawnEntry>();
            foreach (var entry in currentWave.enemiesToSpawn)
            {
                var newEntry = new EnemySpawner.SpawnEntry
                {
                    enemyPrefab = entry.enemyPrefab,
                    amount = entry.amount + (waveMultiplier * currentWave.waveIncrement)
                };
                modifiedSpawns.Add(newEntry);
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

    public bool IsBossWave()
    {
        return isBossWave;
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

        // Notify the player of the kill
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            player.IncrementEnemiesKilled();
        }

        if (!waveInProgress && activeEnemies.Count == 0)
        {
            currentWaveIndex++;
            StartNextWave();
            if (player != null)
            {
                player.IncrementWavesCleared();
            }
        }
    }

    public void OnPlayerDeath()
    {
        StopAllCoroutines();
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

    public int GetCurrentWaveIndex() => currentWaveIndex;
}
