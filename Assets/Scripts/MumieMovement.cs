using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MumieMovement : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float speed;

    private float halfWidth;
    private float dir;
    private bool active = false;
    private Transform initalTransform;

    public void Activate()
    {
        this.gameObject.SetActive(true);
        active = true;
    }

    public void Restart()
    {
        gameObject.SetActive(false);
        active = false;
        if (initalTransform != null)
            transform.position = initalTransform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        dir = 1;
        initalTransform = transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!active)
            return;

        Flip();

        rb.velocity = new Vector2(dir * speed, rb.velocity.y);
    }

    void Flip()
    {
        if ((dir == 1 && !RayRight()) || (dir == -1 && !RayLeft()))
        {
            dir *= -1;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        dir *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    private bool RayLeft()
    {
        return Physics2D.Raycast(groundCheck.position + new Vector3(halfWidth - .2f, 0), Vector2.down, transform.localScale.y / 2f, groundLayer);
    }

    private bool RayRight()
    {
        return Physics2D.Raycast(groundCheck.position - new Vector3(halfWidth - .2f, 0), Vector2.down, transform.localScale.y / 2f, groundLayer);
    }
}
