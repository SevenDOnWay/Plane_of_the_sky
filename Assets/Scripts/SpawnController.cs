using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SpawnController : MonoBehaviour {

    [System.Serializable]
    public class ObstacleType {
        public GameObject prefab;
        public int poolSize = 5;
        [Range(0, 1)] public float spawnWeight = 1f;
    }

    [Header("Obstacle Settings")]
    [SerializeField] ObstacleType[] obstacleTypes;

    [Header("Spawn Settings")]
    [SerializeField] Vector2 spawnPoint = new Vector2(13, 0);
    [SerializeField] float minTimeBetweenSpawns = 1f;
    [SerializeField] float maxTimeBetweenSpawns = 2.5f;
    [SerializeField] float minTimeBetweenWaves = 5f;
    [SerializeField] float maxTimeBetweenWaves = 10f;
    [SerializeField] int minSpawn = 3;
    [SerializeField] int maxSpawn = 10;

    [SerializeField] float height = 3.9f;

    bool isSpawning = false;
    Coroutine spawnRoutine;

    Dictionary<int, List<GameObject>> obstaclePools;
    float totalWeight;

    void Awake() {
        InitializePools();
    }

    private void InitializePools() {
        obstaclePools = new Dictionary<int, List<GameObject>>();
        totalWeight = 0;

        for (int typeIndex = 0; typeIndex < obstacleTypes.Length; typeIndex++) {
            var obstacleType = obstacleTypes[typeIndex];
            var pool = new List<GameObject>();

            // Create pool for this type
            for (int i = 0; i < obstacleType.poolSize; i++) {
                GameObject obj = Instantiate(obstacleType.prefab, spawnPoint, Quaternion.identity);
                obj.SetActive(false);
                pool.Add(obj);
            }

            obstaclePools.Add(typeIndex, pool);
            totalWeight += obstacleType.spawnWeight;
        }
    }

    void Update() {
        if (!StateController.Instance.isPlaying) {
            StopSpawning();
        }
        else if (!isSpawning) {
            StartSpawning();
        }
    }

    void StartSpawning() {
        if (!isSpawning) {
            isSpawning = true;
            spawnRoutine = StartCoroutine(SpawnWaves());
        }
    }

    void StopSpawning() {
        if (isSpawning && spawnRoutine != null) {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
        }
        isSpawning = false;
    }

    IEnumerator SpawnWaves() {
        while (StateController.Instance.isPlaying && isSpawning) {
            int obstacleToSpawn = Random.Range(minSpawn, maxSpawn);
            float waitTime = Random.Range(minTimeBetweenWaves, maxTimeBetweenWaves);
            yield return new WaitForSeconds(waitTime);

            Debug.Log($"Spawning {obstacleToSpawn} obstacles after {waitTime} seconds.");

            for (int i = 0; i < obstacleToSpawn; i++) {
                yield return new WaitForSeconds(Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns));
                SpawnObstacle();
            }

        }

    }

    void SpawnObstacle() {
        GameObject obstacle = GetPooledObstacle();
        if (obstacle != null) {
            obstacle.transform.position = spawnPoint + Vector2.up * Random.Range(-height, height);
            obstacle.SetActive(true);
        }
    }

    GameObject GetPooledObstacle() {
        // Select obstacle type based on weights
        int selectedType = GetRandomObstacleType();
        if (selectedType == -1) return null;

        var pool = obstaclePools[selectedType];

        // Find inactive object in the selected pool
        foreach (var obj in pool) {
            if (!obj.activeInHierarchy) {
                return obj;
            }
        }

        return null; // Pool exhausted
    }

    int GetRandomObstacleType() {
        if (obstacleTypes.Length == 0) return -1;

        float random = Random.Range(0, totalWeight);
        float currentWeight = 0;

        for (int i = 0; i < obstacleTypes.Length; i++) {
            currentWeight += obstacleTypes[i].spawnWeight;
            if (random <= currentWeight) {
                return i;
            }
        }

        return 0;
    }

}
