using System.Collections;
using UnityEngine;
using TMPro;  // Import TextMeshPro namespace
using UnityEngine.Events;

public class CountdownTimer : MonoBehaviour
{
    public float startTime = 10f;  // Time in seconds for countdown
    private float currentTime;

    public TextMeshProUGUI countdownText;  // Reference to a TextMeshProUGUI element
    public UnityEvent onTimerEnd;  // Event to trigger when countdown reaches zero

    private bool isRunning = false;

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
        if (isRunning)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                currentTime = 0;
                isRunning = false;
                TimerEnd();
            }
            UpdateTimerDisplay();
        }
    }

    // Starts the countdown
    public void StartCountdown()
    {
        isRunning = true;
    }

    // Pauses the countdown
    public void PauseCountdown()
    {
        isRunning = false;
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
        onTimerEnd?.Invoke(); // Trigger any event on timer end

        // Kill the player by setting health to zero
        if (playerDamageable != null)
        {
            playerDamageable.Health = 0;  // This triggers death in the Damageable script
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
