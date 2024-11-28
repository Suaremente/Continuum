using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryMenu : MonoBehaviour
{

    public GameObject pauseMenu; 
    public bool isPaused;
    public GameObject player; 

    // Start is called before the first frame update 
    public void Awake()
    {
        
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
        player = GameObject.FindWithTag("Player");
        Damageable damageable = player.GetComponent<Damageable>();
        damageable.IsAlive = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");

    }

    public void quitGame()
    {

        Application.Quit();
    }

    public void retryGame() {
        player = GameObject.FindWithTag("Player");
        Damageable damageable = player.GetComponent<Damageable>();
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.name);
        damageable.IsAlive = true; 
        pauseMenu.SetActive(false); 

    }

    public void Update()
    {
        player = GameObject.FindWithTag("Player");
        Damageable damageable = player.GetComponent<Damageable>();
        if (damageable.Health == 0) {
            damageable.Health = 100;
            pauseGame();
           
        }
    }
}
