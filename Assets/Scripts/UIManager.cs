using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject damageTextPrefab;
    public GameObject healthTextPrefab;

    public Canvas gameCanvas;
    void Start()
    {
        if (FindObjectOfType<UIManager>() == null)
        {
            Instantiate(Resources.Load<GameObject>("UIManager"));
        }
    }

    private void Awake()
    {
        if (FindObjectsOfType<UIManager>().Length > 1)
        {
            Destroy(gameObject); // Prevent duplicates
            return;
        }
 // Make this UIManager persistent
        gameCanvas = FindObjectOfType<Canvas>(); // Reassign the canvas if needed in new scenes
    }
private void OnEnable()
{
    SceneManager.sceneLoaded += OnSceneLoaded;
    CharacterEvents.characterDamaged += CharacterTookDamage;
    CharacterEvents.characterHealed += CharacterHealed;
}

private void OnDisable()
{
    SceneManager.sceneLoaded -= OnSceneLoaded;
    CharacterEvents.characterDamaged -= CharacterTookDamage;
    CharacterEvents.characterHealed -= CharacterHealed;
}

private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{
    gameCanvas = FindObjectOfType<Canvas>(); // Reassign canvas on scene load
}


    public void CharacterTookDamage(GameObject character, int damageReceived)
    {
        if (damageTextPrefab == null)
        {
            damageTextPrefab = Resources.Load<GameObject>("damageTextPrefab");
        }

        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);

        TMP_Text tmpText = Instantiate(damageTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform)
            .GetComponent<TMP_Text>();

        tmpText.text = damageReceived.ToString();
    }


    public void CharacterHealed(GameObject character, int healthRestored) {  

         if (healthTextPrefab == null)
        {
            healthTextPrefab = Resources.Load<GameObject>("healthTextPrefab");
        }

        // Create text at character hit
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);

        TMP_Text tmpText = Instantiate(healthTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform)
            .GetComponent<TMP_Text>();

        tmpText.text = healthRestored.ToString();
    }

    public void OnExitGame(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            #if (UNITY_EDITOR || DEVELOPMENT_BUILD)
                Debug.Log(this.name + " : " + this.GetType() + " : " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            #endif

            #if (UNITY_EDITOR)
                    UnityEditor.EditorApplication.isPlaying = false;
            #elif (UNITY_STANDALONE)
                    Application.Quit();
            #elif (UNITY_WEBGL)
                    SceneManager.LoadScene("QuitScene");
            #endif
        }
    }
}
