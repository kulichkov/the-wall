using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class PlayerController : MonoBehaviour
{
    public float speed = 10.0f;
    public GameObject projectilePrefab;
    private Rigidbody playerRb;
    private IObjectPool<GameObject> objectPool;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
     
    // Throw an exception if we try to return an existing item, already in the pool
    [SerializeField] private bool collectionCheck = true;

    // extra options to control the pool capacity and maximum size
    [SerializeField] private int defaultCapacity = 3;
    [SerializeField] private int maxSize = 6;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    // private void Awake()
    // {
    //     // objectPool = new ObjectPool<GameObject>(CreateProjectile,
    //     //     OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject,
    //     //     collectionCheck, defaultCapacity, maxSize);
    // }

    // // invoked when creating an item to populate the object pool
    // // private GameObject CreateProjectile()
    // // {
    //     // GameObject projectileInstance = Instantiate(projectilePrefab);
    //     // projectileInstance.ObjectPool = objectPool;
    //     // return projectileInstance;
    // // }

    // // Invoked when returning an item to the object pool
    // private void OnReleaseToPool(Projectile pooledObject)
    // {
    //     pooledObject.gameObject.SetActive(false);
    // }

    // // Invoked when retrieving the next item from the object pool
    // private void OnGetFromPool(Projectile pooledObject)
    // {
    //     pooledObject.gameObject.SetActive(true);
    // }

    // // Invoked when we exceed the maximum number of pooled items (i.e. destroy the pooled object)
    // private void OnDestroyPooledObject(Projectile pooledObject)
    // {
    //     Destroy(pooledObject.gameObject);
    // }

    // Update is called once per frame
    void Update()
    {
        // Moving
        float horizontalInput = Input.GetAxis("Horizontal");
        playerRb.AddForce(Vector3.right * speed * horizontalInput);

        // Shooting
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject projectile = Instantiate(projectilePrefab);
            if (projectile == null)
                return;
            Debug.Log("Player shot at: " + Time.time);
            Vector3 projectilePosition = new Vector3(transform.position.x, projectilePrefab.transform.position.y, projectilePrefab.transform.position.z - 1);
            projectile.transform.position = projectilePosition;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Player collided Enemy");
        }  
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            Debug.Log("Player collided Powerup");
            Destroy(other.gameObject);
        }
    }
}
