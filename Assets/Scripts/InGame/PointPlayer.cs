using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

public class PointPlayer : MonoBehaviour
{
    public BallBehaviour ballScript;

    private Vector3 target;
    public GameObject player;
    public GameObject playerSprite;
    public GameObject playerRotationPoint;
    public GameObject ball;
    public GameObject ballShadow;
    public GameObject ballStart;
    public GameObject powerBar;

    public float ballSpeed = 1.0f;
    public float maxSpeed = 20.0f;
    public float maxHold = 3.0f;
    public float maxAnimationScale = 2.0f;

    private float timeStart, timeEnd;
    private Image powerBarFill;

    private bool ballInPlay = false;
    private bool mouseClicked = false;

    private Vector3 ballScale;

    private bool gameRunning = true;
    private Vector2 ballVelocity;

    public UnityEvent spacePressed;

    private bool firingEnabled = true;

    // Start is called before the first frame update
    void Start()
    {
        ballScale = ball.transform.localScale;
        resetBall();


        //powerBar
        powerBarFill = powerBar.transform.Find("Bar").GetComponent<Image>();
        powerBarFill.fillAmount = 0f;
        powerBar.SetActive(false);

        // Setup the cannon
        maxSpeed = PlayerPrefs.GetFloat("maxSpeed", 20.0f);
        maxHold = PlayerPrefs.GetFloat("maxHold", 3.0f);
        //Debug.Log(PlayerPrefs.GetString("spriteName", "Images/Cannon_Sprite_Basic"));
        if (playerSprite == null) Debug.Log("PlayerSprite returned Null");
        playerSprite.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(PlayerPrefs.GetString("spriteName", "Images/Cannon_Sprite_Basic"));

    }

    // Update is called once per frame
    void Update()
    {
        if (gameRunning)
        {
            //target = transform.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z));
            //Getting location of the Mouse 
            target = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z));

            //Calculate rotation according to mouse
            Vector3 difference = target - playerRotationPoint.transform.position;
            float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            if (rotationZ < -90 && rotationZ > -180) rotationZ = 180;
            else if (rotationZ < 0) rotationZ = 0;
            rotationZ -= 90;
            //Update player rotation
            //player.transform.RotateAround(playerRotationPoint.transform.position, Vector3.forward, rotationZ);
            player.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);

            //if (!ballInPlay)
            //{
            if (firingEnabled)
            {
                if (Input.GetMouseButtonDown(0))
                {

                    timeStart = Time.time;
                    //Debug.Log("Mouse clicked at " + timeStart);
                    mouseClicked = true;

                    if (ballInPlay)
                    {
                        AudioManager.Instance.Play("ResetBall");
                        spacePressed.Invoke();
                        resetBall();
                    }
                }

                // Fill poweBar
                if (Input.GetMouseButton(0))
                {
                    if (mouseClicked == true)
                    {
                        float deltaTime = Time.time - timeStart;
                        deltaTime = deltaTime > maxHold ? maxHold : deltaTime;

                        //powerBar
                        powerBar.SetActive(true);
                        powerBarFill.fillAmount = deltaTime / maxHold;
                    }

                }


                if (Input.GetMouseButtonUp(0))
                {
                    if (mouseClicked == true)
                    {
                        mouseClicked = false;
                        // Mininum speed = 1 for time = 0, Maximum speed = 20 for time = 3s
                        timeEnd = Time.time;
                        float distance = difference.magnitude;
                        Vector2 direction = difference / distance;
                        direction.Normalize();
                        fireBall(direction, rotationZ, calculateStartSpeed(timeEnd - timeStart));
                        ball.GetComponent<SpriteRenderer>().enabled = true;
                        ballShadow.GetComponent<SpriteRenderer>().enabled = true;

                        //powerBar
                        powerBarFill.fillAmount = 0f;
                        powerBar.SetActive(false);

                        ballInPlay = true;
                    }
                }
            }
            //}
            //else
            //{
            if (ballInPlay)
            {
                //Pressed space
                if (Input.GetKeyDown("space"))
                {
                    AudioManager.Instance.Play("ResetBall");
                    resetBall();
                    spacePressed.Invoke();
                }

                Vector2 ballVelocity = ball.GetComponent<Rigidbody2D>().velocity;

                // Update ball rotation
                if (ballVelocity.magnitude != 0)
                {
                    rotationZ = Mathf.Atan2(ballVelocity.y, ballVelocity.x) * Mathf.Rad2Deg - 90;
                    ball.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
                }
                // Fit animation speed to ball speed
                ball.GetComponent<Animator>().speed = maxAnimationScale * (ballVelocity.magnitude / maxSpeed);

                //Update ball shadow position
                ballShadow.transform.position = new Vector3(ball.transform.position.x, ball.transform.position.y - 0.02f, ball.transform.position.z);
                //}
            }
        }
    }

    void fireBall(Vector2 direction, float rotationZ, float speed)
    {
        ball.transform.position = ballStart.transform.position;
        ball.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
        ball.GetComponent<Rigidbody2D>().velocity = direction * speed;
        AudioManager.Instance.Play("CannonFire");
    }

    float calculateStartSpeed(float deltaTime)
    {
        deltaTime = deltaTime > maxHold ? maxHold : deltaTime;
        float startSpeed = 1 + maxSpeed * deltaTime / maxHold;
        return startSpeed;
    }

    // Reset ball position and disable
    public void resetBall()
    {
        //Debug.Log("First: " + ball.transform.position.x + ", " + ball.transform.position.y);
        ball.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        ball.GetComponent<Rigidbody2D>().angularVelocity = 0.0f;
        ball.GetComponent<SpriteRenderer>().color = Color.white;
        ball.transform.localScale = ballScale;
        ball.transform.position = ballStart.transform.position;
        //Debug.Log("Second: " + ball.transform.position.x + ", " + ball.transform.position.y);
        ball.GetComponent<SpriteRenderer>().enabled = false;
        ballInPlay = false;
        ballShadow.GetComponent<SpriteRenderer>().enabled = false;
        //Debug.Log("Third: " + ball.transform.position.x + ", " + ball.transform.position.y);
    }

    public void gamePaused()
    {
        gameRunning = !gameRunning;

        if(gameRunning == false)
        {
            //Debug.Log("Mouse click canceled");
            // Cancel the firing of the ball if paused during charge
            mouseClicked = false;
            powerBar.SetActive(false);

            // Remember and restore ball velocity
            ballVelocity = ball.GetComponent<Rigidbody2D>().velocity;
            ball.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
            ball.GetComponent<Rigidbody2D>().angularVelocity = 0.0f;
            ballShadow.transform.position = new Vector3(ball.transform.position.x, ball.transform.position.y - 0.02f, ball.transform.position.z);
            ball.GetComponent<Animator>().speed = 0;
        } else
        {
            ball.GetComponent<Rigidbody2D>().velocity = ballVelocity;
        }
    }

    public void cancelMouseClick()
    {
        mouseClicked = false;
        resetBall();
    }

    public void disableFiring()
    {
        firingEnabled = false;
    }

    public void enableFiring()
    {
        firingEnabled = true;
    }
}
