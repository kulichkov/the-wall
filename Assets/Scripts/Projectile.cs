using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Rigidbody projectileRb { get; private set; }
    public ProjectilePool projectilePool;
    private float yBottomBound = -10;

    void Awake()
    {
        projectileRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < yBottomBound)
            Release();
    }

    public void Release()
    {
        projectilePool.Release(this);
    }

    public void Reset()
    {
        projectileRb.useGravity = false;
        projectileRb.linearVelocity = Vector3.zero;
        projectileRb.angularVelocity = Vector3.zero;
        transform.eulerAngles = Vector3.zero;
    }

    public void Throw(float force)
    {
        projectileRb.useGravity = true;
        projectileRb.AddForce(Vector3.down * force, ForceMode.Impulse);
    }
}
