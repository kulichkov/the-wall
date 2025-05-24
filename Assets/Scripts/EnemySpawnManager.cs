using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField]
    private Enemy standardEnemyPrefab;

    [SerializeField]
    private Enemy strongEnemyPrefab;

    private float xRange = 22.0f;
    private float yEnemySpawn = 10.0f;
    private float zEnemySpawn = -0.5f;
    private int defaultCapacity = 5;
    private int maxSize = 10;
    private bool collectionCheck = true;
    private IObjectPool<Enemy> enemyPool;

    void Start()
    {
        InvokeRepeating(nameof(SpawnStandardEnemy), 1.0f, 4.0f);

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
        pooledObject.gameObject.SetActive(false);
        pooledObject.Reset();
    }

    // Invoked when retrieving the next item from the object pool
    private void OnGetFromPool(Enemy pooledObject)
    {
        pooledObject.gameObject.SetActive(true);
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

}
