using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Rigidbody projectileRb { get; private set; }
    public ProjectilePool projectilePool;
    public ParticleSystem particles;
    private float yBottomBound = -10;

    void Awake()
    {
        projectileRb = GetComponent<Rigidbody>();
        gameObject.layer = LayerMask.NameToLayer("Projectile");
    }

    void Update()
    {
        particles.transform.position = transform.position;

        if (transform.position.y < yBottomBound)
            Release();
    }

    public void Release()
    {
        particles.Play();
        projectilePool.Release(this);
        // projectileRb.isKinematic = true;
        // StartCoroutine(WaitAndRelease(particles.main.duration));
    }

    public void Reset()
    {
        projectileRb.isKinematic = false;
        projectileRb.useGravity = false;
        projectileRb.linearVelocity = Vector3.zero;
        projectileRb.angularVelocity = Vector3.zero;
        transform.eulerAngles = Vector3.zero;
    }

    public void Throw(float force)
    {
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

    private IEnumerator WaitAndRelease(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        projectilePool.Release(this);
    }
}
