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
    bool is_moving = false;
    public int score = 0;
    public int lives = 5;
    public TextMeshProUGUI score_text;
    public GameObject[] lives_image;
    public GameObject game_over_splash;

    // for audio
    public AudioSource paddle_sound;
    public AudioSource brick_sound;


    // for detecting whether the ball is "stuck"
    public float horizontal_threshold = 0.1f; // Threshold for considering horizontal movement
    public float force_magnitude = 1f; // Magnitude of the force to apply
    public float delay_duration = 1f; // Duration to wait before applying force
    private float horizontal_duration = 0f;
    private float previous_vertical_velocity = 0f;
    private float current_vertical_velocity = 0f;
    private Coroutine force_routine;

    void CheckHorizontalMovement()
    {
        // Check if the object is moving horizontally
        if (Mathf.Abs(ball_rb.velocity.y) < horizontal_threshold)
        {
            if (horizontal_duration == 0)
            {
                previous_vertical_velocity = current_vertical_velocity;
            }
            // Increment the horizontal duration
            horizontal_duration += Time.deltaTime;
            

            // If the object just started moving horizontally, start the coroutine
            if (horizontal_duration >= delay_duration && force_routine == null)
            {
                force_routine = StartCoroutine(Applyforce_routine());
            }
        }
        else
        {
            // Reset the horizontal duration if the object's vertical velocity changes
            horizontal_duration = 0f;
            if (force_routine != null)
            {
                StopCoroutine(force_routine);
                force_routine = null;
            }
        }
    }

    IEnumerator Applyforce_routine()
    {
        // Wait for the specified delay duration
        yield return new WaitForSeconds(delay_duration);

        // Apply force in the direction of the previous vertical velocity
        Vector2 forceDirection = previous_vertical_velocity > 0 ? Vector2.up : Vector2.down;
        ball_rb.AddForce(forceDirection * force_magnitude, ForceMode2D.Impulse);

        // Reset the coroutine reference
        force_routine = null;
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
        // paddle_sound = GetComponent<AudioSource>();
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

                float random_angle = Random.Range(40f, 140f);
                float random_angle_rads = random_angle * Mathf.Deg2Rad;
                float vx = Mathf.Cos(random_angle_rads);
                float vy = Mathf.Sin(random_angle_rads);

                Vector3 initial_velocity = new Vector3(vx, -vy, 0f).normalized * initial_speed;

                ball_rb.velocity = initial_velocity;
                is_moving = true;

                
            }
        }

        // this is a test key for testing various behaviors
        if (is_moving)
        {
            if (Input.GetKeyDown(KeyCode.T)) 
            {
                // Get the current velocity
                Vector3 current_velocity = ball_rb.velocity;

                // Set the vertical component of the velocity to 0
                current_velocity.y = 0f;

                // Apply the modified velocity to the Rigidbody
                ball_rb.velocity = current_velocity;
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


        // add some gravity when vertical velocity is below a threshold so ball
        // doesn't get stuck
        // if (is_moving && ball_rb.velocity.y == 0.0f)
        // {
        //     ball_rb.AddForce(Vector2.up * 6f, ForceMode2D.Impulse);
        // }
        if (is_moving)
        {
            CheckHorizontalMovement();
        }

        // Debug.Log(ball_rb.velocity.magnitude);
        score_text.text = score.ToString("000000");

        current_vertical_velocity = ball_rb.velocity.y;
        // Debug.Log(score);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Paddle"))
        {
            paddle_sound.Play();
        }
        if (other.gameObject.CompareTag("Brick"))
        {
            brick_sound.Play();
        }
    }

    
}
