using UnityEngine;
using UnityEngine.Pool;

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
        {
            Release();
        }
    }

    public void Release()
    {
        projectilePool.Release(this);
    }

    public void AddImpulse(float force)
    {
        projectileRb.AddForce(Vector3.down * force, ForceMode.Impulse);
    }
}
