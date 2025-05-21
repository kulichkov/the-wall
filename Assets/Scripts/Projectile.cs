using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float impulseSpeed = 5.0f;
    private Rigidbody projectileRb;
    private float yBottomBound = -10;

    void Awake()
    {
        projectileRb = GetComponent<Rigidbody>();
        if (impulseSpeed > 0.0f)
        {
            projectileRb.AddForce(Vector3.down * impulseSpeed, ForceMode.Impulse);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < yBottomBound)
        {
            Destroy(gameObject);
        }
    }
}
