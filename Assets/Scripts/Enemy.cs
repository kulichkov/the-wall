using System;
using System.Drawing;
using UnityEngine;
using UnityEngine.Pool;

public class Enemy : MonoBehaviour
{
    public float speed = 1.0f;
    public IObjectPool<Enemy> enemyPool { set => _enemyPool = value; }
    private Rigidbody enemyRb;
    private float yBottomBound = -10;
    private IObjectPool<Enemy> _enemyPool;
    
    void Awake()
    {
        enemyRb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!GameManager.Instance.IsGameOver)
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
            DisablePhysics();
            GameManager.Instance.AddScore(1);
        }
        else if (collision.gameObject.TryGetComponent<PlayerController>(out var player))
        {
            DisablePhysics();
            GameManager.Instance.EndGame();
        }
    }

    public void Reset()
    {
        EnablePhysics();
        enemyRb.useGravity = false;
        enemyRb.linearVelocity = Vector3.zero;
    }

    private void DisablePhysics()
    {
        enemyRb.isKinematic = true;
    }

    private void EnablePhysics()
    {
        enemyRb.isKinematic = false;
    }
}
