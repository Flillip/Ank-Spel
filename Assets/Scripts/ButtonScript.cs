using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public Action OnPressed; 
    public Action OnUnPressed;

    [SerializeField] float MoveSpeed;
    [SerializeField] float MaxPos;
    private Vector3 originalPos;
    private Transform originalParent;

    private void Start()
    {
        originalPos = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Player" && collision.gameObject.tag != "Box")
            return;

        float buttonWidth = GetComponent<SpriteRenderer>().bounds.size.x / 2f;

        if (!(collision.transform.position.x >= transform.position.x - buttonWidth &&
            collision.transform.position.x <= transform.position.x + buttonWidth))
            return;

        originalParent = collision.transform.parent;
        collision.transform.parent = transform;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Player" && collision.gameObject.tag != "Box")
            return;

        float buttonWidth = GetComponent<SpriteRenderer>().bounds.size.x / 2f;

        if (!(collision.transform.position.x >= transform.position.x - buttonWidth &&
            collision.transform.position.x <= transform.position.x + buttonWidth))
            return;

        if (transform.localPosition.y <= MaxPos)
        {
            OnPressed?.Invoke();
            return;
        }

        transform.localPosition -= new Vector3(0, MoveSpeed * Time.deltaTime, 0);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Player" && collision.gameObject.tag != "Box")
            return;

        StartCoroutine(MoveBack());
        collision.transform.parent = originalParent;
    }

    private IEnumerator MoveBack()
    {
        while (transform.position.y < originalPos.y)
        {
            transform.localPosition += new Vector3(0, MoveSpeed * Time.deltaTime, 0);
            yield return null;
        }

        OnUnPressed?.Invoke();
    }
}
