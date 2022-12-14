using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    [SerializeField] Image[] Hearts;
    [SerializeField] Sprite HealthFull;
    [SerializeField] Sprite HealthEmpty;

    private int health;

    private void Start()
    {
        health = Hearts.Length;
    }

    public void Damage(int damage)
    {
        health -= damage;

        if (health <= 0)
            Debug.Log("YOU DEAD");

        for (int i = 0; i < Hearts.Length; i++)
        {
            if (i >= health)
            {
                Hearts[i].sprite = HealthEmpty;
            }

            else
            {
                Hearts[i].sprite = HealthFull;
            }
        }
    }
}
