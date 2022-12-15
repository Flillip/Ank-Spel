using System;
using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] Rigidbody2D Rb;
    [SerializeField] Transform GroundCheck;
    [SerializeField] LayerMask GroundLayer;
    [SerializeField] bool CanDoubleJump = true;
    [SerializeField] Camera Camera;
    [SerializeField] Vector2 CameraMovementOffset;
    [SerializeField] float PlayerMovementOffsetOnCameraMove = 2;
    [SerializeField] HealthScript Health;
    [SerializeField] float KnockbackForce;
    [SerializeField] float speed = 8f;

    private bool doubleJump;
    private Animator animator;
    private float horizontal;
    
    private float jumpingPower = 16f;
    private bool isFacingRight = true;
    private float halfWidth;
    private bool onButton;
    private bool canMoveCamera;
    private bool canMove;
    private bool canTakeDamage;
    private bool justReset;

    private void Start()
    {
        animator = GetComponent<Animator>();
        halfWidth = GetComponent<SpriteRenderer>().bounds.size.x / 2f;
        onButton = false;
        animator.SetBool("IsJumping", true);
        canMoveCamera = true;
        canMove = true;
        canTakeDamage = true;
        justReset = false;
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
        if(canMove && !justReset)
            Rb.velocity = new Vector2(horizontal * speed, Rb.velocity.y);
        else if (justReset)
        {
            Rb.velocity = Vector3.zero;
            Rb.angularVelocity = 0;
        }
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

        if (!canTakeDamage)
            return;

        if (collision.gameObject.name == "Mumie" || collision.gameObject.name == "Spikes")
        {
            Health.Damage(1);
            if (Health.Health <= 0)
                return;

            ApplyKnockback(collision);
            StartCoroutine(DisableDamage(0.5f));
            animator.Play("Base Layer.Player_Hurt");

        }
    }

    private void ApplyKnockback(Collision2D collision)
    {
        Vector2 dif = transform.position - collision.transform.position;
        Vector2 knockback = dif.normalized * KnockbackForce;
        Rb.AddForce(knockback, ForceMode2D.Impulse);
        StartCoroutine(DisableMovement(0.2f));
        animator.SetBool("IsJumping", false);
    }

    private IEnumerator DisableDamage(float seconds)
    {
        canTakeDamage = false;
        yield return new WaitForSecondsRealtime(seconds);
        canTakeDamage = true;
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

        HandleCamera(collision);
    }

    private void HandleCamera(Collider2D collision)
    {
        if (canMoveCamera)
        {
            if (collision.gameObject.name == "CameraColliderLeft")
            {
                Camera.transform.position += new Vector3(-CameraMovementOffset.x, 0);
                transform.position += new Vector3(-PlayerMovementOffsetOnCameraMove, 0);
            }

            else if (collision.gameObject.name == "CameraColliderRight")
            {
                transform.position += new Vector3(PlayerMovementOffsetOnCameraMove, 0);
                Camera.transform.position += new Vector3(CameraMovementOffset.x, 0);
            }

            else if (collision.gameObject.name == "CameraColliderBottom")
            {
                Camera.transform.position += new Vector3(0, -CameraMovementOffset.y);
                transform.position += new Vector3(0, -PlayerMovementOffsetOnCameraMove);
            }

            else if (collision.gameObject.name == "CameraColliderTop")
            {
                transform.position += new Vector3(0, PlayerMovementOffsetOnCameraMove);
                Camera.transform.position += new Vector3(0, CameraMovementOffset.y);
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

    public void Restart(Vector2 pos)
    {
        Rb.velocity = Vector3.zero;
        Rb.angularVelocity = 0;
        Rb.position = pos;
        justReset = true;
        Rb.Sleep();
        StartCoroutine(WakeUp(.5f));
    }

    private IEnumerator WakeUp(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Rb.WakeUp();
        justReset = false;
    }
}
