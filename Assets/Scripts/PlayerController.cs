using System;
using System.Collections;
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
    private float horizontalInput { get => Input.GetAxis("Horizontal"); }

    void Start()
    {
        Debug.Log("transform.position " + transform.position);
        startPosition = transform.position;
        playerRb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        animator.SetBool("Static_b", true);
        animator.SetInteger("DeathType_int", 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.IsGameOver)
            return;
            
        var absHorizontalInput = Math.Abs(horizontalInput);
        animator.SetFloat("Speed_f", absHorizontalInput);
        transform.rotation = Quaternion.Euler(0, 180 - 90 * horizontalInput, 0);

        // Shooting
        if (Input.GetKeyDown(KeyCode.Space))
            Shoot();
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.IsGameOver)
            return;

        // Moving
        playerRb.AddForce(Vector3.right * speed * horizontalInput);
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

    public void SetDead()
    {
        StopMoving();
        animator.SetBool("Death_b", true);
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
        animator.SetFloat("Speed_f", 0);
        playerRb.linearVelocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;
    }
    public void Reset()
    {
        transform.position = startPosition;
        animator.Rebind();
        animator.Update(0f);
    }
}
