using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public GameObject pauseMenu;
    public bool isPaused;
    public GameObject player; 
    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        player = GameObject.FindWithTag("Player"); 
        Damageable damageable = player.GetComponent<Damageable>();
        if (Input.GetKeyDown(KeyCode.Escape) && damageable.IsAlive)
        {

            if (isPaused)
            {
                
                resumeGame();
            }
            else
            {
                pauseGame();

            }
        }
    }

    public void pauseGame()
    {

        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

    }

    public void resumeGame()
    {

        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

    }

    public void goToMainMenu()
    {

        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
        pauseMenu.SetActive(false);

    }

    public void quitGame()
    {

        Application.Quit();
    }
}