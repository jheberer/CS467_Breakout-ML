// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using TMPro;

// public class AgentBall : MonoBehaviour
// {
//     public static AgentBall agentInstance { get; private set; }

//     private Rigidbody2D agent_ball_rb;
//     private float initial_speed = 6.0f;
//     private float min_Y = -6.0f;
//     private float max_velocity = 11f;
//     public float min_vertical_speed = 1f;
//     private bool is_moving = false;

//     public bool game_won = false;
//     public int agent_lives = 5;
//     public int agent_score = 0;

//     public TextMeshProUGUI score_text_agent;

//     public GameObject[] agent_lives_image;
//     public bool game_over = false;
//     private float previous_velocity_y = 0f;
//     public int agent_brick_count;

//     void Awake()
//     {
//         if (gameObject.CompareTag("AgentBall"))
//         {
//             if (agentInstance != null && agentInstance != this)
//             {
//                 Destroy(gameObject);
//             }
//             else
//             {
//                 agentInstance = this;
//                 Debug.Log("Agent Ball instance is initialized.");
//             }
//         }
//     }

//     void Start()
//     {
//         GameObject[] bricks = GameObject.FindGameObjectsWithTag("AgentBrick");
//         agent_brick_count = bricks.Length;
//     }

//     void Update()
//     {
//         agent_ball_rb = GetComponent<Rigidbody2D>();
//         if (game_over)
//         {
//             return;
//         }

//         if (agent_brick_count == 0)
//         {
//             return;
//         }

//         if (!is_moving)
//         {
//             StartBallMovement();
//         }
//         else
//         {
//             if (transform.position.y < min_Y)
//             {
//                 Debug.Log("Ball has fallen below the screen.");
//                 if (agent_lives <= 0)
//                 {
//                     return;
//                 }
//                 else
//                 {
//                     agent_lives--;
//                     ResetBall();
//                     agent_lives_image[agent_lives].SetActive(false);
//                     StartBallMovement();
//                 }
//             }

//             if (agent_ball_rb.velocity.magnitude > max_velocity)
//             {
//                 agent_ball_rb.velocity = Vector3.ClampMagnitude(agent_ball_rb.velocity, max_velocity);
//             }

//             if (Mathf.Abs(agent_ball_rb.velocity.y) < min_vertical_speed && is_moving)
//             {
//                 float vertical_force = min_vertical_speed - Mathf.Abs(agent_ball_rb.velocity.y);
//                 Vector2 vertical_force_direction = Vector2.up * Mathf.Sign(previous_velocity_y);
//                 agent_ball_rb.AddForce(vertical_force_direction * vertical_force, ForceMode2D.Impulse);
//             }
//         }

//         score_text_agent.text = agent_score.ToString("000000");
//         previous_velocity_y = agent_ball_rb.velocity.y;
//     }

//     public void ResetIsMoving()
//     {
//         is_moving = false;
//     }

//     public void ResetBall()
//     {
//         agent_ball_rb = GetComponent<Rigidbody2D>();
//         if (gameObject.CompareTag("AgentBall"))
//         {
//             transform.position = new Vector2(-20, -0.5f);
//         }
//         agent_ball_rb.velocity = Vector3.zero;
//         is_moving = false;
//         Debug.Log("Ball has been reset.");
//     }

//     public void StartBallMovement()
//     {
//         agent_ball_rb = GetComponent<Rigidbody2D>();
//         float random_angle = Random.Range(40f, 140f);
//         float random_angle_rads = random_angle * Mathf.Deg2Rad;
//         float vx = Mathf.Cos(random_angle_rads);
//         float vy = Mathf.Sin(random_angle_rads);

//         Vector2 initial_velocity = new Vector2(vx, -vy).normalized * initial_speed;
//         agent_ball_rb.velocity = initial_velocity;
//         is_moving = true;
//         Debug.Log("Ball movement started.");
//     }
// }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class AgentBall : MonoBehaviour
{
    // make ball a singleton so we can reference it from other objects like brick
    public static AgentBall instance { get; private set; }
    Rigidbody2D ball_rb;
    float initial_speed = 6.0f;
    // float min_Y = -4.5f;
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
    private Rigidbody2D agent_ball_rb;

    // for audio
    public AudioSource paddle_sound;
    public AudioSource brick_sound;
    public AudioSource life_lost_sound;
    public AudioSource game_over_sound;
    public AudioSource victory_sound;

    public bool isGamePaused = false;

    public void PauseGame()
    {
        isGamePaused = true;
    }

    public void ResumeGame()
    {
        isGamePaused = false;
    }

    void GameOver()
    {
        PauseGame();
        Debug.Log("game over in AgentBall");
        game_over_sound.Play();
        game_over_splash.SetActive(true);
        game_over = true;
        Time.timeScale = 0;
    }
    void Victory()
    {
        Debug.Log("victory in AgentBall");
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
        if (!isGamePaused)
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
                StartBallMovement();

            }

            // if ball is moving or "in play"
            else
            {
                // if the ball falls below the screen
                if (transform.position.y < min_Y)
                {
                    Paddle paddle = FindObjectOfType<Paddle>();
                    if (paddle != null)
                    {
                        paddle.LostBall(); // Replace with your actual function name
                    }
                    // Debug.Log("inside agentball. ball was lost");
                    isGameStart = false;
                    is_moving = false;
                    if (lives <= 0)
                    {
                        GameOver();
                    }

                    else
                    {
                        isGameStart = false;
                        transform.position = new Vector3(-20, 0, 0);
                        ball_rb.velocity = new Vector3(-20, 0, 0);
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
            score_text.text = score.ToString("000000");

            previous_velocity_y = ball_rb.velocity.y;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Paddle"))
        {
            paddle_sound.Play();
        }
        // else
        // {
        // Debug.Log("AgentBall -  inside hitting brick =" + other.gameObject.tag);
        // }
        if (other.gameObject.CompareTag("AgentBrick"))
        {
            // Debug.Log("compareTag - inside hitting brick ");
            bricksHit = bricksHit + 1;
            brick_sound.Play();
        }
    }

    public void ResetIsMoving()
    {
        is_moving = false;
    }

    public void StartBallMovement()
    {
        // Debug.Log("agentball ====== is_moving = " + is_moving);
        agent_ball_rb = GetComponent<Rigidbody2D>();
        float random_angle = Random.Range(40f, 140f);
        float random_angle_rads = random_angle * Mathf.Deg2Rad;
        float vx = Mathf.Cos(random_angle_rads);
        float vy = Mathf.Sin(random_angle_rads);

        Vector2 initial_velocity = new Vector2(vx, -vy).normalized * initial_speed;
        agent_ball_rb.velocity = initial_velocity;
        is_moving = true;
    }
}
