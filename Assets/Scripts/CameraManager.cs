using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private void Awake()
    {
        // Ensure only one instance exists
        if (FindObjectsOfType<CameraManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject); // Persist across scenes
    }
}
