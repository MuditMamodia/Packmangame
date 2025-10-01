using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class packman_movement_script : MonoBehaviour
{
    public float speed = 5f;   // Movement speed

    private Vector2 moveDirection; // Current movement direction

    public bool stopmoving;

    public timecounter tmc;

    public Vector2 playerposition;

    public bool stopmovingdead;
    public bool playerdead;

    public start_with_currentstatus swc;

    private void Awake()
    {
        playerposition = transform.position;
        
    }

    void Start()
    {
        // Default direction: moving right
        //moveDirection = Vector2.right;
        stopmoving = false;

        tmc = FindObjectOfType<timecounter>();
    }

    void Update()
    {
        HandleInput();

        stopmovingdead = playerdead;

        if (!stopmoving && !stopmovingdead)   // Only move if not stopped
        {
            Move();
        }
    }

    void HandleInput()
    {

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            moveDirection = Vector2.up;
            stopmoving = false;   // Resume movement
            if (!tmc.start_game)
            {
                tmc.start_game = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            moveDirection = Vector2.down;
            stopmoving = false;
            if (!tmc.start_game)
            {
                tmc.start_game = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            moveDirection = Vector2.left;
            stopmoving = false;
            if (!tmc.start_game)
            {
                tmc.start_game = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            moveDirection = Vector2.right;
            stopmoving = false;
            if (!tmc.start_game)
            {
                tmc.start_game = true;
            }
        }
    }

    void Move()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            stopmoving = true;   // Stop when hitting a wall
        }
    }


    public void death_of_player()
    {
        playerdead = true;
        swc.canrestart_checkpooint = true;
    }

    public void restart()
    {
        playerdead = false;
        transform.position = playerposition;
    }
}


