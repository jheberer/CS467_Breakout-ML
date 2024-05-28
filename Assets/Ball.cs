using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Ball : MonoBehaviour
{
    // make ball a singleton so we can reference it from other objects like brick
    public static Ball instance { get; private set; }
    Rigidbody2D ball_rb;
    float initial_speed = 6.0f;
    float min_Y = -6.0f;
    float max_velocity = 11f;
    // *===========================CHANGES-JP==========================================*
    // made is_moving to a public variable
    // createdd public bool variable gameStart and gameStop
    public bool isGameStart = false;
    public bool game_won = false;
    public float x_position = 0;
    public float y_position = 0;
    public int bricksHit = 0;
    public float min_vertical_speed = 1f;
    public bool is_moving = false;
    public int score = 0;
    public int lives = 5;
    public TextMeshProUGUI score_text;
    public GameObject[] lives_image;
    public GameObject game_over_splash;
    public GameObject victory_splash;
    public bool game_over = false;
    float previous_velocity_y = 0f;
    public int brick_count;

    // for audio
    public AudioSource paddle_sound;
    public AudioSource brick_sound;
    public AudioSource life_lost_sound;
    public AudioSource game_over_sound;
    public AudioSource victory_sound;


    void GameOver()
    {
        Debug.Log("game over");
        game_over_sound.Play();
        game_over_splash.SetActive(true);
        game_over = true;
        Time.timeScale = 0;
    }
    void Victory()
    {
        Debug.Log("victory");
        victory_sound.Play();
        victory_splash.SetActive(true);
        game_won = true;
        game_over = true;
        Time.timeScale = 0;
    }

    // code citation
    // purpose of code: help make this game object class a singleton, so that
    // its static variables can be referenced by other game objects
    // link: https://www.youtube.com/watch?v=yhlyoQ2F-NM
    // code snippet at time of video: 2:21
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    void Start()
    {
        GameObject[] bricks = GameObject.FindGameObjectsWithTag("Brick");
        brick_count = bricks.Length;
    }

    // Update is called once per frame
    void Update()
    {
        x_position = transform.position.x;
        y_position = transform.position.y;
        // this references the rigid body component of this ball
        ball_rb = GetComponent<Rigidbody2D>();

        if (game_over)
        {
            return;
        }
        // win
        if (brick_count == 0)
        {
            Victory();
        }

        if (!is_moving)
        {
            // this puts the ball "in-play", and is only available when ball is
            // in the starting position
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isGameStart = true;
                // transform.translate will only apply discrete movements when
                // space is held. Need a vector instead
                // give it a little lateral movement so game start is interesting

                float random_angle = Random.Range(40f, 140f);
                float random_angle_rads = random_angle * Mathf.Deg2Rad;
                float vx = Mathf.Cos(random_angle_rads);
                float vy = Mathf.Sin(random_angle_rads);

                Vector3 initial_velocity = new Vector3(vx, -vy, 0f).normalized * initial_speed;

                ball_rb.velocity = initial_velocity;
                is_moving = true;
            }
        }

        // if ball is moving or "in play"
        else
        {
            // this is a test key for testing various behaviors
            // if (Input.GetKeyDown(KeyCode.T)) 
            // {
            //     // Get the current velocity
            //     Vector3 current_velocity = ball_rb.velocity;

            //     // Set the vertical component of the velocity to 0
            //     current_velocity.y = 0f;

            //     // Apply the modified velocity to the Rigidbody
            //     ball_rb.velocity = current_velocity;
            // }



            // if the ball falls below the screen
            if (transform.position.y < min_Y)
            {
                isGameStart = false;
                if (lives <= 0)
                {
                    GameOver();
                }

                else
                {
                    isGameStart = false;
                    transform.position = Vector3.zero;
                    ball_rb.velocity = new Vector3(0, 0, 0);
                    is_moving = false;
                    lives--;
                    lives_image[lives].SetActive(false);
                    life_lost_sound.Play();
                }
            }

            // cap max speed so game doesn't become too hard
            if (ball_rb.velocity.magnitude > max_velocity)
            {
                ball_rb.velocity = Vector3.ClampMagnitude(ball_rb.velocity, max_velocity);
            }

            // add minimum vertical speed (absolute) so ball doesn't become "stuck"
            // DEV note: Even though it violates DRY, I needed to repeat the is_moving
            // check here to avoid issues on restart. Needs further investigation
            if (Mathf.Abs(ball_rb.velocity.y) < min_vertical_speed && is_moving)
            {
                Debug.Log("Apply minimum vertical force");
                float vertical_force = min_vertical_speed - Mathf.Abs(ball_rb.velocity.y);
                // us direction prior to ball becoming "stuck"
                Vector2 vertical_force_direction = Vector2.up * Mathf.Sign(previous_velocity_y);
                ball_rb.AddForce(vertical_force_direction * vertical_force, ForceMode2D.Impulse);
            }
        }

        // Debug.Log(ball_rb.velocity.magnitude);
        score_text.text = score.ToString("000000");

        previous_velocity_y = ball_rb.velocity.y;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Paddle"))
        {
            paddle_sound.Play();
        }
        if (other.gameObject.CompareTag("Brick"))
        {
            bricksHit = bricksHit + 1;
            brick_sound.Play();
        }
    }

    public void ResetIsMoving()
    {
        is_moving = false;
    }


}
