using UnityEngine;
using UnityEngine.Pool;

public class ProjectileParticlesPool : MonoBehaviour
{
    [SerializeField] private ProjectileParticleSystem projectileParticlesPrefab;
    [SerializeField] int defaultCapacity = 5;
    [SerializeField] int maxSize = 8;
    [SerializeField] bool collectionCheck = true;
    private IObjectPool<ProjectileParticleSystem> projectileParticlesPool;
    
    void Start()
    {
        projectileParticlesPool = new ObjectPool<ProjectileParticleSystem>
        (CreateProjectile, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject, collectionCheck, defaultCapacity, maxSize);
    }

    public ProjectileParticleSystem Get()
    {
        return projectileParticlesPool.Get();
    }
    public void Release(ProjectileParticleSystem particleSystem)
    {
        projectileParticlesPool.Release(particleSystem);
    }

    // invoked when creating an item to populate the object pool
    private ProjectileParticleSystem CreateProjectile()
    {
        ProjectileParticleSystem particleSystemInstance = Instantiate(projectileParticlesPrefab);
        particleSystemInstance.projectileParticlesPool = this;
        return particleSystemInstance;
    }

    // Invoked when returning an item to the object pool
    private void OnReleaseToPool(ProjectileParticleSystem pooledObject)
    {
        pooledObject.gameObject.SetActive(false);
    }

    // Invoked when retrieving the next item from the object pool
    private void OnGetFromPool(ProjectileParticleSystem pooledObject)
    {
        pooledObject.gameObject.SetActive(true);
    }

    // Invoked when we exceed the maximum number of pooled items (i.e. destroy the pooled object)
    private void OnDestroyPooledObject(ProjectileParticleSystem pooledObject)
    {
        Destroy(pooledObject.gameObject);
    }
}
