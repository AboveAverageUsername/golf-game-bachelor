using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PauseController : MonoBehaviour
{

    public GameObject pauseMenu;
    public UnityEvent pausedGame;

    private bool gameRunning;
    private bool escEnabled;

    // Start is called before the first frame update
    void Start()
    {
        gameRunning = true;
        escEnabled = true;
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            AudioManager.Instance.Play("BtnClk");
            if (escEnabled)
            {
                gameRunning = !gameRunning;
                pauseMenu.SetActive(!gameRunning);
                pausedGame.Invoke();
                //Debug.Log("ESC " + (!gameRunning ? "show" : "hide") + " cursor");
                Cursor.visible = !gameRunning;
            }
        }
    }

    public void gamePaused()
    {
        gameRunning = !gameRunning;
        pauseMenu.SetActive(!gameRunning);
        pausedGame.Invoke();
        //Debug.Log("PAUSE " + (!gameRunning ? "show" : "hide") + " cursor");
        Cursor.visible = !gameRunning;
    }

    public void gameFreeze()
    {
        gameRunning = !gameRunning;
        pausedGame.Invoke();
        //Debug.Log("FREEZE " + (!gameRunning ? "show" : "hide") + " cursor");
        Cursor.visible = !gameRunning;
    }

    public void disableEsc()
    {
        escEnabled = false;
    }

    public void enableEsc()
    {
        escEnabled = true;
    }

    public void goToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("MainMenu");
    }
}
