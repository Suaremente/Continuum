using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEye : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed;
    private GameObject player;
    public bool chase = false;
    public Transform startingPoint;
    private Animator animator;
    public DetectionZone attackZone;
    private Damageable damageable;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); 
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

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (player == null)
            return;
        Flip();
        if(chase == true)
            Chase();
        else
            ReturnStartingPoint();

        HasTarget = attackZone.detectedColliders.Count > 0;

    }

    private void ReturnStartingPoint() {

        transform.position = Vector2.MoveTowards(transform.position,startingPoint.position, speed * Time.deltaTime);
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();   
        animator = GetComponent<Animator>();
        attackZone = GetComponentInChildren<DetectionZone>();
        damageable = GetComponent<Damageable>();    

    }

    public void Chase() {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime); 

    }

    private void Flip() {

        if (transform.position.x < player.transform.position.x)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else transform.rotation = Quaternion.Euler(0, 180, 0);
    }
}
