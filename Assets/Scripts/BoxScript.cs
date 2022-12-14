using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : MonoBehaviour, IRestartable
{
    private Vector2 initalPosition;

    private void Start()
    {
        initalPosition = transform.position;
    }

    public void Restart()
    {
        transform.position = initalPosition;
    }
}
