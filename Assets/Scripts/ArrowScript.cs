using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    [SerializeField] float Speed;
    [SerializeField] Rigidbody2D Rb;

    private void Start()
    {
        Speed *= transform.localScale.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        Rb.velocity = new Vector2(Speed, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name != "Shooter")
            Destroy(this.gameObject);
    }
}
