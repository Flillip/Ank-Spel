using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AIChase : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] GameObject player;
    [SerializeField] float speed;
    [SerializeField] float distanceBetween;

    private float distance;
    private bool canMove;

    private Transform target;


    private void OnTriggerStay2D(Collider2D collision)
    {
        {
            if (Vector2.Distance(transform.position, target.position) > 1.0)
            {
                transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            }
        }

    }
        void Start()
    {
        canMove = true;

    }

    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;


        if (distance < distanceBetween && canMove)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(Vector3.forward * angle);
        }

    }
}