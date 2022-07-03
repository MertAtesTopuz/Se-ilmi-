using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private TrailRenderer traRen;

    [Header("Movement")]
    [SerializeField] private int Speed;
    private bool faceRight = true;

    [Header("Jumping")]
    [SerializeField] private int jumpSpeed;
    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;
    private int extraJumps;
    public int extraJumpsValue;

    [Header("Dashing")]
    public float dashSpeed;
    private float dashTime;
    public float startDashTime;
    private int direction;
    public bool isDashing = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        traRen = GetComponent<TrailRenderer>();
        extraJumps = extraJumpsValue;
        dashTime = startDashTime;
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * Speed, rb.velocity.y);
        if (moveInput > 0 || moveInput < 0)
        {
           animator.SetBool("isRunning", true);
        }
        else
        {
           animator.SetBool("isRunning", false);
        }

        if (faceRight == true && moveInput < 0)
        {
            Flip();
        }
        else if (faceRight == false && moveInput > 0)
        {
            Flip();
        }

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("attack");
        }
        // dash start
        if (isDashing == true)
        {
            if (direction == 0)
            {
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    traRen.emitting = true;
                    if (moveInput < 0)
                    {
                        direction = 1;
                    }
                    else if (moveInput > 0)
                    {
                        direction = 2;
                    }
                }
            }
            else
            {
                if (dashTime <= 0)
                {
                    traRen.emitting = false;
                    direction = 0;
                    dashTime = startDashTime;
                    rb.velocity = Vector2.zero;
                }
                else
                {
                    dashTime -= Time.deltaTime;

                    if (direction == 1)
                    {
                        rb.velocity = Vector2.left * dashSpeed;
                    }
                    else if (direction == 2)
                    {
                        rb.velocity = Vector2.right * dashSpeed;
                    }
                }
            }

        }

        // dash end

    }

    private void Update()
    {
        Jump();
        
    }

    void Flip()
    {
        faceRight = !faceRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    void Jump()
    {
        if (isGrounded == true)
        {
            extraJumps = extraJumpsValue;
        }

        if (Input.GetKeyDown(KeyCode.Space) && extraJumps > 0)
        {
            rb.velocity = Vector2.up * jumpSpeed;
            extraJumps--;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && extraJumps == 0 && isGrounded == true)
        {
            rb.velocity = Vector2.up * jumpSpeed;
        }
    }

}
