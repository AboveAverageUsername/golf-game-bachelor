using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClickScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(playButtonSound);
    }

    public void playButtonSound()
    {
        AudioManager.Instance.Play("BtnClk");
    }
}
