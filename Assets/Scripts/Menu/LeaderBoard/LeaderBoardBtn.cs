using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LeaderBoardBtn : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(goToLeaderboard);
    }

    void goToLeaderboard()
    {
        SceneManager.LoadScene("Leaderboard");
    }
}
