using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{

    public List<GameObject> obstacles;
    public List<GameObject> effectTiles;
    public List<GameObject> teleporters;
    public List<GameObject> birds;
    public List<GameObject> lakes;

    public List<GameObject> coins;
    public List<GameObject> holes;

    // All prefabs
    public GameObject Tree_1;
    public GameObject Tree_2;
    public GameObject Tree_3;
    public GameObject Tree_4;
    public GameObject Stump;
    public GameObject Log;
    public GameObject Bush_1;
    public GameObject Bush_2;
    public GameObject Bush_3;
    public GameObject Hole_Yellow;
    public GameObject Hole_Orange;
    public GameObject Hole_Red;
    public GameObject Ice_Small_1;
    public GameObject Ice_Medium_1;
    public GameObject Ice_Big_1;
    public GameObject Mud_Small_1;
    public GameObject Mud_Medium_1;
    public GameObject Mud_Big_1;
    public GameObject Teleporter;
    public GameObject Bird_Flying;
    public GameObject Lake_Small_1;
    public GameObject Lake_Small_2;
    public GameObject Lake_Small_3;
    public GameObject Lake_Medium_1;
    public GameObject Lake_Medium_2;
    public GameObject Lake_Big_1;
    public GameObject Coin;

    public GameObject levelEndScreen;
    public TextMeshProUGUI endText;
    public GameObject retryBtn;
    public GameObject nextLvlBtn;
    public GameObject border;
    public GameObject victoryScreen;
    public GameObject newRecord;
    public TextMeshProUGUI scoreText;

    public PauseController pauseScript;
    public TextMeshProUGUI levelText;


    private GameObject firstTeleporter;
    private int level;

    public string[] audioTracks;

    public string stageName;
    //private int trackIndex;
    //private bool songChanged = false;

    private void Awake()
    {
        if(SceneManager.GetActiveScene().name == GameDataController.controller.customStageName)
        {
            GameData.CustomStage currStage = null; 
            foreach (GameData.CustomStage elem in GameDataController.controller.data.customStages)
            {
                if (GameDataController.controller.data.stageName == elem.stageName)
                {
                    currStage = elem;
                    break;
                }
            }
            firstTeleporter = null;
            foreach (GameData.StageObstacle elem in currStage.allElements)
            {
                switch (elem.name)
                {
                    case "Tree_1":
                        GameObject newTree1 = Instantiate(Tree_1, elem.position, Quaternion.Euler(Vector3.zero));
                        //newTree1.GetComponent<SpriteRenderer>().sortingOrder = setTreeLayerOrder(newTree1.transform);
                        obstacles.Add(newTree1);                      
                        break;
                    case "Tree_2":
                        GameObject newTree2 = Instantiate(Tree_2, elem.position, Quaternion.Euler(Vector3.zero));
                        //newTree2.GetComponent<SpriteRenderer>().sortingOrder = setTreeLayerOrder(newTree2.transform);
                        obstacles.Add(newTree2);
                        break;
                    case "Tree_3":
                        GameObject newTree3 = Instantiate(Tree_3, elem.position, Quaternion.Euler(Vector3.zero));
                        //newTree3.GetComponent<SpriteRenderer>().sortingOrder = setTreeLayerOrder(newTree3.transform);
                        obstacles.Add(newTree3);
                        break;
                    case "Tree_4":
                        GameObject newTree4 = Instantiate(Tree_4, elem.position, Quaternion.Euler(Vector3.zero));
                        //newTree4.GetComponent<SpriteRenderer>().sortingOrder = setTreeLayerOrder(newTree4.transform);
                        obstacles.Add(newTree4);
                        break;
                    case "Stump":
                        obstacles.Add(Instantiate(Stump, elem.position, Quaternion.Euler(Vector3.zero)));
                        break;
                    case "Log":
                        obstacles.Add(Instantiate(Log, elem.position, Quaternion.Euler(Vector3.zero)));
                        break;
                    case "Bush_1":
                        obstacles.Add(Instantiate(Bush_1, elem.position, Quaternion.Euler(Vector3.zero)));
                        break;
                    case "Bush_2":
                        obstacles.Add(Instantiate(Bush_2, elem.position, Quaternion.Euler(Vector3.zero)));
                        break;
                    case "Bush_3":
                        obstacles.Add(Instantiate(Bush_3, elem.position, Quaternion.Euler(Vector3.zero)));
                        break;
                    case "Hole_Yellow":
                        holes.Add(Instantiate(Hole_Yellow, elem.position, Quaternion.Euler(Vector3.zero)));
                        break;
                    case "Hole_Orange":
                        holes.Add(Instantiate(Hole_Orange, elem.position, Quaternion.Euler(Vector3.zero)));
                        break;
                    case "Hole_Red":
                        holes.Add(Instantiate(Hole_Red, elem.position, Quaternion.Euler(Vector3.zero)));
                        break;
                    case "Ice_Small_1":
                        effectTiles.Add(Instantiate(Ice_Small_1, elem.position, Quaternion.Euler(Vector3.zero)));
                        break;
                    case "Ice_Medium_1":
                        effectTiles.Add(Instantiate(Ice_Medium_1, elem.position, Quaternion.Euler(Vector3.zero)));
                        break;
                    case "Ice_Big_1":
                        effectTiles.Add(Instantiate(Ice_Big_1, elem.position, Quaternion.Euler(Vector3.zero)));
                        break;
                    case "Mud_Small_1":
                        effectTiles.Add(Instantiate(Mud_Small_1, elem.position, Quaternion.Euler(Vector3.zero)));
                        break;
                    case "Mud_Medium_1":
                        effectTiles.Add(Instantiate(Mud_Medium_1, elem.position, Quaternion.Euler(Vector3.zero)));
                        break;
                    case "Mud_Big_1":
                        effectTiles.Add(Instantiate(Mud_Big_1, elem.position, Quaternion.Euler(Vector3.zero)));
                        break;
                    case "Teleporter":
                        GameObject newTeleporter = Instantiate(Teleporter, elem.position, Quaternion.Euler(Vector3.zero));
                        if (firstTeleporter == null)
                        {
                            firstTeleporter = newTeleporter;
                            teleporters.Add(newTeleporter);
                        } else
                        {
                            firstTeleporter.GetComponent<Teleporter>().destination = newTeleporter;
                            newTeleporter.GetComponent<Teleporter>().destination = firstTeleporter;
                            firstTeleporter = null;
                            teleporters.Add(newTeleporter);
                        }
                        break;
                    case "Bird_Flying":
                        birds.Add(Instantiate(Bird_Flying, elem.position, Quaternion.Euler(Vector3.zero)));
                        break;
                    case "Lake_Small_1":
                        lakes.Add(Instantiate(Lake_Small_1, elem.position, Quaternion.Euler(Vector3.zero)));
                        break;
                    case "Lake_Small_2":
                        lakes.Add(Instantiate(Lake_Small_2, elem.position, Quaternion.Euler(Vector3.zero)));
                        break;
                    case "Lake_Small_3":
                        lakes.Add(Instantiate(Lake_Small_3, elem.position, Quaternion.Euler(Vector3.zero)));
                        break;
                    case "Lake_Medium_1":
                        lakes.Add(Instantiate(Lake_Medium_1, elem.position, Quaternion.Euler(Vector3.zero)));
                        break;
                    case "Lake_Medium_2":
                        lakes.Add(Instantiate(Lake_Medium_2, elem.position, Quaternion.Euler(Vector3.zero)));
                        break;
                    case "Lake_Big_1":
                        lakes.Add(Instantiate(Lake_Big_1, elem.position, Quaternion.Euler(Vector3.zero)));
                        break;
                    default:
                        break;
                }
            }
            for (int i = 0; i < currStage.numOfCoins; i++)
            {
                coins.Add(Instantiate(Coin));
            }
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        stageName = GameDataController.controller.data.stageName;
        Cursor.visible = false;

        level = GameDataController.controller.data.stageLevel;
        if (level == 0)
        {
            level = 1;
            GameDataController.controller.data.stageLevel = 1;
            //GameDataController.controller.saveLevelProgress(1);
            //Debug.Log("New game started");
        }

        // Start music
        //trackIndex = 0;
        AudioManager.Instance.swapTracks(audioTracks[level - 1], 2.0f);

        levelText.text = "LEVEL " + level;

        foreach (GameObject elem in obstacles)
        {
            if (elem.tag.Contains("Tree"))
            {
                elem.GetComponent<SpriteRenderer>().sortingOrder = setTreeLayerOrder(elem.transform);
            }
        }

        if(level < 2)
        foreach (GameObject elem in effectTiles)
        {
            elem.SetActive(false);
        }

        if(level < 3)
        foreach (GameObject elem in teleporters)
        {
            elem.SetActive(false);
        }
   
        foreach (GameObject elem in birds)
        {
            pauseScript.pausedGame.AddListener(elem.GetComponent<Bird_Controller>().gamePaused);
            if (level < 4)
                elem.SetActive(false);
        }

        if(level < 5)
        foreach (GameObject elem in lakes)
        {
            elem.SetActive(false);
        }

        foreach (GameObject elem in coins)
        {
            pauseScript.pausedGame.AddListener(elem.GetComponent<CoinController>().gamePaused);
            elem.GetComponent<CoinController>().border = border;
        }

    }

    // Update is called once per frame
    void Update()
    {
        // Start new track if old is done
        //if (!AudioManager.Instance.trackPlaying.source.isPlaying && !songChanged)
        //{
        //    trackIndex = (trackIndex + 1) % audioTracks.Length;
        //    //AudioManager.Instance.swapTracks(audioTracks[level - 1], 4.0f);
        //    songChanged = true;
        //}
        //else if(AudioManager.Instance.trackPlaying.source.isPlaying && songChanged)
        //{
        //    songChanged = false;
        //}
        //Debug.Log("Size is " + audioTracks.Length);
    }

    public void levelCompleted()
    {
        if (level == 0) level = 1;
        if (level < 5)
        {
            levelEndScreen.SetActive(true);
            endText.text = "Level Complete";
            retryBtn.SetActive(false);
            nextLvlBtn.SetActive(true);
            GameDataController.controller.saveLevelProgress(level + 1);
        }
        else
        {
            AudioManager.Instance.trackPlaying.source.Pause();
            AudioManager.Instance.Play("WinJingle");
            victoryScreen.SetActive(true);
            scoreText.text = "Score:\n" + GameDataController.controller.data.playerName + " " + GameDataController.controller.data.playerScore; 
            bool gotRecord = GameDataController.controller.addRecord(GameDataController.controller.data.stageName);
            newRecord.SetActive(gotRecord);
        }
        //Debug.Log("Index is " + (level) + ", while size is " + audioTracks.Length);
    }

    public void nextLevel()
    {
        levelEndScreen.SetActive(false);
        if(level < 5)
        {
            levelEndScreen.SetActive(false);
            //Debug.Log(level);
            level++;
            //Debug.Log("Index is " + (level - 1) + ", while size is " + audioTracks.Length);
            AudioManager.Instance.swapTracks(audioTracks[level - 1], 2.0f);
            //Debug.Log(level);
            levelText.text = "LEVEL " + level;
            switch (level)
            {
                case 2:
                    foreach (GameObject elem in effectTiles)
                    {
                        elem.SetActive(true);
                    }
                    break;
                case 3:
                    foreach (GameObject elem in teleporters)
                    {
                        elem.SetActive(true);
                    }
                    break;
                case 4:
                    foreach (GameObject elem in birds)
                    {
                        elem.SetActive(true);
                    }
                    break;
                case 5:
                    foreach (GameObject elem in lakes)
                    {
                        elem.SetActive(true);
                    }
                    break;
                default:
                    break;
            }
        } else
        {

        }
    }

    public void gamePaused()
    {
        //Debug.Log(level);
        //if(level >= 4)
        //    foreach (GameObject e in birds)
        //    {
        //        e.GetComponent<Bird_Controller>().gamePaused();
        //    }
    }

    public void gameOver()
    {
        AudioManager.Instance.trackPlaying.source.Pause();
        AudioManager.Instance.Play("GameOver");
        levelEndScreen.SetActive(true);
        endText.text = "Game Over";
        nextLvlBtn.SetActive(false);
        retryBtn.SetActive(true);
    }

    public void retryLevel()
    {
        if(!AudioManager.Instance.trackPlaying.source.isPlaying) AudioManager.Instance.trackPlaying.source.Play();
        levelEndScreen.SetActive(false);
    }

    // Go back to stage select after Victory
    public void finishGame()
    {
        SceneManager.LoadScene(1);
    }

    public int setTreeLayerOrder(Transform t)
    {
        return Mathf.RoundToInt(t.position.y * 100f) * -1;
    }

    public void goToLeaderboard()
    {
        PlayerPrefs.SetString("leaderboard", stageName);
        SceneManager.LoadSceneAsync("Leaderboard");
    }
}
