using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackButtonControler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(gotoMainMenu);
    }

    public void gotoMainMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
