using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffinHandler : MonoBehaviour, IRestartable
{
    [SerializeField] MumieMovement Mummie;
    [SerializeField] Animator animator;

    public void ActivateMumie()
    {
        Mummie.Activate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        animator.Play("Base Layer.Coffin_Open");
    }



    public void Restart()
    {
        animator.Play("Base Layer.Coffin_Empty");
        Mummie.Restart();
    }
}
