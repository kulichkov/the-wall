using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 15.0f;
    [SerializeField] private float cooldownTime = 0.5f;
    [SerializeField] private float strength = 15.0f;
    [SerializeField] private ProjectilePool projectilePool;
    private Rigidbody playerRb;
    private bool isAbleToShoot = true;
    private Vector3 startPosition;
    private Animator animator;

    void Start()
    {
        Debug.Log("transform.position " + transform.position);
        startPosition = transform.position;
        playerRb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.IsGameOver)
            return;

        // Moving
        float horizontalInput = Input.GetAxis("Horizontal");
        playerRb.AddForce(Vector3.right * speed * horizontalInput);

        var absHorizontalInput = Math.Abs(horizontalInput);

        animator.SetFloat("Speed_f", absHorizontalInput);
        transform.rotation = Quaternion.Euler(0, 180 - 90 * horizontalInput, 0);

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

    private void Run(bool isRunning)
    {
        // Run
        // Static_b = false
        // Speed_f > 0.5

        // Idle
        // Speed_f < 0.25
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

    private IEnumerator CoolDown()
    {
        isAbleToShoot = false;
        yield return new WaitForSeconds(cooldownTime);
        isAbleToShoot = true;
    }

    public void StopMoving()
    {
        playerRb.linearVelocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;
    }
    public void Reset()
    {
        transform.position = startPosition;
    }
}
