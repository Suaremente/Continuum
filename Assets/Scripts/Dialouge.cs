using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent; // The UI Text component for displaying dialogue
    public string[] lines;               // Array of dialogue lines
    public float textSpeed;              // Speed of typing out text
    public float lineDelay = 1.0f;       // Delay before moving to the next line
    public GameObject targetObject;      // The object to activate after the dialogue ends

    private int index; // Tracks the current line index

    // Start is called before the first frame update
    public CountdownTimer countdownTimer;  // Reference to the CountdownTimer script

    void Start()
    {
        textComponent.text = string.Empty;
        if (targetObject != null)
        {
            targetObject.SetActive(false); // Ensure the object is hidden initially
        }

        if (countdownTimer != null)
        {
            countdownTimer.PauseCountdown(); // Pause the timer at the start of the dialogue
        }

        StartDialogue();
    }

    void EndDialogue()
    {
        textComponent.text = string.Empty; // Clear the text
        gameObject.SetActive(false);      // Deactivate the dialogue object

        if (targetObject != null)
        {
            targetObject.SetActive(true); // Make the target object appear
        }

        if (countdownTimer != null)
        {
            countdownTimer.ResumeCountdown(); // Resume the timer after dialogue ends
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        // After typing the line, wait before proceeding to the next one
        yield return new WaitForSeconds(lineDelay);

        // Automatically move to the next line or finish dialogue
        NextLine();
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

}
