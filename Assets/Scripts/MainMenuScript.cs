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
}
