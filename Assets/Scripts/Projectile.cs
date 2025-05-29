using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Rigidbody projectileRb { get; private set; }
    public ProjectilePool projectilePool;
    public ProjectileParticleSystem projectileParticleSystem;
    private float yBottomBound = -10;

    void Awake()
    {
        projectileRb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (transform.position.y < yBottomBound)
            Release();

        SetParticlesPosition();
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
        SetParticlesPosition();
        projectileRb.useGravity = true;
        projectileRb.AddForce(Vector3.down * force, ForceMode.Impulse);
        projectileRb.AddTorque(GetTorqueVector(), ForceMode.Impulse);
    }

    private Vector3 GetTorqueVector()
    {
        return new Vector3(GetRandomTorqueValue(), GetRandomTorqueValue(), GetRandomTorqueValue());
    }

    private float GetRandomTorqueValue()
    {
        return UnityEngine.Random.Range(-1.0f, 1.0f);
    }

    private void SetParticlesPosition()
    {
        if (projectileParticleSystem == null)
            return;

        projectileParticleSystem.transform.position = transform.position;
    }
}
