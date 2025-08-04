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

    [Header("Boss settings")]
    [SerializeField] GameObject bossPrefab;


    [SerializeField] GameObject poolParent;


    [Header("Spawn Settings")]
    [SerializeField] float minTimeBetweenSpawns = 1f;
    [SerializeField] float maxTimeBetweenSpawns = 2.5f;
    [SerializeField] float minTimeBetweenWaves = 5f;
    [SerializeField] float maxTimeBetweenWaves = 8f;
    [SerializeField] int minSpawn = 3;
    [SerializeField] int maxSpawn = 10;


    float spawnPointX;
    float spawnPointY;
    float padding = 90f;


    bool isSpawning = false;
    Coroutine spawnRoutine;

    Dictionary<int, List<GameObject>> obstaclePools;
    float totalWeight;


    private void Start() {
        PlayAgainPannel.ClickHome += ResetObstalce;

        SetSpawnPoint();
    }



    void SetSpawnPoint() {
        spawnPointX = WorldSizeManager.Instance.worldScreenWidth;
        spawnPointY = WorldSizeManager.Instance.worldScreenHeight / 2 * padding / 100;
    }


    void Awake() {
        InitializePools();
    }

    private void InitializePools() {
        obstaclePools = new Dictionary<int, List<GameObject>>();
        totalWeight = 0;

        for ( int typeIndex = 0; typeIndex < obstacleTypes.Length; typeIndex++ ) {
            var obstacleType = obstacleTypes[typeIndex];
            var pool = new List<GameObject>();

            // Create pool for this type
            for ( int i = 0; i < obstacleType.poolSize; i++ ) {
                GameObject obj = Instantiate(obstacleType.prefab, new Vector3(99,0), Quaternion.identity, poolParent.transform);
                SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
                sr.sortingOrder = 50;

                obj.SetActive(false);
                pool.Add(obj);
            }

            obstaclePools.Add(typeIndex, pool);
            totalWeight += obstacleType.spawnWeight;
        }
    }

    void Update() {
        if ( !StateManager.Instance.isPlaying ) {
            StopSpawning();
        }
        else if ( !isSpawning ) {
            StartSpawning();
        }
    }

    void StartSpawning() {
        if ( !isSpawning ) {
            isSpawning = true;
            spawnRoutine = StartCoroutine(SpawnWaves());
        }
    }

    void StopSpawning() {
        if ( isSpawning && spawnRoutine != null ) {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
        }
        isSpawning = false;
    }

    IEnumerator SpawnWaves() {
        while ( StateManager.Instance.isPlaying && isSpawning ) {
            int obstacleToSpawn = Random.Range(minSpawn, maxSpawn);
            float waitTime = Random.Range(minTimeBetweenWaves, maxTimeBetweenWaves);
            yield return new WaitForSeconds(waitTime);

            Debug.Log($"Spawning {obstacleToSpawn} obstacles after {waitTime} seconds.");

            for ( int i = 0; i < obstacleToSpawn; i++ ) {
                while ( StateManager.Instance.isPausing ) yield return null; // Pause the spawning if the game is paused

                yield return new WaitForSeconds(Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns));
                SpawnObstacle();
            }

        }

    }

    void SpawnObstacle() {
        GameObject obstacle = GetPooledObstacle();
        if ( obstacle != null ) {
            SpriteRenderer spriteRenderer = obstacle.GetComponent<SpriteRenderer>();

            float heightSprite = spriteRenderer.bounds.extents.y;
            float height = spawnPointY - heightSprite;

            obstacle.transform.position = new Vector3(spawnPointX, Random.Range(-height, height), 0);
            obstacle.SetActive(true);
        }
    }

    GameObject GetPooledObstacle() {
        // Select obstacle type based on weights
        int selectedType = GetRandomObstacleType();
        if ( selectedType == -1 ) return null;

        var pool = obstaclePools[selectedType];

        // Find inactive object in the selected pool
        foreach ( var obj in pool ) {
            if ( !obj.activeInHierarchy ) {
                return obj;
            }
        }

        return null; // Pool exhausted
    }

    int GetRandomObstacleType() {
        if ( obstacleTypes.Length == 0 ) return -1;

        float random = Random.Range(0, totalWeight);
        float currentWeight = 0;

        for ( int i = 0; i < obstacleTypes.Length; i++ ) {
            currentWeight += obstacleTypes[i].spawnWeight;
            if ( random <= currentWeight ) {
                return i;
            }
        }

        return 0;
    }

    void ResetObstalce() {
        foreach ( var obj in obstaclePools ) {
            foreach ( var obstacle in obj.Value ) {
                obstacle.transform.position = new Vector3(99, 0);
                obstacle.SetActive(false);
            }
        }
    }
}
