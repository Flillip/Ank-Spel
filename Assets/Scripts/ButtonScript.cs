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
    private bool stillPressed;
    private bool currentlyMoving;
    private IEnumerator movingUpEnumerator;

    private void Start()
    {
        originalPos = transform.position;
        stillPressed = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Player" && collision.gameObject.tag != "Box")
            return;

        originalParent = collision.transform.parent;
        collision.transform.parent = transform;
        StopAllCoroutines();
        movingUpEnumerator = Move();
        StartCoroutine(movingUpEnumerator);
    }

    private IEnumerator Move()
    {
        yield return new WaitForSecondsRealtime(0.05f);

        if (stillPressed)
        {
            currentlyMoving = true;
            while (transform.localPosition.y > MaxPos)
            {
                transform.localPosition -= new Vector3(0, MoveSpeed * Time.deltaTime, 0);
                yield return null;
            }

            OnPressed?.Invoke();
        }

        currentlyMoving = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Player" && collision.gameObject.tag != "Box")
            return;

        stillPressed = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Player" && collision.gameObject.tag != "Box")
            return;

        stillPressed = false;
        StartCoroutine(MoveBack());
        collision.transform.parent = originalParent;
    }

    private IEnumerator MoveBack()
    {
        yield return new WaitForSeconds(0.1f);

        if (stillPressed)
            yield break;

        StopCoroutine(movingUpEnumerator);


        currentlyMoving = true;
        while (transform.position.y < originalPos.y)
        {
            transform.localPosition += new Vector3(0, MoveSpeed * Time.deltaTime, 0);
            yield return null;
        }

        OnUnPressed?.Invoke();
        currentlyMoving = false;
    }
}
