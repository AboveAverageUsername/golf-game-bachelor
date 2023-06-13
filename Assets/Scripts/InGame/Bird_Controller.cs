using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird_Controller : MonoBehaviour
{

    private float cameraH, cameraW;
    public float birdSpeed = 5.0f;
    private float spawnDistance;
    private Vector2 birdSize;
    private bool gameRunning = true;
    private Vector2 birdVelocity;

    // Start is called before the first frame update
    void Start()
    {
        cameraH = UnityEngine.Camera.main.orthographicSize;
        cameraW = cameraH * UnityEngine.Camera.main.aspect;
        birdSpeed = birdSpeed + Random.Range(0, 5);
        birdSize = GetComponent<SpriteRenderer>().size;
        spawnDistance = cameraW + 5 * birdSize.x;
        transform.position = new Vector3(-spawnDistance, transform.position.y, transform.position.z);
        GetComponent<Rigidbody2D>().velocity = new Vector2(birdSpeed, 0.0f);
    }

    private void OnEnable()
    {
        Start();
        gameRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < -spawnDistance || transform.position.x > spawnDistance)
        {
            GetComponent<SpriteRenderer>().flipX = transform.position.x > 0;
            GetComponent<Rigidbody2D>().velocity = new Vector2(transform.position.x > 0 ? -birdSpeed : birdSpeed, 0);
            transform.position = transform.position.x > 0 ? new Vector3(spawnDistance - birdSize.x, transform.position.y, transform.position.z) : new Vector3(-spawnDistance + birdSize.x, transform.position.y, transform.position.z);
            //Debug.Log("Birb switcharoo!");
        }
    }


    public void gamePaused()
    {
        //Debug.Log("Bird " + gameObject.name + (gameRunning?" paused":" resumed"));
        if (gameRunning)
        {
            birdVelocity = GetComponent<Rigidbody2D>().velocity;
            GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
            GetComponent<Animator>().speed = 0;
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = birdVelocity;
            GetComponent<Animator>().speed = 1;
        }

        gameRunning = !gameRunning;
    }
}
