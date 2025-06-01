using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawnManager : MonoBehaviour
{
    public float spawnTime = 4.0f;
    
    [SerializeField]
    private Enemy standardEnemyPrefab;

    [SerializeField]
    private Enemy strongEnemyPrefab;

    private float xRange = 22.0f;
    [SerializeField] private float yEnemySpawn = 0.0f;
    private float zEnemySpawn = -0.5f;
    private int defaultCapacity = 5;
    private int maxSize = 10;
    private bool collectionCheck = true;
    private IObjectPool<Enemy> enemyPool;
    private List<Enemy> activeEnemies = new List<Enemy>();
    private Coroutine spawining;
    private int enemiesSpawned;
    private float curveSharpness = 0.05f;
    [SerializeField] private float minSpawnTime = 0.5f;
    [SerializeField] private float initialSpawnTime = 2.0f;

    void Start()
    {
        enemyPool = new ObjectPool<Enemy>
        (CreateObject, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject, collectionCheck, defaultCapacity, maxSize);
    }

    // invoked when creating an item to populate the object pool
    private Enemy CreateObject()
    {
        Enemy enemyInstance = Instantiate(standardEnemyPrefab);
        enemyInstance.enemyPool = enemyPool;
        return enemyInstance;
    }

    // Invoked when returning an item to the object pool
    private void OnReleaseToPool(Enemy pooledObject)
    {
        activeEnemies.Remove(pooledObject);
        pooledObject.gameObject.SetActive(false);
    }

    // Invoked when retrieving the next item from the object pool
    private void OnGetFromPool(Enemy pooledObject)
    {
        pooledObject.gameObject.SetActive(true);
        activeEnemies.Add(pooledObject);
        pooledObject.Reset();
    }

    // Invoked when we exceed the maximum number of pooled items (i.e. destroy the pooled object)
    private void OnDestroyPooledObject(Enemy pooledObject)
    {
        Destroy(pooledObject.gameObject);
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnTime);
        }
    }

    private void SpawnEnemy()
    {
        Enemy enemy = enemyPool.Get();
        enemy.transform.position = GenerateEnemyVector();
        enemiesSpawned += 1;
        HandleSpawnTime();
    }

    private Vector3 GenerateEnemyVector()
    {
        float xAxisValue = UnityEngine.Random.Range(-xRange, xRange);
        return new Vector3(xAxisValue, yEnemySpawn, zEnemySpawn);
    }

    private void HandleSpawnTime()
    {
        var newSpawnTime = initialSpawnTime / (1 + curveSharpness * enemiesSpawned);
        spawnTime = Math.Clamp(newSpawnTime, minSpawnTime, initialSpawnTime);
    }

    public void StartSpawning()
    {
        StopSpawning();
        spawining = StartCoroutine(SpawnEnemies());
        Debug.Log("Spawning started");
    }

    public void StopSpawning()
    {
        if (spawining == null)
            return;
        StopCoroutine(spawining);
        foreach (var enemy in activeEnemies)
        {
            enemy.StopClimbing();
        }
    }

    public void Clear()
    {
        enemiesSpawned = 0;
        spawnTime = initialSpawnTime;

        if (activeEnemies.Count == 0)
            return;

        int lastIndex = activeEnemies.Count - 1;
        int firstIndex = 0;

        Debug.Log($"lastIndex: {lastIndex}, firstIndex: {firstIndex}");
        for (int i = lastIndex; i >= firstIndex; i--)
        {
            Debug.Log($"i: {i}, activeEnemies.Count: {activeEnemies.Count}");
            var enemy = activeEnemies[i];
            activeEnemies.Remove(enemy);
            enemyPool.Release(enemy);
        }

        enemyPool.Clear();
    }
}
