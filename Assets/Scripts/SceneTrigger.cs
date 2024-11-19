using UnityEngine;

public class SceneTrigger : MonoBehaviour
{
    public string sceneToLoad;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<SceneTransitionManager>().LoadScene(sceneToLoad);
        }
    }
}
