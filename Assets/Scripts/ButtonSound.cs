using UnityEngine;

public class PlaySoundOnClick : MonoBehaviour
{
    public AudioSource audioSource;

    // This method will be called when the button is clicked
    public void PlaySound()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource is not assigned.");
        }
    }
}
