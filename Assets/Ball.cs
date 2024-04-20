using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D ball_rb;
    float initial_speed = 6.0f;
    float min_Y = -6.0f;
    float max_velocity = 12f; 
    bool is_moving = false;
    // int score = 0;
    int lives = 5;
    
    void Start()
    {

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
            transform.position = Vector3.zero;
            ball_rb.velocity = new Vector3(0, 0, 0);
            is_moving = false;
            lives--;
        }
        
        if(ball_rb.velocity.magnitude > max_velocity)
        {
            ball_rb.velocity = Vector3.ClampMagnitude(ball_rb.velocity, max_velocity);
        }

        Debug.Log(ball_rb.velocity.magnitude);
    }

    
}
