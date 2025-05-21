using UnityEngine;

public class MoveUp : MonoBehaviour
{
    public float speed = 1.0f;
    private Rigidbody enemyRb;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
}
