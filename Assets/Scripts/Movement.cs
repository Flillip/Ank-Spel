using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Movement : MonoBehaviour
{
   

    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] bool canDoubleJump = true;

    private bool doubleJump;
    private Animator animator;
    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 16f;
    private bool isFacingRight = true;
    private float halfWidth;
    private bool OnButton;

    private void Start()
    {
        animator = GetComponent<Animator>();
        halfWidth = GetComponent<SpriteRenderer>().bounds.size.x / 2f;
        OnButton = false;
    }


    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        animator.SetFloat("Speed", Mathf.Abs(horizontal));

        bool grounded = IsGrounded();

        if ((grounded || OnButton) && !Input.GetButton("Jump"))
        {
            doubleJump = false;
            animator.SetBool("IsJumping", false);
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (grounded || doubleJump || OnButton)
            {
                animator.SetBool("IsJumping", true);
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);

                if (canDoubleJump)
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
        return Physics2D.Raycast(groundCheck.position - new Vector3(halfWidth - .2f, 0), Vector2.down, transform.localScale.y / 8f, groundLayer) ||
            Physics2D.Raycast(groundCheck.position + new Vector3(halfWidth - .2f, 0), Vector2.down, transform.localScale.y / 8f, groundLayer);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        FixButton(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        FixButton(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        FixButton(collision, true);
    }

    private void FixButton(Collision2D collision, bool exit = false)
    {
        OnButton = (collision.gameObject.name == "Butt" ||
            collision.gameObject.name == "ButtSquare")
            && !exit;
    }
}
