using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10.0f;
    public float cooldownTime = 0.5f;
    public float strength = 15.0f;
    private Rigidbody playerRb;

    private bool isAbleToShoot;

    [SerializeField] private ProjectilePool projectilePool;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        isAbleToShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.IsGameOver)
            return;
            
        // Moving
        float horizontalInput = Input.GetAxis("Horizontal");
        playerRb.AddForce(Vector3.right * speed * horizontalInput);

        // Shooting
        if (Input.GetKeyDown(KeyCode.Space))
           Shoot();
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
        if (!isAbleToShoot)
            return;

        Debug.Log("Player shoots");
        Projectile projectile = projectilePool.Get();
        if (projectile == null)
            return;

        Vector3 projectilePosition = new Vector3(transform.position.x, transform.position.y, transform.position.z - 2);
        projectile.transform.position = projectilePosition;

        projectile.Throw(strength);
        Debug.Log("Projectile velocity: " + projectile.projectileRb.linearVelocity);
        StartCoroutine(CoolDown());
    }

    IEnumerator CoolDown()
    {
        isAbleToShoot = false;
        yield return new WaitForSeconds(cooldownTime);
        isAbleToShoot = true;
    }
}
