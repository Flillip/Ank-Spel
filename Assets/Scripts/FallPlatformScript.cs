using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallPlatformScript : MonoBehaviour
{
    private float fallDelay = 0.5f;
    private float destroyDelay = 2f;

    [SerializeField] private Rigidbody2D rb;

    private void Start()
    {
        BoxCollider2D trigger = GetComponents<BoxCollider2D>()[1];
        trigger.size = new Vector2(GetComponent<SpriteRenderer>().size.x, trigger.size.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Fall());
        }
    }

    private IEnumerator Fall()
    {
        yield return new WaitForSeconds(fallDelay);
        rb.bodyType = RigidbodyType2D.Dynamic;
        Destroy(gameObject, destroyDelay);
    }
}
