using UnityEngine;

public class FallingProjectile : MonoBehaviour
{
    public float lifeTime = 5f;
    AnimationStrings animationStrings;
    Damageable damageable = new Damageable();
    void Start()
    {
        Destroy(gameObject, lifeTime); // Destroy after some time
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            damageable = collision.collider.GetComponent<Damageable>();
            damageable.Health -= 10; 
            Debug.Log("Player hit!");
        }

         // Destroy the projectile on collision
    }

    private void Awake()
    {
        damageable = GetComponent<Damageable>();
    }
}
