using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// *===========================CHANGES-JP- OUTSIDE OF THE CODE==========================================*
// on the unity editor installed MLagents package


// changed the Behavior parameters 
// bahavior name = HitBall
// space size from 1 to 6
// continous actions from 0 to 2
// discrete branch from 1 to 0

// under Paddle(script)
// added transform for ball 

// added isGamestart in the ball.script.
// checked for is game start to set when to get rewards
// *===========================CHANGES-JP==========================================*
//  added packages =========>>>>>>>namespace unity.mlagents, using Unity.MLAgents , using Unity.MLAgents.Actuators,
// using Unity.MLAgents.Sensors, new namespace unity.mlagents
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.VisualScripting;
using UnityEngine.SocialPlatforms.Impl;
using Unity.Mathematics;
// *===========================CHANGES-JP==========================================*
// *===========================CHANGES-JP==========================================*
// From public class Paddle : MonoBehaviour ============>>>>>> public class Paddle : Agent
// public class Paddle : MonoBehaviour
public class Paddle : Agent
// *===========================CHANGES-JP==========================================*
{
    // serializing move_speed in case we want power-ups or something later
    [SerializeField] float move_speed = 7.0f;
    // *===========================CHANGES-JP==========================================*
    // added transform to locate ball
    // added a global speed_amount variable
    // set boundary for that if passed, the ball was considered lost
    [SerializeField] private Transform ballTransform; // to get the ball poisition
    [SerializeField] float speed_amount = 0;
    float min_Y_for_Ball = -5.9f;           // where the ball is considered lost
    GameObject temp;
    bool isInPlay = false;
    int lives = 0;
    int numOfHits = 0;
    float paddle_position_x = 0;
    float ball_position_x = 0;
    float paddle_position_y = 0;
    float ball_position_y = 0;
    float outermin = -2f;
    float outermax = 2f;
    // *===========================CHANGES-JP==========================================*
    // gets the ball position and the number of lives and ball velocity for the MLagent
    // under onActionReceived
    // reward 15 if game is won, 
    // reward .5 if getting near ball and 1 if really close
    // reward -1 if lost ball and -2 if lost ball with no a single hit
    // under collectObservations
    // now collecting the balls velocity so update on inspector for paddle the vector observation under 
    // behavior parameter from 6 to 8
    // under collisions, set formula to reward agent based on the amount of hits made
    public override void Initialize()
    {
        temp = GameObject.Find("Ball");
        lives = temp.GetComponent<Ball>().lives;
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        if (temp.GetComponent<Ball>().game_won)
        {
            AddReward(+15f);
        }
        if (temp == null)
        {
            Debug.Log("empty ball gameobject");
        }
        else
        {
            isInPlay = temp.GetComponent<Ball>().isGameStart;
        }
        float min_x = -7.64f;
        float max_x = 7.64f;
        float speed_amount = actions.ContinuousActions[0] * move_speed * Time.deltaTime;
        if (transform.position.x <= min_x && speed_amount < 0)
        {
            speed_amount = 0;
        }

        if (transform.position.x >= max_x && speed_amount > 0)
        {
            speed_amount = 0;
        }
        transform.Translate(speed_amount, 0, 0);
        paddle_position_x = transform.position.x;
        paddle_position_y = transform.position.y;
        ball_position_x = temp.GetComponent<Ball>().x_position;
        ball_position_y = temp.GetComponent<Ball>().y_position;
        if (isInPlay && temp.GetComponent<Ball>().is_moving)
        {
            if (ball_position_y < -3.75)
            {
                if (math.abs(paddle_position_x - ball_position_x) < 1)
                {
                    Debug.Log("getting reward for chasing ball with more precise");
                    AddReward(+1f);
                }
                if (math.abs(paddle_position_x - ball_position_x) < 2)
                {
                    Debug.Log("getting reward for chasing ball");
                    AddReward(+0.5f);
                }
            }
        }
        if (!isInPlay)
        {
            if (lives != temp.GetComponent<Ball>().lives)
            {
                if (numOfHits == 0)
                {
                    Debug.Log("+++++++++++++++++++++++++LOST NO HITS+++++++++++++++++++++++++\t\t\t\t\t-1");
                    AddReward(-2f);
                    numOfHits = 0;
                    lives = lives - 1;
                    EndEpisode();
                }
                else
                {
                    Debug.Log("+++++++++++++++++++++++++LOST, ATLEAST 1 HIT+++++++++++++++++++++++++\t\t\t\t\t-1");
                    AddReward(-1f);
                    numOfHits = 0;
                    lives = lives - 1;
                    EndEpisode();
                }
            }
        }
        speed_amount = 0;
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(ballTransform.position);
        sensor.AddObservation(ballTransform.GetComponent<Rigidbody2D>().velocity);
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continousActions = actionsOut.ContinuousActions;
        continousActions[0] = Input.GetAxisRaw("Horizontal");
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            float accumulativeReward = (numOfHits * .05f) + 1;
            AddReward(accumulativeReward);
            numOfHits = +1;
            // if ((paddle_position_x < outermin) || paddle_position_x > outermax)
            // {
            //     Debug.Log("+++++++++++++++++++++++++Going beyond boundaries+++++++++++++++++++++++++\t\t\t\t\t5");
            //     AddReward(+1f);
            // }
        }
    }
}
