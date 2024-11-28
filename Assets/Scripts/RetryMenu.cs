using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryMenu : MonoBehaviour
{

    public GameObject pauseMenu;
    public GameObject Obj; 
    public bool isPaused;
    // Start is called before the first frame update
    public RetryMenu instance; 
    public void Awake()
    {
        if (instance == null)
        {
            instance = this; // Assign this instance
            DontDestroyOnLoad(Obj); // Persist this object across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicates
            return;
        }

        // Initialize pause menu
        pauseMenu.SetActive(false);
    }
    public void callRetry() {

        pauseGame(); 
    }

    public void pauseGame()
    {

        pauseMenu.SetActive(true);
        isPaused = true;

    }

    public void goToMainMenu()
    {
     
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");

    }

    public void quitGame()
    {

        Application.Quit();
    }

    public void retryGame() {

        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.name);

    } 

}
