using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryMenu : MonoBehaviour
{

    public GameObject pauseMenu;
    public bool isPaused;
    // Start is called before the first frame update

    public void Awake() { 
    
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
