using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingSpaceDirections), typeof(Damageable))]
public class BikeBoss : MonoBehaviour
{
    public float walkSpeed = 3f;
    public float followSpeed = 5f; // Speed when following the player
    public float walkStopRate = 0.02f;
    public DetectionZone attackZone;
    public DetectionZone cliffDetectionZone;
    public DetectionZone attackingZone;
    public Transform player; // Reference to the player's transform

    private Rigidbody2D rb;
    private TouchingSpaceDirections touchingDirections;
    private Animator animator;
    private Damageable damageable;
    public FloatingHealthBar healthBar;

    public enum WalkableDirection { Left, Right }

    private WalkableDirection _walkDirection;
    private Vector2 walkDirectionVector = Vector2.right;

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player object not found! Ensure the Player has the tag 'Player'.");
        }
    }
    public WalkableDirection WalkDirection
    {
        get { return _walkDirection; }
        set
        {
            if (_walkDirection != value)
            {
                _walkDirection = value;

                // Flip the character to face the correct direction
                if (_walkDirection == WalkableDirection.Right)
                {
                    walkDirectionVector = Vector2.right;
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
                else
                {
                    walkDirectionVector = Vector2.left;
                    transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
            }
        }
    }

    public bool _hasTarget = false;
    public bool HasTarget
    {
        get { return _hasTarget; }
        private set
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }

    public bool _attZone = false;
    public bool attZone
    {
        get { return _attZone; }
        private set
        {
            _attZone = value;
            animator.SetBool(AnimationStrings.attZone, value);
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

    public float flipCooldown
    {
        get
        {
            return animator.GetFloat(AnimationStrings.flipCooldown);
        }
        private set
        {
            animator.SetFloat(AnimationStrings.flipCooldown, Mathf.Max(value, 0));
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingSpaceDirections>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
        healthBar = GetComponent<FloatingHealthBar>();
    }

    private void FixedUpdate()
    {

        // Don't flip if we're already flipping or standing on something
        if (player == null)
        {
            return; // No player found, don't try to move
        }
        if (touchingDirections.IsOnWall)
        {
            FlipDirection();
        }

        
        if (HasTarget && flipCooldown <= 0 && CanMove)
        {
            // Determine if the BikeBoss should flip to face the player
            Vector2 directionToPlayer = (player.position - transform.position).normalized;

            if (directionToPlayer.x > 0 && WalkDirection == WalkableDirection.Left)
            {
                WalkDirection = WalkableDirection.Right;

            }
            else if (directionToPlayer.x < 0 && WalkDirection == WalkableDirection.Right)
            {
                WalkDirection = WalkableDirection.Left;
            }

            rb.velocity = new Vector2(followSpeed * walkDirectionVector.x, rb.velocity.y);
            flipCooldown = 1;
        }
        else
        {
            if (CanMove)
                rb.velocity = new Vector2(walkSpeed * walkDirectionVector.x, rb.velocity.y);
            else
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);

        }

      
    }

    void Update()
    {
        if (player == null)
        {
            // Reattempt to find the player if it's null
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
            return;
        }
        HasTarget = attackZone.detectedColliders.Count > 0;
        attZone = attackingZone.detectedColliders.Count > 0; 

        if (AttackCooldown > 0)
        {
            AttackCooldown -= Time.deltaTime;
        }

        if (flipCooldown > 0) { 
        
            flipCooldown -= Time.deltaTime; 
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

    public void OnCliffDetected()
    {
        // Only flip if grounded and not in a flip state
        
    }
}
