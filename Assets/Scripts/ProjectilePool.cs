using UnityEngine;
using UnityEngine.Pool;

public class ProjectilePool : MonoBehaviour
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private ParticleSystem particlesPrefab;
    [SerializeField] int defaultCapacity = 5;
    [SerializeField] int maxSize = 8;
    [SerializeField] bool collectionCheck = true;
    private IObjectPool<Projectile> projectilePool;

    void Start()
    {
        // projectileParticlesPool = new ProjectileParticlesPool();
        projectilePool = new ObjectPool<Projectile>
        (CreateProjectile, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject, collectionCheck, defaultCapacity, maxSize);

        Debug.Log("Try to warm-up the pool");
        projectilePool.Get().Release();
    }

    public Projectile Get()
    {
        return projectilePool.Get();
    }
    public void Release(Projectile projectile)
    {
        projectilePool.Release(projectile);
    }

    // invoked when creating an item to populate the object pool
    private Projectile CreateProjectile()
    {
        Projectile projectileInstance = Instantiate(projectilePrefab);
        projectileInstance.particles = Instantiate(particlesPrefab);
        projectileInstance.projectilePool = this;
        return projectileInstance;
    }

    // Invoked when returning an item to the object pool
    private void OnReleaseToPool(Projectile pooledObject)
    {
        pooledObject.gameObject.SetActive(false);
    }

    // Invoked when retrieving the next item from the object pool
    private void OnGetFromPool(Projectile pooledObject)
    {
        pooledObject.Reset();
        pooledObject.gameObject.SetActive(true);
    }

    // Invoked when we exceed the maximum number of pooled items (i.e. destroy the pooled object)
    private void OnDestroyPooledObject(Projectile pooledObject)
    {
        Destroy(pooledObject.gameObject);
    }
}
