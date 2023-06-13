using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class InGameUI : MonoBehaviour
{

    private const int maxHealth = 5;
    public GameObject[] hearts;

    // Pocetno vreme u sekundama
    public int startTime = 60;

    private int health;
    public TextMeshProUGUI healthText;

    private int score;
    private int scoreStart = 0;
    public TextMeshProUGUI scoreText;

    private float time;
    public TextMeshProUGUI timeText;
    public GameObject timeBar;


    private bool gameRunning = true;

    public UnityEvent gameOver;


    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;

        score = GameDataController.controller.data.playerScore;

        time = startTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameRunning)
        {
            scoreText.text = "SCORE: " + score;
            healthText.text = health.ToString();


            if (time > 0)
            {
                time -= Time.deltaTime;
            }
            else
            {
                time = 0;
                gameOver.Invoke();
            }
            displayTime(time);
            timeBar.GetComponent<Image>().fillAmount = time / (float)startTime;
        }
    }



    public void increaseScore(int amount)
    {
        score += amount;
    }

    public void increaseHealth(int amount)
    {
        health += amount;
        if(health > 5) health = 5;
        //hearts[health - 1].GetComponent<SpriteRenderer>().enabled = true;
        for (int i = 0; i < health; i++)
        {
            hearts[i].SetActive(true);
        }
        
    }

    public void decreaseHealth(int amount)
    {
        health -= amount;
        if(health == 0) gameOver.Invoke();
        if (health < 0)
        {
            health = 0;
        }
        //hearts[health].GetComponent<SpriteRenderer>().enabled = false;
        hearts[health].SetActive(false);
    }

    public void displayTime(float timeToDispley)
    {
        if(timeToDispley < 0)
        {
            timeToDispley = 0;
        }
        timeText.text = string.Format("TIME: {0:00}:{1:00}", Mathf.FloorToInt(timeToDispley / 60), Mathf.FloorToInt(timeToDispley % 60));

        //timeText.text = "TIME: " + Mathf.FloorToInt(timeToDispley / 60) + ":" + Mathf.FloorToInt(timeToDispley % 60);
    }

    public void increaseTime()
    {
        time += startTime / 3;
        if (time > startTime) time = startTime;
    }


    public void gamePaused()
    {
        gameRunning = !gameRunning;
    }

    public void retryLevel()
    {
        score = scoreStart;
        health = 5;
        for(int i = 0; i < maxHealth; i++)
        {
            hearts[i].SetActive(true);
        }
        time = startTime;
    }

    public void levelComplete()
    {
        health = 5;
        for (int i = 0; i < 5; i++)
        {
            hearts[i].SetActive(true);
        }
        time = startTime;
        scoreStart = score;
        GameDataController.controller.saveScoreProgress(score);
    }
}
