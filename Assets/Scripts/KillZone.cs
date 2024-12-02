using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            PlayerDeath(other.gameObject);
        }
    }

    private void PlayerDeath(GameObject player)
    {
     
        Damageable damageable = player.GetComponent<Damageable>();
        if (damageable != null)
        {
            damageable.Health = 0; 
        }

        Debug.Log("Player fell off the stage and died!");
    }
}
