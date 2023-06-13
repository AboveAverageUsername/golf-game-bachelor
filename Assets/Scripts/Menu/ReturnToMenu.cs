using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenu : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            returnToMenu();
        }
    }

    public void returnToMenu()
    {
        AudioManager.Instance.Play("BtnClk");
        SceneManager.LoadScene("MainMenu");
    }
}
