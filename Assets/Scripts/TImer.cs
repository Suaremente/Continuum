using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System;

public class CountdownTimer : MonoBehaviour
{
    public float startTime = 10f;  // Time in seconds for countdown
    private float currentTime;

    public TextMeshProUGUI countdownText;  // Reference to a TextMeshProUGUI element
    public UnityEvent onTimerEnd;  // Event to trigger when countdown reaches zero

    private bool isRunning = false;
    private bool isPaused = false;  // New flag to handle pausing

    [SerializeField]
    private Damageable playerDamageable;  // Reference to the player's Damageable component

    void Start()
    {
        currentTime = startTime;
        UpdateTimerDisplay();
        StartCountdown();
    }

    void Update()
    {
        if (isRunning && !isPaused)
        {
            // Check if player is still alive
            if (playerDamageable != null && playerDamageable.Health == 0f)
            {
                isRunning = false;
                Debug.Log("Player has died. Stopping the timer.");
                return;
            }

            // Countdown timer
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                currentTime = 0;
                isRunning = false;
                TimerEnd();
                return;
            }
            UpdateTimerDisplay();
        }
    }

    // Starts the countdown
    public void StartCountdown()
    {
        isRunning = true;
        isPaused = false;
    }

    // Pauses the countdown
    public void PauseCountdown()
    {
        isPaused = true;
       
    }

    // Resumes the countdown
    public void ResumeCountdown()
    {
        isPaused = false;
        
    }

    // Resets the countdown to the original time
    public void ResetCountdown()
    {
        currentTime = startTime;
        UpdateTimerDisplay();
    }

    // What happens when the timer reaches 0
    private void TimerEnd()
    {
        Debug.Log("Countdown Ended!");
        // Trigger any event on timer end

        // Kill the player by setting health to zero
        if (playerDamageable != null)
        {
            playerDamageable.Health = 0;
            onTimerEnd?.Invoke();
        }
        else
        {
            Debug.LogWarning("Player Damageable component is not assigned!");
        }
    }

    // Updates the displayed time on the UI
    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);  // Update TextMeshPro text
    }
}
