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

    private int currentWaveIndex = 0;
    private bool waveInProgress = false;

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

        waveInProgress = false;
        currentWaveIndex++;
    }

    public void OnEnemyKilled()
    {
        if (waveInProgress) return;

        var enemies = FindObjectsOfType<EnemyBase>();
        if (enemies.Length == 0)
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
        StartCoroutine(StartNextWave());
    }
}
