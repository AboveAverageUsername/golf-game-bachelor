using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuController : MonoBehaviour
{

    public GameObject continueBtn;

    public GameObject playerNameWindow;
    public GameObject nameInputField;

    public GameObject stageEditorWindow;
    public GameObject stageListContent;
    public GameObject stageListElem;

    public GameObject settingsWindow;
    public TMP_Dropdown windowModeDropDown;
    public TMP_Dropdown resolutionDropDown;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public Toggle audioMuted;

    public GameObject stageInfo;
    public GameObject stageInfoImage;
    public TextMeshProUGUI stageInfoText;
    public GameObject stageListSelected = null;

    public GameObject aboutWindow;

    private string menuOpen = "main";

    private bool stagesLoaded = false;

    // Start is called before the first frame update
    void Start()
    {
        continueBtn.GetComponent <Button> ().interactable = GameDataController.controller.data.stageLevel != 0;
        stageEditorWindow.SetActive(false);
        settingsWindow.SetActive(false);
        playerNameWindow.SetActive(false);
        musicVolumeSlider.value = GameDataController.controller.data.musicVolume;
        sfxVolumeSlider.value = GameDataController.controller.data.soundEffectsVolume;
        audioMuted.isOn = GameDataController.controller.data.audioMuted;
        windowModeDropDown.value = GameDataController.controller.data.isFullScreen ? 1 : 0;
        resolutionDropDown.value = GameDataController.controller.data.resolution;
        AudioManager.Instance.swapTracks("MainTheme", 2.0f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            returnClick();
            AudioManager.Instance.Play("BtnClk");
        }
    }

    // Click New Game button
    public void newGameBtnClk()
    {
        SceneManager.LoadScene(1);
    }

    // Click Continue button
    public void continueBtnClk()
    {
        if (GameDataController.controller.data.stageLevel > 0)
        {
            if (doesSceneExist(GameDataController.controller.data.stageName))
            {
                //Debug.Log("Stage was valid");
                SceneManager.LoadSceneAsync(GameDataController.controller.data.stageName);
            }
            else
            {
                SceneManager.LoadSceneAsync(GameDataController.controller.customStageName);
            }
        }
    }

    // Click Player Name button
    public void playerNameBtnClk()
    {
        menuOpen = "playerName";
        playerNameWindow.SetActive(true);
        nameInputField.GetComponent<TMP_InputField>().text = GameDataController.controller.data.playerName;
        //Debug.Log(GameDataController.controller.data.playerName);
    }

    // Save player Name
    public void saveBtnClk()
    {
        string name = nameInputField.GetComponent<TMP_InputField>().text;
        // Debug.Log("Input player name: " + name);
        if (name != "")
        {
            GameDataController.controller.data.playerName = name;
            returnClick();
        }
    }

    // Click Leaderboard Button
    public void leaderBoardBtnClk()
    {
        SceneManager.LoadScene("Leaderboard");
    }

    public void stageEditorBtnClk()
    {
        menuOpen = "stageEditor";
        GameDataController.controller.chosenStage = null;
        stageEditorWindow.SetActive(true);
        stageInfo.SetActive(false);

        if(!stagesLoaded)
            foreach (GameData.CustomStage elem in GameDataController.controller.data.customStages)
            {
                GameObject listElem = Instantiate(stageListElem, stageListContent.transform);
                listElem.GetComponentInChildren<TextMeshProUGUI>().text = elem.stageName;
                listElem.GetComponent<ListElement>().myStage = elem;
                listElem.GetComponent<ListElement>().menu = this;
            }

        stagesLoaded = true;
    }

    public void newStageBtnClk()
    {
        GameDataController.controller.chosenStage = null;
        SceneManager.LoadSceneAsync("StageEditor");
    }

    public void editStageClk()
    {
        if(GameDataController.controller.data.stageName == GameDataController.controller.chosenStage.stageName)
        {
            GameDataController.controller.resetContinueData();
            continueBtn.GetComponent<Button>().interactable = false;
        }
        SceneManager.LoadSceneAsync("StageEditor");
    }

    public void deleteStageClk()
    {
        if (GameDataController.controller.data.stageName == GameDataController.controller.chosenStage.stageName)
        {
            GameDataController.controller.resetContinueData();
            continueBtn.GetComponent<Button>().interactable = false;
        }
        stageInfo.SetActive(false);
        GameDataController.controller.removeLeaderboard(GameDataController.controller.chosenStage.stageName);
        GameDataController.controller.data.customStages.Remove(GameDataController.controller.chosenStage);
        Destroy(stageListSelected);
        GameDataController.controller.chosenStage = null;
        stageListSelected = null;
    }

    public void controlsBtnClk()
    {
        SceneManager.LoadSceneAsync("Controls");
    }

    public void helpBtnClk()
    {
        SceneManager.LoadSceneAsync("Help");
    }

    public void exitBtnClk()
    {
        Application.Quit();
    }

    public void settingsBtnClk()
    {
        menuOpen = "settings";
        resolutionDropDown.value = GameDataController.controller.data.resolution;
        settingsWindow.SetActive(true);
    }

    public void aboutBtnClk()
    {
        menuOpen = "about";
        aboutWindow.SetActive(true);
    }

    public void windowModeSelect()
    {
        GameDataController.controller.setFullScreen(windowModeDropDown.value);
    }

    public void resolutionSelect()
    {
        if(resolutionDropDown.value != GameDataController.controller.data.resolution)
        {
            GameDataController.controller.setResolution(resolutionDropDown.value);
        }
        //AudioManager.Instance.Play("BtnClk");
    }

    public void returnClick()
    {
        switch (menuOpen)
        {
            case "main":
                break;
            case "playerName":
                playerNameWindow.SetActive(false);
                break;
            case "stageEditor":
                stageEditorWindow.SetActive(false);
                break;
            case "settings":
                settingsWindow.SetActive(false);
                break;
            case "about":
                aboutWindow.SetActive(false);
                break;
            default:
                break;
        }
    }

    public static bool doesSceneExist(string name)
    {
        if (string.IsNullOrEmpty(name))
            return false;

        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            var scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            var lastSlash = scenePath.LastIndexOf("/");
            var sceneName = scenePath.Substring(lastSlash + 1, scenePath.LastIndexOf(".") - lastSlash - 1);

            if (string.Compare(name, sceneName, true) == 0)
                return true;
        }

        return false;
    }
}
