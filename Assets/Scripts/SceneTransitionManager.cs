using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);

        // Reposition player after the new scene loads
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Find spawn point in the new scene
        GameObject spawnPoint = GameObject.FindWithTag("PlayerSpawnPoint");
        if (spawnPoint != null && FindObjectOfType<PlayerController>() != null)
        {
            FindObjectOfType<PlayerController>().transform.position = spawnPoint.transform.position;
        }

        // Unsubscribe from event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
