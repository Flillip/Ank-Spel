using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
   

    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;

    private bool doubleJump;
    private Animator animator;
    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 16f;
    private bool isFacingRight = true;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }


    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        animator.SetFloat("Speed", Mathf.Abs(horizontal));

        bool grounded = IsGrounded();

        if (grounded && !Input.GetButton("Jump"))
        {
            doubleJump = false;
            animator.SetBool("IsJumping", false);
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (grounded || doubleJump)
            {
                animator.SetBool("IsJumping", true);
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);

                doubleJump = !doubleJump;
            }
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        Flip();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.Raycast(groundCheck.position - new Vector3(transform.localScale.x / 2f, 0), Vector2.down, transform.localScale.y / 8f, groundLayer) ||
            Physics2D.Raycast(groundCheck.position + new Vector3(transform.localScale.x / 2f, 0), Vector2.down, transform.localScale.y / 8f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
