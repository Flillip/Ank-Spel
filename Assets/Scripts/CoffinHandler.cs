using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffinHandler : MonoBehaviour
{
    [SerializeField] MumieMovement Mummie;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name != "Player")
            return;
        Mummie.Activate();
    }
}
