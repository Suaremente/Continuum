using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchingSpaceDirections : MonoBehaviour
{   
    Animator animator; 
    public ContactFilter2D castFilter;
    public float groundDistance = 0.05f; 
    CapsuleCollider2D touchingCol; 
    RaycastHit2D[] groundHits = new RaycastHit2D[5];

    [SerializeField]
    private bool _isGrounded;
    public bool IsGrounded { get {
        return _isGrounded; 
    } private set {
        _isGrounded = value;
        animator.SetBool("isGrounded", value); 
    } } 
    private void Awake() {
        touchingCol = GetComponent<CapsuleCollider2D>(); 
    }
    void FixedUpdate()
    {
        IsGrounded = touchingCol.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0; 
    }
}
