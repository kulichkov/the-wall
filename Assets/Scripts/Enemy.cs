using UnityEngine;
using UnityEngine.Pool;

public class Enemy : MonoBehaviour
{
    public float speed = 1.0f;
    public IObjectPool<Enemy> enemyPool { set => _enemyPool = value; }
    private Rigidbody enemyRb;
    private float yBottomBound = -10;
    private IObjectPool<Enemy> _enemyPool;

    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        if (transform.position.y < yBottomBound)
            _enemyPool.Release(this);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Projectile>(out var projectile))
        {
            projectile.Release();
            _enemyPool.Release(this);
        }
    }

    public void Reset()
    {
        enemyRb.useGravity = false;
        enemyRb.linearVelocity = Vector3.zero;
        enemyRb.angularVelocity = Vector3.zero;
        transform.eulerAngles = Vector3.zero;
    }
}
