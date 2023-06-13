using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CoinController : MonoBehaviour
{

    public GameObject border;
    public GameObject goldCoin;
    public GameObject redCoin;
    public GameObject blueCoin;
    public GameObject greyCoin;
    public float coinSpawnTimer = 5;

    private bool spawn = true;
    private bool coinHit = false;

    private float w, h;
    private float currTime;

    private bool gameRunning = true;
    private bool pausedDuringWait = false;

    private GameObject newCoin;

    // Start is called before the first frame update
    void Start()
    {
        w = border.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        h = border.GetComponent<SpriteRenderer>().bounds.size.y / 2;
        StartCoroutine(spawnCoins());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void hitCoin()
    {
        coinHit = true;
    }

    public void stopSpawn()
    {
        spawn = false;
    }

    private IEnumerator spawnCoins()
    {
        newCoin = null;
        //Collider2D[] colWith = new Collider2D[10];
        List<Collider2D> colRes = new List<Collider2D>();
        ContactFilter2D colFltr = new ContactFilter2D();
        bool validSpawn = false;
        GameObject coinOfChoice = goldCoin;
        while (spawn)
        {
            validSpawn = false;

            switch (Random.Range(0, 4))
            {
                case 0:
                    coinOfChoice = goldCoin;
                    break;
                case 1:
                    coinOfChoice = redCoin;
                    break;
                case 2:
                    coinOfChoice = blueCoin;
                    break;
                case 3:
                    coinOfChoice = greyCoin;
                    break;
                default:
                    coinOfChoice = goldCoin;
                    break;
            }
            while (!validSpawn)
            {
                float randX = Random.Range(border.transform.position.x - w, border.transform.position.x + w);
                float randY = Random.Range(border.transform.position.y - h, border.transform.position.y + h);
                colRes.Clear();
                if (newCoin != null) Destroy(newCoin);

                newCoin = Instantiate(coinOfChoice, new Vector3(randX, randY, 0.0f), Quaternion.Euler(0, 0, 0));

                Collider2D col = newCoin.GetComponent<CapsuleCollider2D>();
                if (col.OverlapCollider(colFltr.NoFilter(), colRes) > 0)
                {
                    validSpawn = true;
                    foreach (Collider2D element in colRes)
                    {
                        if(!element.gameObject.CompareTag("Mud") && !element.gameObject.CompareTag("Ice"))
                        {
                            validSpawn = false;
                            break;
                        }
                    }
                }
                else break;
            }

            currTime = Time.time;
            yield return new WaitForSeconds(Random.Range(coinSpawnTimer - 2 > 0 ? coinSpawnTimer - 2 : coinSpawnTimer, coinSpawnTimer));

            // Wait for game to be unpaused
            while (!gameRunning)
            {
                yield return null;
            }
            // Continue the waiting where it was left off
            if (pausedDuringWait)
            {
                yield return new WaitForSeconds(currTime);
                pausedDuringWait = false;
            }

                if (!coinHit)
            {
                if(newCoin != null) Destroy(newCoin);
                newCoin = null;
            }
        }
    }

    public void gamePaused()
    {
        if (gameRunning)
        {
            currTime = Time.time - currTime;
            if (newCoin != null) newCoin.GetComponent<Animator>().speed = 0;
        }
        else
        {
            if (newCoin != null) newCoin.GetComponent<Animator>().speed = 1;
        }
        pausedDuringWait = true;
        gameRunning = !gameRunning;
    }
}
