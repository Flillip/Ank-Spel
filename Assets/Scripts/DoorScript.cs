using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [SerializeField] ButtonScript Button;
    [SerializeField] Transform TopPos;
    [SerializeField] float MoveSpeed;

    private Vector3 posToMoveTo;
    private Vector3 originalPos;
    private DoorState doorState;
    private bool buttonReleased;

    // Start is called before the first frame update
    void Start()
    {
        Button.OnPressed += OnButtonPressed;
        Button.OnUnPressed += OnButtonUnPressed;
        posToMoveTo = TopPos.position;
        originalPos = transform.position;
        doorState = DoorState.Idle;
        buttonReleased = false;
    }

    void OnButtonPressed()
    {
        if (doorState == DoorState.Idle)
            StartCoroutine(MoveToTop());
    }

    void OnButtonUnPressed()
    {
        buttonReleased = true;
        StartCoroutine(MoveToBottom());
    }

    private IEnumerator MoveToTop()
    {
        doorState = DoorState.MovingUp;

        while (transform.position.y < posToMoveTo.y)
        {
            if (buttonReleased == true)
                break;
            transform.position += new Vector3(0, MoveSpeed * Time.deltaTime, 0);
            yield return null;
        }

        doorState = DoorState.Idle;
    }

    private IEnumerator MoveToBottom()
    {
        while (doorState != DoorState.Idle) { yield return null; }
        buttonReleased= false;
        doorState = DoorState.MovingDown;

        while (transform.position.y > originalPos.y)
        {
            transform.position -= new Vector3(0, MoveSpeed * Time.deltaTime, 0);
            yield return null;
        }

        doorState = DoorState.Idle;
    }
}

enum DoorState
{
    MovingUp,
    MovingDown,
    Idle
}
