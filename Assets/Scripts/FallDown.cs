using Unity.VisualScripting;
using UnityEngine;

public class FallDown : MonoBehaviour
{
    public float impulseSpeed = 0.0f;
    private Rigidbody fallingRb;
    private float yDestroy = -10;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fallingRb = GetComponent<Rigidbody>();
        fallingRb.useGravity = true;
        if (impulseSpeed > 0.0f)
        {
            fallingRb.AddForce(Vector3.down * impulseSpeed, ForceMode.Impulse);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < yDestroy)
        {
            Destroy(gameObject);
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        if (gameObject.CompareTag("Projectile") && collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Projectile collided with Enemy");
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}

