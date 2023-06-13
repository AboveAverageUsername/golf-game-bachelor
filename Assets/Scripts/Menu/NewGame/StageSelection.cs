using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;
using System.Text;

public class StageSelection : MonoBehaviour
{
    public TextMeshProUGUI stageText;
    public GameObject stagePreview;


    private string[] baseStages;
    private string currStage;
    private int currIndex;
    private bool onBaseStage;


    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.swapTracks("MainTheme", 2.0f);
        currIndex = 0;
        onBaseStage = true;

        baseStages = GameDataController.controller.gameStages.ToArray();
        loadStagePreview(baseStages[0], false);
    }


    public void startGame()
    {
        GameDataController.controller.data.stageName = stageText.text;
        GameDataController.controller.data.playerScore = 0;
        GameDataController.controller.data.stageLevel = 0;
        //Debug.Log(baseStages[currIndex]);
        if (onBaseStage)
        {
            if(SceneUtility.GetBuildIndexByScenePath("Scenes/" + baseStages[currIndex]) != -1)
                SceneManager.LoadSceneAsync(baseStages[currIndex]);
            else
                SceneManager.LoadSceneAsync(GameDataController.controller.customStageName);
        }
        else
        {
            SceneManager.LoadSceneAsync(GameDataController.controller.customStageName);
        }      
    }

    public void nextStage()
    {
        clickArrowSound();
        currIndex++;
        if (onBaseStage)
        {
            if(currIndex >= baseStages.Length && GameDataController.controller.data.customStages.Count > 0)
            {
                currIndex = 0;
                onBaseStage = false;
                loadStagePreview(GameDataController.controller.data.customStages[currIndex].stageName, true);
            }
            else
            {
                onBaseStage= true;
                currIndex = currIndex % baseStages.Length;
                loadStagePreview(baseStages[currIndex], false);
            }
            
        }
        else
        {
            if (currIndex >= GameDataController.controller.data.customStages.Count && baseStages.Length > 0)
            {
                currIndex = 0;
                onBaseStage = true;
                loadStagePreview(baseStages[currIndex], false);
            }
            else
            {
                onBaseStage = false;
                currIndex = currIndex % GameDataController.controller.data.customStages.Count;
                loadStagePreview(GameDataController.controller.data.customStages[currIndex].stageName, true);
            }
        }
    }

    public void prevStage()
    {
        clickArrowSound();
        currIndex--;
        if (onBaseStage)
        {
            if (currIndex < 0 && GameDataController.controller.data.customStages.Count > 0)
            {
                currIndex = GameDataController.controller.data.customStages.Count - 1;
                onBaseStage = false;
                loadStagePreview(GameDataController.controller.data.customStages[currIndex].stageName, true);
            }
            else
            {
                onBaseStage = true;
                currIndex = currIndex % baseStages.Length;
                loadStagePreview(baseStages[currIndex], false);
            }

        }
        else
        {
            if (currIndex < 0 && baseStages.Length > 0)
            {
                currIndex = baseStages.Length - 1;
                onBaseStage = true;
                loadStagePreview(baseStages[currIndex], false);
            }
            else
            {
                onBaseStage = false;
                currIndex = currIndex % GameDataController.controller.data.customStages.Count;
                loadStagePreview(GameDataController.controller.data.customStages[currIndex].stageName, true);
            }
        }
    }

    public void loadStagePreview(string name, bool isCustom)
    {
        stageText.text = name;
        Texture2D s = Resources.Load<Texture2D>("Images/DefaultStageImage"); ;
        if (onBaseStage)
        {
            s = Resources.Load<Texture2D>("Images/" + (isCustom ? "CustomStages/" : "") + name);
            if (s == null)
            {
                s = Resources.Load<Texture2D>("Images/DefaultStageImage");
            }
        }
        else
        {
            foreach (GameData.CustomStage elem in GameDataController.controller.data.customStages)
            {
                if(name == elem.stageName)
                {
                    s = new Texture2D(1, 1, TextureFormat.ARGB32, false);
                    s.LoadImage(Convert.FromBase64String(elem.stagePreview));
                    break;
                }
            }
        }
        stagePreview.GetComponent<RawImage>().texture = s;
    }


    public void clickArrowSound()
    {
        AudioManager.Instance.Play("BtnClk");
    }
}
