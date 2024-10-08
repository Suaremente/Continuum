using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; 

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingSpaceDirections))]
public class PlayerController : MonoBehaviour
{

    Rigidbody2D rb;
    Animator animator;
    Vector2 moveInput;
    TouchingSpaceDirections touchingDirections;

    public float walkSpeed = 5f;
    private float jumpImpulse = 10f;

    public float CurrentMoveSpeed
    {
        get
        {
            if (IsMoving && !touchingDirections.IsOnWall) return walkSpeed; return 0;
        }
    } 


    [SerializeField]
    private bool _isMoving = true;
    public bool IsMoving { get {

        return _isMoving; 
    }
    
    private set {

        _isMoving = value; 
        animator.SetBool(AnimationStrings.isMoving, value); 
    }
    }
    
    public bool _isFacingRight = true; 
    public bool IsFacingRight { get { return _isFacingRight; }
    private set{
        if(_isFacingRight != value){
            transform.localScale *= new Vector2(-1, 1); 
        }
        _isFacingRight = value; 
    } }

    private void Awake() 
    {
        rb = GetComponent<Rigidbody2D>(); 
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingSpaceDirections>();
    }
   
    private void FixedUpdate() 
    {
        rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
    }
    public void OnMove(InputAction.CallbackContext context) 
    {
        moveInput = context.ReadValue<Vector2>(); 

        IsMoving = moveInput != Vector2.zero; 

        SetFacingDirection(moveInput);
    }

    private void SetFacingDirection(Vector2 moveInput) {

        if(moveInput.x > 0 && !IsFacingRight) {
            IsFacingRight = true; 
        }

        else if(moveInput.x < 0 && IsFacingRight) {
            IsFacingRight = false; 
        }
    }

    public void OnJump(InputAction.CallbackContext context) {

        if (context.started && touchingDirections.IsGrounded)
        {
            animator.SetTrigger(AnimationStrings.jump);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
        }
    }

}
