using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D), typeof(TouchingSpaceDirections))]
public class PastGrunt : MonoBehaviour
{

    public float walkSpeed = 3f;
    public float walkStopRate = .02f;
    public DetectionZone attackZone;
    public DetectionZone cliffDetectionZone; 


    Rigidbody2D rb;
    TouchingSpaceDirections touchingDirections;
    private Animator animator;

    public enum WalkableDirection { Left, Right } 

    private WalkableDirection _walkDirection;
    private Vector2 walkDirectionVector = Vector2.right;

    public WalkableDirection WalkDirection { 
    
        get { return _walkDirection; }  
        set {

            if (_walkDirection != value)
            {
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);

                if (value == WalkableDirection.Right)
                {
                    walkDirectionVector = Vector2.left;

                }
                else if (value == WalkableDirection.Left) {

                    walkDirectionVector = Vector2.right; 
                } 
                
            }
            _walkDirection = value;
        }
    }

    public bool _hasTarget = false; 
    public bool HasTarget { get { return _hasTarget; } private set
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
        }

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }

    }
    public float AttackCooldown
    {
        get
        {
            return animator.GetFloat(AnimationStrings.attackCooldown);
        }
        private set
        {
            animator.SetFloat(AnimationStrings.attackCooldown, Mathf.Max(value, 0));
        }
    }

    private void Awake() {

        rb = GetComponent<Rigidbody2D>(); 
        touchingDirections = GetComponent<TouchingSpaceDirections>();
        animator = GetComponent<Animator>();    
    }

    private void FixedUpdate() {

        if (touchingDirections.IsOnWall && touchingDirections.IsGrounded) {
            FlipDirection(); 
        }
        if (CanMove)
            rb.velocity = new Vector2(walkSpeed * walkDirectionVector.x, rb.velocity.y);

        else rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
    }

    // Update is called once per frame
    void Update()
    {
        HasTarget = attackZone.detectedColliders.Count > 0;

        if (AttackCooldown > 0)
        {
            AttackCooldown -= Time.deltaTime;
        }
    }

    private void FlipDirection()
    {
        if (WalkDirection == WalkableDirection.Right)
        {
            WalkDirection = WalkableDirection.Left;

        }
        else if (WalkDirection == WalkableDirection.Left)
        {
            WalkDirection = WalkableDirection.Right;
        }
     
    }
    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);

    }

    public void OnCliffDectected() { 
        if(touchingDirections.IsGrounded) FlipDirection();
    }
}
