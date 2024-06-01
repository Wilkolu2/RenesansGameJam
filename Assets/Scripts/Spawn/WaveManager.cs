using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public List<EnemySpawner.SpawnEntry> enemiesToSpawn;
        public float delayBetweenWaves;
    }

    [Header("Waves")]
    [SerializeField] private List<Wave> waves;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private int bossWaveIndex = 3;  // Change to the wave number when the boss should spawn
    [SerializeField] private GameObject bossPrefab;

    private int currentWaveIndex = 0;
    private bool waveInProgress = false;
    private List<EnemyBase> activeEnemies = new List<EnemyBase>();

    private void Start()
    {
        StartCoroutine(StartNextWave());
    }

    private IEnumerator StartNextWave()
    {
        if (currentWaveIndex >= waves.Count)
        {
            Debug.Log("All waves completed!");
            yield break;
        }

        waveInProgress = true;
        var currentWave = waves[currentWaveIndex];

        foreach (var spawnPoint in spawnPoints)
        {
            var spawner = spawnPoint.GetComponent<EnemySpawner>();
            spawner.ResetSpawner(new List<EnemySpawner.SpawnEntry>(currentWave.enemiesToSpawn));
        }

        yield return new WaitForSeconds(currentWave.delayBetweenWaves);

        if (currentWaveIndex == bossWaveIndex)
        {
            SpawnBoss();
        }

        waveInProgress = false;
        currentWaveIndex++;
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
            StartCoroutine(StartNextWave());
        }
    }

    public void OnPlayerDeath()
    {
        StopAllCoroutines();
        currentWaveIndex = 0;
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

        StartCoroutine(StartNextWave());
    }
}
