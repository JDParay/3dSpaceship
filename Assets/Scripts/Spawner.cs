using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject asteroidPrefab;
    public Transform ship;

    [Header("Spawn Rate")]
    public float spawnInterval = 0.5f; 
    public float spawnDistanceAhead = 60f;
    
    [Header("Cluster Density")]
    public int minPerSpawn = 1;
    public int maxPerSpawn = 3;

    [Header("Range Settings")]
    public float horizontalRange = 15f;
    public float verticalRange = 5f;

    [Header("Size Settings")]
    public float minScale = 0.5f;
    public float maxScale = 3.5f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            int spawnCount = Random.Range(minPerSpawn, maxPerSpawn + 1);

            for (int i = 0; i < spawnCount; i++)
            {
                SpawnAsteroid();
            }
            timer = 0f;
        }
    }

    void SpawnAsteroid()
    {
        Vector3 spawnPos = ship.position + 
                          (ship.forward * spawnDistanceAhead) + 
                          (ship.right * Random.Range(-horizontalRange, horizontalRange)) + 
                          (ship.up * Random.Range(-verticalRange, verticalRange));

        GameObject asteroid = Instantiate(asteroidPrefab, spawnPos, Random.rotation);

        float randomScale = Random.Range(minScale, maxScale);
        asteroid.transform.localScale = Vector3.one * randomScale;
    }
}