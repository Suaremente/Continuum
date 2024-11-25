using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChipCount : MonoBehaviour
{
    // Chip count
    [SerializeField]
    public int ChipNum;

    private void OnEnable()
    {
        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unsubscribe from the sceneLoaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reset chip count when a new scene is loaded
        ChipNum = 0;
        Debug.Log("Chip count reset for new scene");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Chip"))
        {
            ChipNum++;
            Debug.Log("Chip collided");
            ChipCount2.instance.increaseCoins(ChipNum);
        }
    }
}
