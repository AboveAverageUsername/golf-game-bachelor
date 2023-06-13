using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BallBehaviour : MonoBehaviour
{
    public PauseController pauseController;

    public GameObject ballShadow;
    public float speedThreshold;
    public PointPlayer playerScript;
    private Vector3 originalScale;

    // Script for UI
    public InGameUI UIScript;
    public int yellowHole = 100;
    public int orangeHole = 300;
    public int redHole = 500;
    public int coinPoints = 200;

    private Coroutine teleporting = null;
    private bool justTeleported = false;
    private bool holeHit = false;
    private bool ballBoosted = false;

    public UnityEvent levelComplete;

    public bool pausedGame = false;

    // Start is called before the first frame update
    void Start()
    {
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(transform.position.x + ", " + transform.position.y);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if((collision.gameObject.CompareTag("Obstacle") && !collision.gameObject.name.Contains("Bush")) || collision.gameObject.CompareTag("Tree") || collision.gameObject.CompareTag("Border"))
        {
            AudioManager.Instance.Play("TreeHit");
        }
        else if (collision.gameObject.CompareTag("Obstacle") && collision.gameObject.name.Contains("Bush"))
        {
            AudioManager.Instance.Play("BushHit");
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Detected collision with " + collision.gameObject.name);
        if (collision.gameObject.tag.Contains("Hole"))
        {
            if (transform.GetComponent<Rigidbody2D>().velocity.magnitude <= speedThreshold)
            {
                // Give points for Succesfull Hole hit
                switch (collision.gameObject.tag)
                {
                    case "Hole_Yellow":
                        UIScript.increaseScore(yellowHole);
                        break;
                    case "Hole_Orange":
                        UIScript.increaseScore(orangeHole);
                        break;
                    case "Hole_Red":
                        UIScript.increaseScore(redHole);
                        break;
                    default: break;
                }
                holeHit = true;
                StartCoroutine("fallInHole");
            }
        }
        else if (collision.gameObject.tag == "Lake" && !ballBoosted)
        {
            StartCoroutine("fallInHole");
        }
        // Bird hit the ball
        else if (collision.gameObject.CompareTag("Bird"))
        {
            AudioManager.Instance.Play("BirdPickUp");
            UIScript.decreaseHealth(1);
            playerScript.resetBall();
            ballBoosted = false;
        }
        // Reduce Drag for Ice, Increase Drag for Mud
        else if (collision.gameObject.tag == "Ice" && !ballBoosted)
        {
            transform.GetComponent<Rigidbody2D>().drag = 0.0f;
        }
        else if (collision.gameObject.tag == "Mud" && !ballBoosted)
        {
            transform.GetComponent<Rigidbody2D>().drag = 3.0f;
        }

        else if (collision.gameObject.CompareTag("Teleporter"))
        {
            if (!justTeleported)
            {
                //Debug.Log("Arrived at first teleporter");
                justTeleported = true;
                AudioManager.Instance.Play("Teleport");
                teleporting = StartCoroutine(teleportBall(collision.gameObject));
                //transform.position = collision.gameObject.GetComponent<Teleporter>().getDestination().position;
            }
            else
            {
                //Debug.Log("Arrived at other teleporter");
                AudioManager.Instance.Play("Teleport");
                justTeleported = false;
            }
        }
        else if (collision.gameObject.tag.Contains("Coin"))
        {
            AudioManager.Instance.Play("CoinPickUp");
            switch (collision.gameObject.tag) {
                case "Coin_Gold":
                    UIScript.increaseScore(coinPoints);
                    Destroy(collision.gameObject);
                    break;
                case "Coin_Red":
                    Destroy(collision.gameObject);
                    UIScript.increaseHealth(5);
                    break;
                case "Coin_Grey":
                    UIScript.increaseTime();
                    Destroy(collision.gameObject);
                    break;
                case "Coin_Blue":
                    ballBoosted = true;
                    GetComponent<SpriteRenderer>().color = Color.blue;
                    Destroy(collision.gameObject);
                    break;
                default: break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Ice" || collision.gameObject.tag == "Mud")
        {
            transform.GetComponent<Rigidbody2D>().drag = 0.7f;
        }
    }

    IEnumerator fallInHole()
    {
        //Debug.Log("Ball fell");
        pauseController.disableEsc();
        playerScript.disableFiring();
        transform.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        ballShadow.GetComponent<SpriteRenderer>().enabled = false;
        AudioManager.Instance.Play("HoleDrop");
        for(float f = 1f; f >= 0.4f; f -= 0.05f)
        {
            //Debug.Log(f);
            transform.localScale = originalScale * f;
            yield return new WaitForSeconds(0.05f);
        }
        playerScript.resetBall();
        ballBoosted = false;

        pauseController.enableEsc();
        if (holeHit)
        {
            AudioManager.Instance.Play("HoleHit");
            levelComplete.Invoke();
            holeHit = false;
        }
        else
        {
            AudioManager.Instance.Play("WaterPlup");
            UIScript.decreaseHealth(1);
        }
        playerScript.enableFiring();
    }

    IEnumerator teleportBall(GameObject teleporterIn)
    {
        Vector2 velocity = transform.GetComponent<Rigidbody2D>().velocity;
        float angluarVelocity = transform.GetComponent<Rigidbody2D>().angularVelocity;
        transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        transform.GetComponent<Rigidbody2D>().angularVelocity = 0;
        for (float i = 0.5f; i >= 0; i-=Time.deltaTime)
        {
            transform.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 2*i);
            yield return null;
            while (pausedGame)
            {
                yield return new WaitForSeconds(0.2f);
            }
        }


        transform.position = teleporterIn.GetComponent<Teleporter>().getDestination().position;

        for (float i = 0; i <= 0.5f; i += Time.deltaTime)
        {
            transform.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 2*i);
            yield return null;
            while (pausedGame)
            {
                yield return new WaitForSeconds(0.2f);
            }
        }

        transform.GetComponent<Rigidbody2D>().velocity = velocity;
        transform.GetComponent<Rigidbody2D>().angularVelocity = angluarVelocity;

        teleporting = null;
    }

    public void cancelTeleport()
    {
        if(teleporting != null)
        StopCoroutine(teleporting);
        justTeleported = false;
        teleporting = null;
    }

    public void pauseCoroutine()
    {
        pausedGame = !pausedGame;
    }
}
