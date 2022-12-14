using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Movement : MonoBehaviour
{
   

    [SerializeField] Rigidbody2D Rb;
    [SerializeField] Transform GroundCheck;
    [SerializeField] LayerMask GroundLayer;
    [SerializeField] bool CanDoubleJump = true;
    [SerializeField] Camera Camera;
    [SerializeField] float CameraMovementOffset;
    [SerializeField] float PlayerMovementOffsetOnCameraMove = 2;
    [SerializeField] HealthScript Health;
    [SerializeField] float KnockbackForce;

    private bool doubleJump;
    private Animator animator;
    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 16f;
    private bool isFacingRight = true;
    private float halfWidth;
    private bool onButton;
    private bool canMoveCamera;
    private bool canMove;

    private void Start()
    {
        animator = GetComponent<Animator>();
        halfWidth = GetComponent<SpriteRenderer>().bounds.size.x / 2f;
        onButton = false;
        animator.SetBool("IsJumping", true);
        canMoveCamera = true;
        canMove = true;
    }


    private void Update()
    {
        canMoveCamera = true;
        horizontal = Input.GetAxisRaw("Horizontal");
        animator.SetFloat("Speed", Mathf.Abs(horizontal));

        bool grounded = IsGrounded();

        if ((grounded || onButton) && !Input.GetButton("Jump"))
        {
            doubleJump = false;
            animator.SetBool("IsJumping", false);
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (grounded || doubleJump || onButton)
            {
                animator.SetBool("IsJumping", true);
                Rb.velocity = new Vector2(Rb.velocity.x, jumpingPower);

                if (CanDoubleJump)
                    doubleJump = !doubleJump;
            }
        }

        if (Input.GetButtonUp("Jump") && Rb.velocity.y > 0f)
        {
            Rb.velocity = new Vector2(Rb.velocity.x, Rb.velocity.y * 0.5f);
        }

        Flip();
    }

    private void FixedUpdate()
    {
        if(canMove)
            Rb.velocity = new Vector2(horizontal * speed, Rb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.Raycast(GroundCheck.position - new Vector3(halfWidth - .2f, 0), Vector2.down, transform.localScale.y / 8f, GroundLayer) ||
            Physics2D.Raycast(GroundCheck.position + new Vector3(halfWidth - .2f, 0), Vector2.down, transform.localScale.y / 8f, GroundLayer);
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

        if (collision.gameObject.name == "Mumie")
        {
            Health.Damage(1);
            ApplyKnockback(collision);
        }
    }

    private void ApplyKnockback(Collision2D collision)
    {
        Vector2 dif = transform.position - collision.transform.position;
        Vector2 knockback = dif.normalized * KnockbackForce;
        Rb.AddForce(knockback, ForceMode2D.Impulse);
        StartCoroutine(DisableMovement(0.2f));
    }

    private IEnumerator DisableMovement(float seconds)
    {
        canMove = false;
        yield return new WaitForSeconds(seconds);
        canMove = true;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        FixButton(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        FixButton(collision, true);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Water")
        {
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsSwimming", true);
        }


        if (canMoveCamera)
        {
            if (collision.gameObject.name == "CameraColliderLeft")
            {
                Camera.transform.position += new Vector3(-CameraMovementOffset, 0);
                transform.position += new Vector3(-2, 0);
            }
            
            else if (collision.gameObject.name == "CameraColliderRight")
            {
                transform.position += new Vector3(PlayerMovementOffsetOnCameraMove, 0);
                Camera.transform.position += new Vector3(CameraMovementOffset, 0);
            }

            canMoveCamera = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Water")
        {
            animator.SetBool("IsSwimming", false);
        }
    }

    private void FixButton(Collision2D collision, bool exit = false)
    {
        onButton = (collision.gameObject.name == "Butt" ||
            collision.gameObject.name == "ButtSquare")
            && !exit;
    }
}
