using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingSpaceDirections), typeof(Damageable))]
public class PlayerController : MonoBehaviour
{

    Rigidbody2D rb;
    Animator animator;
    Vector2 moveInput;
    TouchingSpaceDirections touchingDirections;
    Damageable damageable;

    [SerializeField]
    public float airWalkSpeed = 5f;

    [SerializeField]
    public float walkSpeed = 5f;

    [SerializeField]
    private float jumpImpulse = 10f;

    [SerializeField]
    private int maxJumpCount = 2;  

    private int jumpCount = 0;  

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

    public bool IsAlive {

        get {

            return animator.GetBool(AnimationStrings.isAlive); 
        }
    
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingSpaceDirections>();
        damageable = GetComponent<Damageable>();
    }

    private void FixedUpdate()
    {
        
        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);

      
        if (touchingDirections.IsGrounded)
        {
            jumpCount = 0;  
        }

        if (CanMove)
            rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);

        else rb.velocity = new Vector2(0, 0);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (IsAlive) {

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

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.attackTrigger);
        }
    }

    public void OnBlock(InputAction.CallbackContext context)
    {
        if (context.started) { 
            animator.SetTrigger(AnimationStrings.isBlocking);
        }
    }

    public void OnHit(int damage, Vector2 knockback) 
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
          
    }
}
