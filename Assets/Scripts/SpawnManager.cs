using UnityEngine;
using UnityEngine.Pool;

public class SpawnManager : MonoBehaviour
{
    public GameObject powerupPrefab;
    public GameObject projectilePrefab;
    
    private float xRange = 22.0f;
    private float yProjectileSpawn = 25.5f;
    private float zProjectileSpawn = 0.43f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // InvokeRepeating(nameof(SpawnProjectile), 2.0f, 3.0f);
        // InvokeRepeating(nameof(SpawnPowerup), 2.0f, 3.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnProjectile()
    {
        Instantiate(projectilePrefab, GenerateProjectileVector(), projectilePrefab.transform.rotation);
    }

    private void SpawnPowerup()
    {
        Instantiate(powerupPrefab, GeneratePowerupVector(), powerupPrefab.transform.rotation);
    }
    private Vector3 GenerateProjectileVector()
    {
        float xAxisValue = Random.Range(-xRange, xRange);
        return new Vector3(xAxisValue, yProjectileSpawn, zProjectileSpawn);
    }

    private Vector3 GeneratePowerupVector()
    {
        float xAxisValue = Random.Range(-xRange, xRange);
        return new Vector3(xAxisValue, yProjectileSpawn, zProjectileSpawn);
    }
}
