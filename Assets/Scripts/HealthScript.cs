using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    [SerializeField] Image[] Hearts;
    [SerializeField] Transform[] SpawnPoints;
    [SerializeField] Sprite HealthFull;
    [SerializeField] Sprite HealthEmpty;

    public int Health { get; private set; }
    private int spawnPointIndex;

    public void ChangeSpawnPointIndex()
    {
        spawnPointIndex++;
    }

    private void Start()
    {
        Health = Hearts.Length;
        spawnPointIndex = 0;
    }

    public void Damage(int damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            RestartObjects("Coffin");
            RestartObjects("Box");
            GetComponent<Movement>().Restart(SpawnPoints[spawnPointIndex].position);
            Health = Hearts.Length;
        }

        for (int i = 0; i < Hearts.Length; i++)
        {
            if (i >= Health)
            {
                Hearts[i].sprite = HealthEmpty;
            }

            else
            {
                Hearts[i].sprite = HealthFull;
            }
        }
    }

    private void RestartObjects(string tag)
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject gameObject in gameObjects)
            gameObject.GetComponent<IRestartable>().Restart();
    }
}
