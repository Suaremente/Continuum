using UnityEngine;

public class PlayerBlock : MonoBehaviour
{
    public bool isBlocking = false;
    public float blockDuration = 1.0f; // How long the block lasts
    public float blockCooldown = 2.0f; // Time before the player can block again
    private float blockTimer;
    private float cooldownTimer;

    private Animator animator; // Assuming there's an Animator component for animations

    void Start()
    {
        animator = GetComponent<Animator>();
        blockTimer = 0;
        cooldownTimer = 0;
    }

    void Update()
    {
        HandleBlockInput();
    }

    void HandleBlockInput()
    {
        // Block button (e.g., left shift or other input)
        if (Input.GetKeyDown(KeyCode.LeftShift) && cooldownTimer <= 0)
        {
            isBlocking = true;
            blockTimer = blockDuration;
            cooldownTimer = blockCooldown;

            // Trigger block animation
            animator.SetBool("isBlocking", true);
        }

        if (isBlocking)
        {
            blockTimer -= Time.deltaTime;

            if (blockTimer <= 0)
            {
                EndBlock();
            }
        }

        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    void EndBlock()
    {
        isBlocking = false;
        animator.SetBool("isBlocking", false);
    }

    // Call this function in your damage script to check if the player is blocking
    public bool IsBlocking()
    {
        return isBlocking;
    }
}
