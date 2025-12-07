using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab, _EnemiesGroup;
    private static GameObject enemyPrefab, EnemiesGroup;
    public static List<GameObject> spawnedEnemies = new();

    public bool spawnActive = true;

    public float baseSpawnInterval = 5f;
    public float minSpawnInterval = 0.5f;
    public float maxSpawnInterval = 10f;

    public float minSpawnDistance = 16f;
    public float maxSpawnDistance = 25f;
    public float maxSpawnAttempts = 20f;
    public LayerMask obstacleMask;

    private Coroutine spawnCoroutine;
    private Transform playerTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerTransform = Utils.GetPlayer().transform;

        enemyPrefab = _enemyPrefab;
        EnemiesGroup = _EnemiesGroup;

        GameController.OnGameStarted += StartSpawnLoop;
        GameController.OnGameWon += StopSpawnLoop;
        GameController.OnTimerFinished += SpawnBoss;

        EnemyBehaviour.OnEnemyDeath += OnEnemyDeath;
    }

    void OnDestroy()
    {
        GameController.OnGameStarted -= StartSpawnLoop;
        GameController.OnGameWon -= StopSpawnLoop;
        GameController.OnTimerFinished -= SpawnBoss;

        EnemyBehaviour.OnEnemyDeath -= OnEnemyDeath;
    }

    void StartSpawnLoop()
    {
        spawnCoroutine = StartCoroutine(SpawnLoop());
    }
    void StopSpawnLoop()
    {
        if(spawnCoroutine != null) StopCoroutine(spawnCoroutine);
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            if(GameController.Instance.isPaused || spawnedEnemies.Count >= 150 || !spawnActive)
            {
                yield return null;
                continue;
            }

            float spawnInterval = GetDynamicSpawnInterval();
            SpawningInfo.spawnInterval = spawnInterval;
            yield return new WaitForSeconds(spawnInterval);

            int enemyAmount = 1;
            if(Random.value <= 0.10f) enemyAmount++;

            while(enemyAmount > 0)
            {
                if(TrySpawnEnemy(out GameObject spawnedEnemy)) spawnedEnemies.Add(spawnedEnemy);
                else Debug.Log("[EnemySpawner] Failed to spawn enemy after multiple attempts.");
                enemyAmount--; // Se mesmo após várias tentativas não conseguir spawnar, desiste e diminui a quantidade restante.
            }
        }
    }

    public bool TrySpawnEnemy(out GameObject spawnedEnemy)
    {
        spawnedEnemy = null;

        Vector3? maybePos = GetRandomPosAroundPlayer();
        if(maybePos == null) return false;
        Vector3 spawnPos = maybePos.Value;

        spawnedEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity, EnemiesGroup.transform);

        ScaleEnemyStats(spawnedEnemy);

        return true;
    }
    public static GameObject SpawnEnemy(Vector2 position)
    {
        GameObject enemy = Instantiate(enemyPrefab, position, Quaternion.identity, EnemiesGroup.transform);

        ScaleEnemyStats(enemy);

        return enemy;
    }

    Vector3? GetRandomPosAroundPlayer()
    {
        for(int attempt = 0; attempt < maxSpawnAttempts; attempt++)
        {
            Vector2 dir = Random.insideUnitCircle.normalized;
            float distance = Random.Range(minSpawnDistance, maxSpawnDistance);
            Vector3 potentialPos = playerTransform.position + (Vector3)(distance * dir);

            if(Physics2D.OverlapCircle(potentialPos, 0.5f, obstacleMask)) continue;
            if(IsVisibleToCamera(potentialPos)) continue;

            return potentialPos;
        }

        return null;
    }

    bool IsVisibleToCamera(Vector3 position)
    {
        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(position);
        return viewportPoint.x >= 0 && viewportPoint.x <= 1 && viewportPoint.y >= 0 && viewportPoint.y <= 1;
    }

    float GetDynamicSpawnInterval()
    {
        GetDifficulties(out float timeFactor, out float levelFactor, out float killFactor);          

        float finalInterval = baseSpawnInterval - (baseSpawnInterval * timeFactor) - levelFactor;

        finalInterval -= killFactor;

        return Mathf.Clamp(finalInterval, minSpawnInterval, maxSpawnInterval);
    }

    static void ScaleEnemyStats(GameObject enemy)
    {
        GetDifficulties(out float timeDifficulty, out float levelDifficulty, out float killDifficulty);

        SpawningInfo.lastTimeDifficulty = timeDifficulty;
        SpawningInfo.lastLevelDifficulty = levelDifficulty;
        SpawningInfo.lastKillDifficulty = killDifficulty;

        AttributesSystem enemyAttributes = enemy.GetComponent<AttributesSystem>();

        float healthUp = (timeDifficulty * 80f) + (levelDifficulty * 10f) + (killDifficulty * 5f);
        enemyAttributes.maxHealth.ApplyPercentUpgrade(healthUp);

        float moveSpeedUp = (timeDifficulty * 30f) + (levelDifficulty * 15f) + (killDifficulty * 1.5f);
        enemyAttributes.moveSpeed.ApplyPercentUpgrade(moveSpeedUp);

        float AttackDamageUp = (timeDifficulty * 50f) + (levelDifficulty * 5f) + (killDifficulty * 2f);
        enemyAttributes.attackDamage.ApplyPercentUpgrade(AttackDamageUp);

        bool isMutant = GameController.Instance.killedEnemies >= 1000 && Random.value <= 0.30f;
        if(isMutant)
        {
            enemyAttributes.maxHealth.ApplyPercentUpgrade(healthUp);
            enemyAttributes.moveSpeed.ApplyPercentUpgrade(moveSpeedUp/2);
            enemyAttributes.attackDamage.ApplyPercentUpgrade(AttackDamageUp);
            enemy.GetComponent<SpriteRenderer>().color = Color.red;
        }

        SpawningInfo.lastSpawnedZombie = enemy;
    }

    public static void GetDifficulties(out float timeDifficulty, out float levelDifficulty, out float killDifficulty)
    {
        float playerLevel = Utils.GetPlayer().GetComponent<LevelSystem>().CurrentLevel;
        float timeLeft = GameController.Instance.RoundTime;

        timeDifficulty = Mathf.Clamp01(1f - (timeLeft / GameController.MaxRoundTime));
        levelDifficulty = 1f + (playerLevel * 0.15f);
        killDifficulty = Mathf.Clamp(GameController.Instance.killedEnemies * 0.01f, 0f, 3f);
    }

    private void OnEnemyDeath(EnemyBehaviour behaviour, DamageSource source)
    {
        spawnedEnemies.Remove(behaviour.gameObject);
    }

    // ----- Boss Spawn -----
    private void SpawnBoss()
    {
        // TODO: Implement boss spawn logic
    }
}

public static class SpawningInfo
{
    public static float lastTimeDifficulty = 0;
    public static float lastLevelDifficulty = 0;
    public static float lastKillDifficulty = 0;
    public static float spawnInterval = 0;

    public static GameObject lastSpawnedZombie = null;
}
