using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AgentBall : MonoBehaviour
{
    public static AgentBall agentInstance { get; private set; }
    
    private Rigidbody2D agent_ball_rb;
    private float initial_speed = 6.0f;
    private float min_Y = -6.0f;
    private float max_velocity = 11f; 
    public float min_vertical_speed = 1f;
    private bool is_moving = false;

    public int agent_lives = 5;
    public int agent_score = 0;

    public TextMeshProUGUI score_text_agent;

    public GameObject[] agent_lives_image;
    public bool game_over = false;
    private float previous_velocity_y = 0f;
    public int agent_brick_count;

    void Awake()
    {
        if (gameObject.CompareTag("AgentBall"))
        {
            if (agentInstance != null && agentInstance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                agentInstance = this;
                Debug.Log("Agent Ball instance is initialized.");
            }
        }
    }

    void Start()
    {
        GameObject[] bricks = GameObject.FindGameObjectsWithTag("AgentBrick");
        agent_brick_count = bricks.Length;
    }

    void Update()
    {
        agent_ball_rb = GetComponent<Rigidbody2D>();
        if (game_over)
        {
            return;
        }

        if (agent_brick_count == 0)
        {
            return;
        }

        if (!is_moving) 
        {
            StartBallMovement();
        }
        else
        {
            if (transform.position.y < min_Y)
            {
                Debug.Log("Ball has fallen below the screen.");
                if (agent_lives <= 0)
                {
                    return;
                }
                else
                {
                    agent_lives--;
                    ResetBall();
                    agent_lives_image[agent_lives].SetActive(false);
                    StartBallMovement();
                }
            }

            if (agent_ball_rb.velocity.magnitude > max_velocity)
            {
                agent_ball_rb.velocity = Vector3.ClampMagnitude(agent_ball_rb.velocity, max_velocity);
            }

            if (Mathf.Abs(agent_ball_rb.velocity.y) < min_vertical_speed && is_moving)
            {
                float vertical_force = min_vertical_speed - Mathf.Abs(agent_ball_rb.velocity.y);
                Vector2 vertical_force_direction = Vector2.up * Mathf.Sign(previous_velocity_y);
                agent_ball_rb.AddForce(vertical_force_direction * vertical_force, ForceMode2D.Impulse);
            }
        } 

        score_text_agent.text = agent_score.ToString("000000");
        previous_velocity_y = agent_ball_rb.velocity.y;
    }

    public void ResetIsMoving() {
        is_moving = false;
    }

    public void ResetBall()
    {
        agent_ball_rb = GetComponent<Rigidbody2D>();
        if (gameObject.CompareTag("AgentBall"))
        {
            transform.position = new Vector2(-20, -0.5f);
        }
        agent_ball_rb.velocity = Vector3.zero;
        is_moving = false;
        Debug.Log("Ball has been reset.");
    }

    public void StartBallMovement()
    {
        agent_ball_rb = GetComponent<Rigidbody2D>();
        float random_angle = Random.Range(40f, 140f);
        float random_angle_rads = random_angle * Mathf.Deg2Rad;
        float vx = Mathf.Cos(random_angle_rads);
        float vy = Mathf.Sin(random_angle_rads);

        Vector2 initial_velocity = new Vector2(vx, -vy).normalized * initial_speed;
        agent_ball_rb.velocity = initial_velocity;
        is_moving = true;
        Debug.Log("Ball movement started.");
    } 
}