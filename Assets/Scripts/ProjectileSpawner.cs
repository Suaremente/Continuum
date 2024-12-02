using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform[] spawnPoints; // Positions from where projectiles fall
    public float spawnInterval = 2f;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnProjectile), 0f, spawnInterval);
    }

    void SpawnProjectile()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(projectilePrefab, spawnPoints[randomIndex].position, Quaternion.identity);
    }

    public void StopSpawning()
    {
        CancelInvoke(nameof(SpawnProjectile));
    }
}
