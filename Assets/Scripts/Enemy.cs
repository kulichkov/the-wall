using UnityEngine;
using UnityEngine.Pool;

public class Enemy : MonoBehaviour
{
    public float speed = 1.0f;
    public IObjectPool<Enemy> enemyPool { set => _enemyPool = value; }
    private Rigidbody enemyRb;
    private float yBottomBound = -10;
    private float yTopBound = 24;
    private IObjectPool<Enemy> _enemyPool;
    private Animator animator;
    private bool isDead;
    [SerializeField] private ParticleSystem blood;

    void Awake()
    {
        enemyRb = GetComponent<Rigidbody>();
        animator = transform.GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (!GameManager.Instance.IsGameOver)
            transform.Translate(Vector3.up * speed * Time.deltaTime);

        if (transform.position.y < yBottomBound)
        {
            _enemyPool.Release(this);
        }
        else if (transform.position.y >= yTopBound)
        {
            GameManager.Instance.EndGame();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isDead && collision.gameObject.TryGetComponent<Projectile>(out var projectile))
        {
            projectile.Release();
            // _enemyPool.Release(this);
            // DisablePhysics();
            enemyRb.useGravity = true;
            animator.SetBool("Grounded", false);
            blood.Play();
            isDead = true;
            GameManager.Instance.AddScore(1);
        }
        // else if (collision.gameObject.TryGetComponent<PlayerController>(out var player))
        // {
        //     DisablePhysics();
        //     GameManager.Instance.EndGame();
        // }
    }

    public void Reset()
    {
        // EnablePhysics();
        blood.Stop();
        isDead = false;
        animator.Rebind();
        animator.Update(0f);
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
