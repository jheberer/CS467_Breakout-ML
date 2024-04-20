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
    float max_velocity = 12f; 
    bool is_moving = false;
    public int score = 0;
    public int lives = 5;
    public TextMeshProUGUI score_text;
    public GameObject[] lives_image;
    public GameObject game_over_splash;

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
    
    void GameOver()
    {
        Debug.Log("game over");
        game_over_splash.SetActive(true);
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // this references the rigid body component of this ball
        ball_rb = GetComponent<Rigidbody2D>();

       
        if (!is_moving) 
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // transform.translate will only apply discrete movements when
                // space is held. Need a vector instead
                // give it a little lateral movement so game start is interesting

                float random_angle = Random.Range(20f, 160f);
                float random_angle_rads = random_angle * Mathf.Deg2Rad;
                float vx = Mathf.Cos(random_angle_rads);
                float vy = Mathf.Sin(random_angle_rads);

                Vector3 initial_velocity = new Vector3(vx, -vy, 0f).normalized * initial_speed;

                ball_rb.velocity = initial_velocity;
                is_moving = true;
                
            }
        }

        // if the ball falls below the screen
        if(transform.position.y < min_Y)
        {
            if (lives <= 0)
            {
                GameOver();
            }
            else 
            {
                transform.position = Vector3.zero;
                ball_rb.velocity = new Vector3(0, 0, 0);
                is_moving = false;
                lives--;
                lives_image[lives].SetActive(false);
            }
        }
        
        if(ball_rb.velocity.magnitude > max_velocity)
        {
            ball_rb.velocity = Vector3.ClampMagnitude(ball_rb.velocity, max_velocity);
        }

        // Debug.Log(ball_rb.velocity.magnitude);
        score_text.text = score.ToString("000000");
        Debug.Log(score);
    }

    
}
