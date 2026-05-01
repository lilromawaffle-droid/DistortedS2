using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Spawner musuh 3D dengan sistem wave.
/// Spawn di sekitar player, selalu di posisi yang valid di NavMesh.
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemySpawnData
    {
        public string       poolTag;
        public EnemyStatsSO stats;
        public int          countPerWave = 10;
        [Range(0f, 1f)]
        public float        spawnWeight  = 1f;
    }

    [Header("Enemy Types")]
    public List<EnemySpawnData> enemyTypes;

    [Header("Spawn Settings")]
    public float spawnRadius    = 15f;
    public float minSpawnRadius = 8f;

    [Header("Wave Settings")]
    public float waveInterval   = 30f;
    public int   currentWave    = 1;
    public int   maxEnemiesAlive = 300;

    [Header("Continuous Spawn")]
    public bool  continuousSpawn   = true;
    public float spawnInterval     = 0.4f;
    public int   spawnPerInterval  = 3;

    private Transform playerTransform;
    private int       activeEnemyCount;

    void Start()
    {
        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) playerTransform = playerObj.transform;

        StartCoroutine(WaveRoutine());
        if (continuousSpawn)
            StartCoroutine(ContinuousSpawnRoutine());
    }

    // ─── Wave tiap beberapa detik ─────────────────────────────────────────
    IEnumerator WaveRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(waveInterval);
            SpawnWave();
            currentWave++;
        }
    }

    void SpawnWave()
    {
        Debug.Log($"[Wave {currentWave}] Dimulai!");
        foreach (var data in enemyTypes)
        {
            int count = Mathf.RoundToInt(data.countPerWave * (1f + (currentWave - 1) * 0.2f));
            for (int i = 0; i < count; i++)
            {
                if (activeEnemyCount >= maxEnemiesAlive) return;
                SpawnEnemy(data);
            }
        }
    }

    // ─── Spawn terus-menerus seperti Vampire Survivors ───────────────────
    IEnumerator ContinuousSpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            if (activeEnemyCount >= maxEnemiesAlive) continue;

            for (int i = 0; i < spawnPerInterval; i++)
            {
                var data = GetRandomEnemyType();
                if (data != null) SpawnEnemy(data);
            }
        }
    }

    EnemySpawnData GetRandomEnemyType()
    {
        float total = 0f;
        foreach (var e in enemyTypes) total += e.spawnWeight;
        float rand = Random.Range(0f, total);
        float cumul = 0f;
        foreach (var e in enemyTypes)
        {
            cumul += e.spawnWeight;
            if (rand <= cumul) return e;
        }
        return enemyTypes.Count > 0 ? enemyTypes[0] : null;
    }

    // ─── Spawn satu musuh di posisi valid NavMesh ─────────────────────────
    void SpawnEnemy(EnemySpawnData data)
    {
        if (playerTransform == null || ObjectPool.Instance == null) return;

        Vector3 spawnPos;
        if (!TryGetNavMeshSpawnPoint(out spawnPos)) return;

        GameObject obj = ObjectPool.Instance.Get(data.poolTag, spawnPos, Quaternion.identity);
        if (obj == null) return;

        var enemy = obj.GetComponent<EnemyBase>();
        if (enemy == null) return;

        enemy.poolTag = data.poolTag;
        enemy.stats   = data.stats;
        enemy.Initialize(currentWave);

        activeEnemyCount++;
        StartCoroutine(WatchEnemy(obj));
    }

    IEnumerator WatchEnemy(GameObject obj)
    {
        yield return new WaitUntil(() => !obj.activeInHierarchy);
        activeEnemyCount = Mathf.Max(0, activeEnemyCount - 1);
    }

    // ─── Cari posisi valid di NavMesh sekitar player ──────────────────────
    bool TryGetNavMeshSpawnPoint(out Vector3 result)
    {
        for (int attempt = 0; attempt < 10; attempt++)
        {
            // Arah acak di lingkaran sekitar player
            Vector2 randomCircle = Random.insideUnitCircle.normalized;
            float   dist         = Random.Range(minSpawnRadius, spawnRadius);
            Vector3 candidate    = playerTransform.position
                                 + new Vector3(randomCircle.x, 0f, randomCircle.y) * dist;

            // Cek apakah posisi tersebut ada di NavMesh
            NavMeshHit hit;
            if (NavMesh.SamplePosition(candidate, out hit, 3f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }

        result = Vector3.zero;
        return false; // Tidak ketemu posisi valid — skip spawn ini
    }

    // ─── Gizmos debug ─────────────────────────────────────────────────────
    void OnDrawGizmosSelected()
    {
        if (playerTransform == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(playerTransform.position, spawnRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(playerTransform.position, minSpawnRadius);
    }
}
