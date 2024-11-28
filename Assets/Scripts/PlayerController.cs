using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody2D), typeof(TouchingSpaceDirections), typeof(Damageable))]
public class PlayerController : MonoBehaviour
{

    public PlayerImport playerData;
    Rigidbody2D rb;
    Animator animator;
    Vector2 moveInput;
    TouchingSpaceDirections touchingDirections;
    Damageable damageable;
    PauseMenu pauseMenu;
    private bool bufferedHeavyAttack = false;
    private static PlayerController instance;   

    [SerializeField]
    public float airWalkSpeed = 5f;

    [SerializeField]
    public float walkSpeed = 5f;

    [SerializeField]
    private float jumpImpulse = 10f;

    [SerializeField]
    private int maxJumpCount = 2;

    private int jumpCount = 0;

    [SerializeField]
    private float dashSpeed = 10f;  // Speed during dash
    [SerializeField]
    private float dashDuration = 0.2f;  // Duration of the dash
    [SerializeField]
    private float dashCooldown = 1f;  // Cooldown duration for dashing
    private bool isDashing = false;
    private float lastDashTime = -Mathf.Infinity;  // Timestamp of last dash

    public float CurrentMoveSpeed
    {
        get
        {
            if (IsMoving && !touchingDirections.IsOnWall)
            {
                if (touchingDirections.IsGrounded)
                {
                    return walkSpeed;
                }
                else
                {
                    return airWalkSpeed;
                }
            }
            return 0;
        }
    }

    [SerializeField]
    private bool _isMoving = true;
    public bool IsMoving
    {
        get
        {
            return _isMoving;
        }

        private set
        {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        }
    }

    public bool _isFacingRight = true;
    public bool IsFacingRight
    {
        get { return _isFacingRight; }
        private set
        {
            if (_isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;
        }
    }
    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }

    }

    public bool IsAlive
    {
        get
        {
            return animator.GetBool(AnimationStrings.isAlive);
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingSpaceDirections>();
        damageable = GetComponent<Damageable>();
        pauseMenu = GetComponent<PauseMenu>();

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist this instance across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
            return;
        }
    }

    private void FixedUpdate()
    {
        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);

        if (touchingDirections.IsGrounded)
        {
            jumpCount = 0;

            // Execute buffered heavy attack on ground, if cooldown allows
            if (bufferedHeavyAttack && Time.time >= lastHeavyAttackTime + heavyAttackCooldown)
            {
                animator.SetTrigger(AnimationStrings.heavyAttackTrigger);
                lastHeavyAttackTime = Time.time;
                bufferedHeavyAttack = false;
            }
        }

        if (CanMove && !isDashing)
            rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
        else if (isDashing)
            rb.velocity = new Vector2(moveInput.x * dashSpeed, rb.velocity.y);
        else
            rb.velocity = new Vector2(0, 0);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (IsAlive)
        {

            IsMoving = moveInput != Vector2.zero;

            SetFacingDirection(moveInput);
        }
        else IsMoving = false;
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && (touchingDirections.IsGrounded || jumpCount < maxJumpCount))
        {
            animator.SetTrigger(AnimationStrings.jumpTrigger);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
            jumpCount++;
        }
    }

    public void OnRangedAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.rangedAttackTrigger);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.attackTrigger);
        }
    }

    [SerializeField]
    private float heavyAttackCooldown = 2f;  // Cooldown duration in seconds

    private float lastHeavyAttackTime = -Mathf.Infinity;  // Tracks time of last heavy attack

    public void OnHeavyAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (Time.time >= lastHeavyAttackTime + heavyAttackCooldown && touchingDirections.IsGrounded)
            {
                // Grounded heavy attack if cooldown allows, perform immediately
                animator.SetTrigger(AnimationStrings.heavyAttackTrigger);
                lastHeavyAttackTime = Time.time;
                bufferedHeavyAttack = false;  // Clear buffer
            }
            else if (!touchingDirections.IsGrounded)
            {
                // Buffer heavy attack if attempted in air
                bufferedHeavyAttack = false;
            }
        }
    }

    public void OnBlock(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.isBlocking);
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

    public void OnDeath()
    {
        Time.timeScale = 0f;
        pauseMenu.goToMainMenu();
    }

    // Dash logic
    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started && !isDashing && Time.time >= lastDashTime + dashCooldown && !touchingDirections.IsGrounded)
        {
            isDashing = true;
            lastDashTime = Time.time;

            // Perform the dash for the set duration
            StartCoroutine(DashRoutine());
        }
    }

    private IEnumerator DashRoutine()
    { // Play dash animation
        yield return new WaitForSeconds(dashDuration);  // Wait for dash duration
        isDashing = false;
    }
}
