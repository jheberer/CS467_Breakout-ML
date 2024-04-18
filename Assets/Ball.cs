using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D ball_rb;
    float initial_speed = -3.0f;
    float min_Y = -6.0f;
    float max_velocity = 12f; 
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // this references the rigid body component of this ball
        ball_rb = GetComponent<Rigidbody2D>();
    
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // transform.translate will only apply discrete movements when
            // space is held. Need a vector instead
            ball_rb.velocity = new Vector3(0, initial_speed, 0);
            
        }

        if(transform.position.y < min_Y)
        {
            transform.position = Vector3.zero;
            ball_rb.velocity = new Vector3(0, 0, 0);
        }
        
        if(ball_rb.velocity.magnitude > max_velocity)
        {
            ball_rb.velocity = Vector3.ClampMagnitude(ball_rb.velocity, max_velocity);
        }
    }

    
}
