using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class PlayerController : MonoBehaviour
{
    public float speed = 10.0f;
    public float strength = 15.0f;
    private Rigidbody playerRb;

    [SerializeField] private ProjectilePool projectilePool;
    
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Moving
        float horizontalInput = Input.GetAxis("Horizontal");
        playerRb.AddForce(Vector3.right * speed * horizontalInput);

        // Shooting
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
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

    private void Shoot()
    {
        Debug.Log("Player shoots");
        Projectile projectile = projectilePool.Get();
        if (projectile == null)
            return;

        Vector3 projectilePosition = new Vector3(transform.position.x, transform.position.y, transform.position.z - 2);
        projectile.transform.position = projectilePosition;

        projectile.AddImpulse(strength);
        Debug.Log("Projectile velocity: " + projectile.projectileRb.linearVelocity);
    }
}
