using System.Collections;
using Unity.Hierarchy;
using UnityEngine;

public class SpawnController : MonoBehaviour {
    [SerializeField] GameObject spawnPrefab;
    [SerializeField] readonly int xSpawnPoint = 13;
    [SerializeField] int maxTimeBetweenSpawns;
    [SerializeField] int minTimeBetweenSpawns;
    [SerializeField] int maxTimeBetweenWaves;
    [SerializeField] int minTimeBetweenWaves;

    [SerializeField] readonly float height = 8.5f;

    bool isPlaying = false;
    bool isSpawning = false;

    void Start() {

    }

    void Update() {
        while(isPlaying) {
            StartCoroutine(SpawnWaveObstacles());
        }
    }

    IEnumerator SpawnWaveObstacles() {
        if (isSpawning) {
            float waitTime = Random.Range(minTimeBetweenWaves, maxTimeBetweenWaves);
            yield return new WaitForSeconds(waitTime);


            Vector2 spawnPoint = new Vector2(xSpawnPoint, Random.Range(-height, height));
            Instantiate(spawnPrefab, spawnPoint, Quaternion.identity);
        }
        
    }

}
