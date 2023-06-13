using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LeaderboardController : MonoBehaviour
{

    public GameObject[] allRecords;
    public TMP_Dropdown stageChoice;

    // Start is called before the first frame update
    void Start()
    {
        List<string> allStages = new List<string>();
        foreach (GameData.StageLeaderboard item in GameDataController.controller.data.leaderboardList)
        {
            allStages.Add(item.stagename);   
        }
        stageChoice.AddOptions(allStages);

        stageChoice.onValueChanged.AddListener(selectedStage);

        AudioManager.Instance.swapTracks("MainTheme", 2.0f);
        string stage = "";
        if (PlayerPrefs.HasKey("leaderboard"))
        {
            stage = PlayerPrefs.GetString("leaderboard");
            PlayerPrefs.DeleteKey("leaderboard");
            for (int i = 0; i < stageChoice.options.Count; i++)
            {
                if(stageChoice.options[i].text == stage)
                {
                    stageChoice.value = i;
                    break;
                }
            }
        }
        else stage = stageChoice.options[stageChoice.value].text;
        GameData.LeaderBoardEntry[] leaderboard = GameDataController.controller.getLeaderboard(stage);
        for (int i = 0; i < 10; i++)
        {
            //allRecords[i].transform.Find("Name/NameText").GetComponent<TextMeshProUGUI>().text = GameDataController.controller.data.leaderboard[i].playerName;
            //allRecords[i].transform.Find("Stage/StageText").GetComponent<TextMeshProUGUI>().text = GameDataController.controller.data.leaderboard[i].stageName;
            //allRecords[i].transform.Find("Score/ScoreText").GetComponent<TextMeshProUGUI>().text = GameDataController.controller.data.leaderboard[i].playerScore == 0 ? "" : GameDataController.controller.data.leaderboard[i].playerScore.ToString();
            allRecords[i].transform.Find("Name/NameText").GetComponent<TextMeshProUGUI>().text = leaderboard[i].playerName;
            allRecords[i].transform.Find("Rank/RankText").GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();
            allRecords[i].transform.Find("Score/ScoreText").GetComponent<TextMeshProUGUI>().text = leaderboard[i].playerScore == 0 ? "" : leaderboard[i].playerScore.ToString();

        }
    }

    void selectedStage(int index)
    {
        string stage = stageChoice.options[stageChoice.value].text;
        GameData.LeaderBoardEntry[] leaderboard = GameDataController.controller.getLeaderboard(stage);
        for (int i = 0; i < 10; i++)
        {
            allRecords[i].transform.Find("Name/NameText").GetComponent<TextMeshProUGUI>().text = leaderboard[i].playerName;
            allRecords[i].transform.Find("Rank/RankText").GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();
            allRecords[i].transform.Find("Score/ScoreText").GetComponent<TextMeshProUGUI>().text = leaderboard[i].playerScore == 0 ? "" : leaderboard[i].playerScore.ToString();
        }
    }
}
