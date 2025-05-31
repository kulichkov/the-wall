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
        gameObject.layer = LayerMask.NameToLayer("Characters");
        enemyRb = GetComponent<Rigidbody>();
        animator = transform.GetComponentInChildren<Animator>();
        animator.enabled = true;
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
            enemyRb.useGravity = true;

            int deathType = Random.Range(0, 2);

            animator.SetBool("Climb_b", false);
            if (deathType == 0)
            {
                animator.SetBool("Grounded", false);
                if (collision.contacts.Length > 0)
                {
                    var point = collision.contacts[0].point;
                    blood.transform.position = point;
                }
            }
            else if (deathType == 1)
            {
                animator.SetBool("Death_b", true);
                animator.SetInteger("DeathType_int", 3);
                blood.transform.position = transform.position;
            }
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

    public void StopClimbing()
    {
        animator.enabled = false;
    }

    public void Reset()
    {
        enemyRb.useGravity = false;
        enemyRb.linearVelocity = Vector3.zero;
        blood.Stop();
        isDead = false;
        animator.Rebind();
        animator.Update(0.0f);
        animator.SetBool("Death_b", false);
        animator.SetBool("Grounded", true);
        animator.SetBool("Climb_b", true);
    }
}
