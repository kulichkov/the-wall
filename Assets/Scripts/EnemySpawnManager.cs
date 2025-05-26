using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawnManager : MonoBehaviour
{
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

    void Start()
    {
        InvokeRepeating(nameof(SpawnStandardEnemy), 2.0f, 1.0f);
        enemyPool = new ObjectPool<Enemy>
        (CreateObject, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject, collectionCheck, defaultCapacity, maxSize);
    }

    // invoked when creating an item to populate the object pool
    private Enemy CreateObject()
    {
        Enemy enemyInstance = Instantiate(standardEnemyPrefab);
        enemyInstance.gameObject.layer = LayerMask.NameToLayer("Enemy");
        enemyInstance.enemyPool = enemyPool;        
        return enemyInstance;
    }

    // Invoked when returning an item to the object pool
    private void OnReleaseToPool(Enemy pooledObject)
    {
        activeEnemies.Remove(pooledObject);
        pooledObject.gameObject.SetActive(false);
        pooledObject.Reset();
    }

    // Invoked when retrieving the next item from the object pool
    private void OnGetFromPool(Enemy pooledObject)
    {
        pooledObject.gameObject.SetActive(true);
        activeEnemies.Add(pooledObject);
    }

    // Invoked when we exceed the maximum number of pooled items (i.e. destroy the pooled object)
    private void OnDestroyPooledObject(Enemy pooledObject)
    {
        Destroy(pooledObject.gameObject);
    }

    private void SpawnStandardEnemy()
    {
        if (GameManager.Instance.IsGameOver)
            return;

        Enemy enemy = enemyPool.Get();
        enemy.transform.position = GenerateEnemyVector();
    }

    private Vector3 GenerateEnemyVector()
    {
        float xAxisValue = Random.Range(-xRange, xRange);
        return new Vector3(xAxisValue, yEnemySpawn, zEnemySpawn);
    }

    public void Clear()
    {
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
