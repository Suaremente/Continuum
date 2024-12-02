using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerDeath(other.gameObject);
        }
    }

    private void PlayerDeath(GameObject player)
    {
        // Example: If your player has a Damageable script
        Damageable damageable = player.GetComponent<Damageable>();
        if (damageable != null)
        {
            damageable.Health = 0; // Kill the player
        }

        // Or restart the level
        // UnityEngine.SceneManagement.SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        Debug.Log("Player fell off the stage and died!");
    }
}
