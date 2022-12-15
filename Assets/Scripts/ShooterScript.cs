using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterScript : MonoBehaviour
{
    [SerializeField] GameObject Arrow;
    [SerializeField] float ShootDelay;

    private bool shouldShoot = false;
    private bool alreadyShooting = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name != "Player")
            return;

        shouldShoot = true;
        StartCoroutine(Shoot());
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name != "Player")
            return;

        shouldShoot = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name != "Player")
            return;

        shouldShoot = false;
    }

    private IEnumerator Shoot()
    {
        if (alreadyShooting)
            yield break;

        alreadyShooting = true;
        while (shouldShoot)
        {
            Instantiate(Arrow, transform.position, transform.rotation * Quaternion.Euler(0, 0, -180), transform);

            yield return new WaitForSeconds(ShootDelay);
        }

        alreadyShooting = false;
    }
}
