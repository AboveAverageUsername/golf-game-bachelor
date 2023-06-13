using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    [System.Serializable]
    public class LeaderBoardEntry
    {
        public string playerName;
        public int playerScore;
        public string stageName;
        
        public LeaderBoardEntry()
        {
            playerName = "";
            playerScore = 0;
            stageName = "";
        }

        public LeaderBoardEntry(string name, int score, string stage)
        {
            playerName = name;
            playerScore = score;
            stageName = stage;
        }
    }


    [System.Serializable]
    public class StageObstacle
    {
        public string name;
        public Vector3 position;

        public StageObstacle()
        {
            name = "";
            position = Vector3.zero;
        }

        public StageObstacle(string Name, Vector3 Position)
        {
            name = Name;
            position = Position;
        }
    }

    [System.Serializable]
    public class CustomStage
    {
        public string stageName;
        public string stagePreview;
        public int numOfCoins;

        //public StageObstacle[] obstacles;
        //public StageObstacle[] effectTiles;
        //public StageObstacle[] teleporters;
        //public StageObstacle[] birds;
        //public StageObstacle[] lakes;

        //public StageObstacle[] allElements;
        public List<StageObstacle> allElements;

        public CustomStage()
        {
            stageName = "";
            stagePreview = "";
            numOfCoins = 0;
            allElements = new List<StageObstacle>();
        }

        public CustomStage(string name)
        {
            stageName = name;
            stagePreview = "";
            numOfCoins = 0;
            allElements = new List<StageObstacle>();
        }
    }

    [System.Serializable]
    public class StageLeaderboard
    {
        public string stagename;
        public LeaderBoardEntry[] leaderboard;
        public StageLeaderboard(string name)
        {
            stagename = name;
            leaderboard = new LeaderBoardEntry[10];
            for (int i = 0; i < 10; i++)
            {
                leaderboard[i] = new LeaderBoardEntry();
            }
        }
    }

    public string playerName;
    public string stageName;
    public int stageLevel;
    public int playerScore;

    public bool audioMuted;
    public float musicVolume;
    public float soundEffectsVolume;

    public bool isFullScreen;
    public int resolution;
    public Vector2 position;

    //public LeaderBoardEntry[] leaderboard;
    public List<StageLeaderboard> leaderboardList;
    //public CustomStage[] customStages;
    public List<CustomStage> customStages;


    public GameData()
    {
        playerName = "Player";
        stageName = "";
        stageLevel = 0;
        playerScore = 0;

        audioMuted = false;
        musicVolume = 0.5f;
        soundEffectsVolume = 0.5f;

        isFullScreen = false;
        resolution = 0;
        position = Vector2.zero;

        //leaderboard = new LeaderBoardEntry[10];
        //for (int i = 0; i < 10; i++)
        //{
        //    leaderboard[i] = new LeaderBoardEntry();
        //}
        leaderboardList = new List<StageLeaderboard>();
        if(GameDataController.controller != null)
            foreach (string baseStage in GameDataController.controller.gameStages)
            {
                leaderboardList.Add(new StageLeaderboard(baseStage));
            }
        customStages = new List<CustomStage>();
    }
}
