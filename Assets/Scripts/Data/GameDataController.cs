using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class GameDataController : MonoBehaviour
{
    public static GameDataController controller;

    public GameData data;

    public GameData.CustomStage chosenStage = null;

    [SerializeField] private string fileName;
    private FileDataHandler dataHandler;

    public List<string> gameStages;

    public Texture2D cursorImage;

    public string customStageName = "StageCustom";

    public int[,] resolutions = new int[7, 2]{
        { 2560, 1440 },
        { 1920, 1080 },
        { 1600, 900 },
        { 1366, 768 },
        { 1280, 720 },
        { 1152, 648 },
        { 1024, 576 }
    };

#if UNITY_STANDALONE_WIN || UNITY_EDITOR
    [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
    private static extern bool SetWindowPos(IntPtr hwnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
    [DllImport("user32.dll", EntryPoint = "FindWindow")]
    public static extern IntPtr FindWindow(System.String className, System.String windowName);
    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);
    [StructLayout(LayoutKind.Sequential)]
    private struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }
    public static void SetPosition(int x, int y, int resX = 0, int resY = 0)
    {
        SetWindowPos(FindWindow(null, "Golf"), 0, x, y, resX, resY, resX * resY == 0 ? 1 : 0);
    }
#endif

    private void Awake()
    {
        if(controller == null)
        {
            DontDestroyOnLoad(gameObject);
            controller = this;
            Debug.Log("Data created");

        }
        else if(controller != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Registrovanje osnovnih terena
        gameStages = new List<string>();

        gameStages.Add("Maze Hill");


        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, false);
        LoadGame();

        AudioManager.Instance.muteAudioOnCheck(controller.data.audioMuted);
        AudioManager.Instance.onMusicSliderValueChange(controller.data.musicVolume);
        AudioManager.Instance.onSFXSliderValueChange(controller.data.soundEffectsVolume);



        Cursor.SetCursor(cursorImage, new Vector2(12, 9), CursorMode.Auto);

        if (controller.data.isFullScreen)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            while(resolutions[controller.data.resolution, 0] > Display.main.systemWidth || resolutions[controller.data.resolution, 1] > Display.main.systemHeight)
            {
                controller.data.resolution++;
            }
            if (controller.data.resolution >= controller.resolutions.Length) controller.data.resolution = controller.resolutions.Length - 1;
            Screen.SetResolution(resolutions[controller.data.resolution, 0], resolutions[controller.data.resolution, 1] ,false);
            SetPosition((int)controller.data.position.x, (int)controller.data.position.y);
        }

    }

    public void NewGame()
    {
        this.data = new GameData();
    }

    public void LoadGame()
    {
        this.data = dataHandler.Load();
        

        if(this.data == null)
        {
            NewGame();
            Debug.Log("No data found in memory. New data created. Player name: " + this.data.playerName);
        }
        else
        {
            Debug.Log("Data loaded from memory. Player name: " + this.data.playerName);
        }
    }

    public void SaveGame()
    {
        dataHandler.Save(this.data);
    }

    private void OnApplicationQuit()
    {
        RECT rct = new RECT();
        GetWindowRect(FindWindow(null, "Golf"), ref rct);
        controller.data.position.x = rct.Left;
        controller.data.position.y = rct.Top;
        SaveGame();
    }

    public void saveLevelProgress(int level)
    {
        this.data.stageLevel = level;
    }

    public void saveScoreProgress(int score)
    {
        this.data.playerScore = score;
    }

    public bool addRecord(string stageName)
    {
        GameData.LeaderBoardEntry[] leaderboard = getLeaderboard(stageName);
        int place = -1;
        int oldPlace = -1;
        for(int i = 9; i >= 0; i--)
        {
            if(data.playerScore > leaderboard[i].playerScore)
            {
                place = i;
            }
            if (data.playerName == leaderboard[i].playerName)
            {
                oldPlace = i;
            }
        }
        Debug.Log("New Record place is " + place + ", Old Record place is " + oldPlace);
        // New Record and no old record
        if (place >= 0 && oldPlace == -1)
        {
            // Move lower scores
            for (int i = 9; i > place; i--)
            {
                leaderboard[i] = leaderboard[i - 1];
            }


            leaderboard[place] = new GameData.LeaderBoardEntry(data.playerName, data.playerScore, data.stageName);

            //for (int i = 0; i < 10; i++)
            //{
            //    Debug.Log(i + ". place: " + data.leaderboard[i].playerName + " " + data.leaderboard[i].playerScore);
            //}
        }
        else if (place >= 0 && oldPlace >= 0 && place <= oldPlace)
        {
            for (int i = oldPlace; i > place; i++)
            {
                leaderboard[i] = leaderboard[i - 1];
            }
            leaderboard[place] = new GameData.LeaderBoardEntry(data.playerName, data.playerScore, data.stageName);
        }
        else
        {
            data.stageLevel = 0;
            data.playerScore = 0;
            data.stageName = "";
            return false;
        }

        data.stageLevel = 0;
        data.playerScore = 0;
        data.stageName = "";
        SaveGame();
        return true;
    }

    public void resetContinueData()
    {
        data.stageName = "";
        data.stageLevel = 0;
        data.playerScore = 0;
    }

    public void changeMusicVolume(float value)
    {
        controller.data.musicVolume = value;
    }

    public void changeSFXVolume(float value)
    {
        controller.data.soundEffectsVolume = value;
    }

    public void setMuteAudio(bool muted)
    {
        controller.data.audioMuted = muted;
    }

    public void setFullScreen(int value)
    {
        bool isFullScreen = value == 1 ? true : false;
        controller.data.isFullScreen = isFullScreen;
        //Debug.Log("VALUE IS: " + value);
        switch (value)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                setResolution(controller.data.resolution);
                //Debug.Log("WIN");
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;

               // Debug.Log("FULL");
                break;
            default:
                break;
        }
    }

    public void setResolution(int index)
    {
        if(index > 0 && index < resolutions.Length)
        {
            if(!controller.data.isFullScreen)
                Screen.SetResolution(resolutions[index, 0], resolutions[index, 1], false);
            controller.data.resolution = index;
        }
    }

    public GameData.LeaderBoardEntry[] getLeaderboard(string stageName)
    {
        foreach (GameData.StageLeaderboard item in GameDataController.controller.data.leaderboardList)
        {
            if(item.stagename == stageName)
            {
                return item.leaderboard;
            }
        }
        GameData.LeaderBoardEntry[] leaderboard = new GameData.LeaderBoardEntry[10];
        for (int i = 0; i < 10; i++)
        {
            leaderboard[i] = new GameData.LeaderBoardEntry();
        }
        return leaderboard;
    }

    public void changeLeaderboardName(string from, string to)
    {
        foreach (GameData.StageLeaderboard item in GameDataController.controller.data.leaderboardList)
        {
            if (item.stagename == from)
            {
                item.stagename = to;
                return;
            }
        }
    }

    public void removeLeaderboard(string stageName)
    {
        GameData.StageLeaderboard leaderboard = null;
        foreach (GameData.StageLeaderboard item in GameDataController.controller.data.leaderboardList)
        {
            if (item.stagename == stageName)
            {
                leaderboard = item;
            }
        }
        if(leaderboard != null)
        {
            controller.data.leaderboardList.Remove(leaderboard);
        }
    }
}
