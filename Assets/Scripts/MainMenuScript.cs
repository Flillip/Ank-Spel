using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void ChangeToPart1()
    {
        SceneManager.LoadScene("Part-1");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SceneManager.LoadScene("Win");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            ChangeToPart1();
    }
}
