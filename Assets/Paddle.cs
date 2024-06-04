// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// // *===========================CHANGES-JP- OUTSIDE OF THE CODE==========================================*
// // on the unity editor installed MLagents package


// // changed the Behavior parameters 
// // bahavior name = HitBall
// // space size from 1 to 6
// // continous actions from 0 to 2
// // discrete branch from 1 to 0

// // under Paddle(script)
// // added transform for ball 

// // added isGamestart in the ball.script.
// // checked for is game start to set when to get rewards
// // *===========================CHANGES-JP==========================================*
// //  added packages =========>>>>>>>namespace unity.mlagents, using Unity.MLAgents , using Unity.MLAgents.Actuators,
// // using Unity.MLAgents.Sensors, new namespace unity.mlagents
// using Unity.MLAgents;
// using Unity.MLAgents.Actuators;
// using Unity.MLAgents.Sensors;
// using Unity.VisualScripting;
// using UnityEngine.SocialPlatforms.Impl;
// using Unity.Mathematics;
// // *===========================CHANGES-JP==========================================*
// // *===========================CHANGES-JP==========================================*
// // From public class Paddle : MonoBehaviour ============>>>>>> public class Paddle : Agent
// // public class Paddle : MonoBehaviour
// public class Paddle : Agent
// // *===========================CHANGES-JP==========================================*
// {
//     // serializing move_speed in case we want power-ups or something later
//     [SerializeField] float move_speed = 7.0f;
//     // *===========================CHANGES-JP==========================================*
//     // added transform to locate ball
//     // added a global speed_amount variable
//     // set boundary for that if passed, the ball was considered lost
//     [SerializeField] private Transform ballTransform; // to get the ball poisition
//     [SerializeField] float speed_amount = 0;
//     float min_Y_for_Ball = -5.9f;           // where the ball is considered lost
//     GameObject temp;
//     bool isInPlay = false;
//     int lives = 0;
//     int numOfHits = 0;
//     float paddle_position_x = 0;
//     float ball_position_x = 0;
//     float paddle_position_y = 0;
//     float ball_position_y = 0;
//     float outermin = -2f;
//     float outermax = 2f;
//     bool hitHalf = false;
//     bool almostComplete = false;
//     // *===========================CHANGES-JP==========================================*
//     // gets the ball position and the number of lives
//     public override void Initialize()
//     {
//         temp = GameObject.Find("Ball");
//         lives = temp.GetComponent<Ball>().lives;
//     }
//     public override void OnActionReceived(ActionBuffers actions)
//     {
//         if (temp.GetComponent<Ball>().game_won)
//         {
//             AddReward(+100f);
//         }
//         if (temp == null)
//         {
//             Debug.Log("empty ball gameobject");
//         }
//         else
//         {
//             isInPlay = temp.GetComponent<Ball>().isGameStart;
//         }
//         float min_x = -7.64f;
//         float max_x = 7.64f;
//         float speed_amount = actions.ContinuousActions[0] * move_speed * Time.deltaTime;
//         if (transform.position.x <= min_x && speed_amount < 0)
//         {
//             speed_amount = 0;
//         }

//         if (transform.position.x >= max_x && speed_amount > 0)
//         {
//             speed_amount = 0;
//         }
//         transform.Translate(speed_amount, 0, 0);
//         paddle_position_x = transform.position.x;
//         paddle_position_y = transform.position.y;
//         ball_position_x = temp.GetComponent<Ball>().x_position;
//         ball_position_y = temp.GetComponent<Ball>().y_position;
//         if (isInPlay && temp.GetComponent<Ball>().is_moving)
//         {
//             if (ball_position_y < -3.5)
//             {
//                 if (math.abs(paddle_position_x - ball_position_x) <= .15f)
//                 {
//                     // Debug.Log("getting reward for chasing ball with <= .15 distance\t\treward 1");
//                     AddReward(+1f);
//                 }
//                 else if (math.abs(paddle_position_x - ball_position_x) <= .25f)
//                 {
//                     // Debug.Log("getting reward for chasing ball with <= .25 distance \t\treward .6");
//                     AddReward(+.6f);
//                 }
//                 else if (math.abs(paddle_position_x - ball_position_x) <= .35f)
//                 {
//                     // Debug.Log("getting reward for chasing ball with  <= .35 distance \t\treward .3");
//                     AddReward(+.3f);
//                 }
//                 // if (math.abs(paddle_position_x - ball_position_x) < 3)
//                 // {
//                 //     Debug.Log("getting reward for chasing ball but still far");
//                 //     AddReward(+.5f);
//                 // }
//             }
//         }
//         if (!isInPlay)
//         {
//             if (lives != temp.GetComponent<Ball>().lives)
//             {

//                 if (numOfHits == 0)
//                 {
//                     Debug.Log("+++++++++++++++++++++++++LOST NO HITS+++++++++++++++++++++++++\t\t\t\t\treward-10");
//                     AddReward(-15f);
//                     numOfHits = 0;
//                     lives = lives - 1;
//                     EndEpisode();
//                 }
//                 else
//                 {
//                     Debug.Log("+++++++++++++++++++++++++LOST, ATLEAST 1 HIT+++++++++++++++++++++++++\t\t\t\t\treward -5");
//                     AddReward(-10f);
//                     numOfHits = 0;
//                     lives = lives - 1;
//                     EndEpisode();
//                 }
//             }
//         }
//         if (temp.GetComponent<Ball>().bricksHit >= 35)
//         {
//             hitHalf = true;
//         }
//         if (temp.GetComponent<Ball>().bricksHit >= 40)
//         {
//             almostComplete = true;

//         }
//         speed_amount = 0;
//     }
//     public override void CollectObservations(VectorSensor sensor)
//     {
//         Debug.Log("Paddle Position: " + transform.position);
//         Debug.Log("Ball Position: " + ballTransform.position);
//         Debug.Log("Ball Velocity: " + ballTransform.GetComponent<Rigidbody2D>().velocity);
//         sensor.AddObservation(transform.position);
//         sensor.AddObservation(ballTransform.position);
//         sensor.AddObservation(ballTransform.GetComponent<Rigidbody2D>().velocity);
//     }
//     public override void Heuristic(in ActionBuffers actionsOut)
//     {
//         ActionSegment<float> continousActions = actionsOut.ContinuousActions;
//         continousActions[0] = Input.GetAxisRaw("Horizontal");
//     }
//     private void OnCollisionEnter2D(Collision2D other)
//     {
//         // if (math.abs(paddle_position_x - ball_position_x) <= .35)
//         // {
//         //     Debug.Log("getting reward for chasing ball with <= .35 in collision reward 1");
//         //     AddReward(+1f);
//         // }
//         // if (numOfHits == 0)
//         // {
//         //     Debug.Log("\t\t\t\t\t+++++++++++++++++++++++++FIRST Hitting Ball+++++++++++++++++++++++++ reward 1");

//         //     AddReward(+1f);
//         // }
//         if (other.gameObject.CompareTag("Ball"))
//         {
//             if (hitHalf)
//             {
//                 Debug.Log("getting reward for hitting half of bricks");
//                 AddReward(+10f);
//             }
//             if (almostComplete)
//             {
//                 Debug.Log("+++++++++++++++++++++++++REWARD ALMOST COMPLETE+++++++++++++++++++++++++\t\t\t\t\treward 45");
//                 AddReward(45f);
//             }
//             if (math.abs(paddle_position_x - ball_position_x) <= .15)
//             {
//                 Debug.Log("getting reward for chasing ball with <= .15 in collision reward 1");
//                 AddReward(+1f);
//             }
//             if (numOfHits == 0)
//             {
//                 Debug.Log("\t\t\t\t\t+++++++++++++++++++++++++FIRST Hitting Ball+++++++++++++++++++++++++");

//                 // AddReward(+1f);
//             }
//             // Debug.Log("+++++++++++++++++++++++++Hitting Ball +++++++++++++++++++++++++\t\t\t\t\t" + numOfHits);
//             float accumulativeReward = 0;
//             if (lives != 0)
//             {
//                 accumulativeReward = (numOfHits * .5f) * lives + 1;
//             }
//             else
//             {
//                 accumulativeReward = (numOfHits * .5f) + 1;
//             }
//             // Debug.Log("reward = " + accumulativeReward);
//             AddReward(accumulativeReward);
//             numOfHits = numOfHits + 1;
//             if (numOfHits > 2)
//             {
//                 Debug.Log("\t\t\t\t\t+++++++++++++++++++++++++MORE THAN 2 Hits+++++++++++++++++++++++++");
//                 AddReward(+5f);
//             }
//             if (numOfHits == 10)
//             {
//                 Debug.Log("\t\t\t\t\t+++++++++++++++++++++++++MORE THAN 10 Hits+++++++++++++++++++++++++");
//                 AddReward(+10f);
//             }
//         }
//     }
// }

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
    bool hitHalf = false;
    bool almostComplete = false;
    // *===========================CHANGES-JP==========================================*
    // gets the ball position and the number of lives
    public override void Initialize()
    {
        // temp = GameObject.Find("Ball");
        temp = GameObject.Find("Ball (1)");
        // lives = temp.GetComponent<Ball>().lives;
        lives = temp.GetComponent<AgentBall>().lives;

    }
    public bool isLearningPaused = false;

    public void PauseLearning()
    {
        isLearningPaused = true;
    }

    public void ResumeLearning()
    {
        isLearningPaused = false;
    }
    public void LostBall()
    // if (!temp.GetComponent<AgentBall>().)
    {
        // Debug.Log("the number of hits is " + numOfHits);
        // if (lives != temp.GetComponent<Ball>().lives)
        // if (lives != temp.GetComponent<AgentBall>().lives)
        // {
        if (numOfHits == 0)
        {
            Debug.Log("+++++++++++++++++++++++++LOST NO HITS+++++++++++++++++++++++++\t\t\t\t\treward-10");
            AddReward(-10f);
            numOfHits = 0;
            lives = lives - 1;
            EndEpisode();
        }
        else
        {
            Debug.Log("+++++++++++++++++++++++++LOST, ATLEAST 1 HIT+++++++++++++++++++++++++\t\t\t\t\treward -5");
            AddReward(-5f);
            numOfHits = 0;
            lives = lives - 1;
            EndEpisode();
        }
        // }
    }
    public override void OnActionReceived(ActionBuffers actions)
    {

        if (lives < 0)
        {
            isLearningPaused = true;
        }
        if (!isLearningPaused)
        {
            if (ball_position_y > -4 && ball_position_y < 0)
            {
                Debug.Log("FRAME");
            }
            // if (temp.GetComponent<Ball>().game_won)
            if (temp.GetComponent<AgentBall>().game_won)
            {
                AddReward(+100f);
            }
            if (temp == null)
            {
                Debug.Log("empty ball gameobject");
            }
            else
            {
                // isInPlay = temp.GetComponent<Ball>().isGameStart;
                isInPlay = !(temp.GetComponent<AgentBall>().isGamePaused);
                // Debug.Log("========================is in play is equal to " + isInPlay + "========================");
            }
            // float min_x = -7.64f;
            // float max_x = 7.64f;
            // due to change in the start positioin of the mlagent.
            float min_x = -27.64f;
            float max_x = -12.36f;
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
            // ball_position_x = temp.GetComponent<Ball>().x_position;
            // ball_position_y = temp.GetComponent<Ball>().y_position;
            // if (isInPlay && temp.GetComponent<Ball>().is_moving)
            ball_position_x = temp.GetComponent<AgentBall>().x_position;
            ball_position_y = temp.GetComponent<AgentBall>().y_position;

            // if (isInPlay && temp.GetComponent<AgentBall>().is_moving)
            if (temp.GetComponent<AgentBall>().is_moving)
            {
                if (ball_position_y < 4)
                {
                    // if (math.abs(paddle_position_x - ball_position_x) <= .05f)
                    // {
                    //     Debug.Log("getting reward for chasing ball with <= .05 distance\t\treward .5");
                    //     AddReward(+.5f);
                    // }
                    // else if (math.abs(paddle_position_x - ball_position_x) <= .15f)
                    // {
                    //     Debug.Log("getting reward for chasing ball with <= .15 distance \t\treward .25");
                    //     AddReward(+.25f);
                    // }
                    // else if (math.abs(paddle_position_x - ball_position_x) <= .25f)
                    if (math.abs(paddle_position_x - ball_position_x) <= .25f)
                    {
                        Debug.Log("getting reward for chasing ball with  <= .25 distance \t\treward .15");
                        AddReward(+3f);
                    }
                    // else if (math.abs(paddle_position_x - ball_position_x) <= .5f)
                    // {
                    //     Debug.Log("getting reward for chasing ball with  <= 1 distance \t\treward .1");
                    //     AddReward(+.1f);
                    // }
                    // if (math.abs(paddle_position_x - ball_position_x) < 3)
                    // {
                    //     Debug.Log("getting reward for chasing ball but still far");
                    //     AddReward(+.5f);
                    // }
                }
            }
            // Debug.Log("====== is_moving = " + temp.GetComponent<AgentBall>().is_moving);
            // if (!isInPlay)
            // // if (!temp.GetComponent<AgentBall>().)
            // {
            //     Debug.Log("the number of hits is " + numOfHits);
            //     // if (lives != temp.GetComponent<Ball>().lives)
            //     if (lives != temp.GetComponent<AgentBall>().lives)
            //     {
            //         if (numOfHits == 0)
            //         {
            //             Debug.Log("+++++++++++++++++++++++++LOST NO HITS+++++++++++++++++++++++++\t\t\t\t\treward-10");
            //             AddReward(-25f);
            //             numOfHits = 0;
            //             lives = lives - 1;
            //             EndEpisode();
            //         }
            //         else
            //         {
            //             Debug.Log("+++++++++++++++++++++++++LOST, ATLEAST 1 HIT+++++++++++++++++++++++++\t\t\t\t\treward -5");
            //             AddReward(-10f);
            //             numOfHits = 0;
            //             lives = lives - 1;
            //             EndEpisode();
            //         }
            //     }
            // }
            // if (temp.GetComponent<Ball>().bricksHit >= 35)
            if (temp.GetComponent<AgentBall>().bricksHit >= 24)
            {
                hitHalf = true;
            }
            // if (temp.GetComponent<Ball>().bricksHit > 41)
            if (temp.GetComponent<AgentBall>().bricksHit > 40)
            {
                almostComplete = true;
            }
        }
        else
        {
            // Debug.Log("not learning ============================");
        }
        // speed_amount = 0;
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
        if (math.abs(paddle_position_x - ball_position_x) <= .35)
        {
            Debug.Log("getting reward for chasing ball with <= .35 in collision reward 1");
            AddReward(+1f);
        }
        // if (numOfHits == 0)
        // {
        //     Debug.Log("\t\t\t\t\t+++++++++++++++++++++++++FIRST Hitting Ball+++++++++++++++++++++++++ reward 1");

        //     AddReward(+1f);
        // }
        if (other.gameObject.CompareTag("AgentBall"))
        {
            // Debug.Log("collision hithalf = " + hitHalf);
            // Debug.Log("collision almostComplete = " + almostComplete);
            // if (temp.GetComponent<AgentBall>().bricksHit >= 21)
            // {
            //     Debug.Log("getting reward for hitting 7 of bricks");
            //     AddReward(+10f);
            // }
            if (hitHalf)
            {
                Debug.Log("getting reward for hitting half of bricks");
                AddReward(+5f);
                hitHalf = false;
            }
            if (almostComplete)
            {
                Debug.Log("+++++++++++++++++++++++++REWARD ALMOST COMPLETE+++++++++++++++++++++++++\t\t\t\t\treward 45");
                AddReward(10f);
                almostComplete = false;
            }
            if (math.abs(paddle_position_x - ball_position_x) <= .15)
            {
                Debug.Log("getting reward for chasing ball with <= .15 in collision reward 5");
                AddReward(+5f);
            }
            if (numOfHits == 0)
            {
                Debug.Log("\t\t\t\t\t+++++++++++++++++++++++++FIRST Hitting Ball+++++++++++++++++++++++++50");
                AddReward(+50f);
            }
            // Debug.Log("+++++++++++++++++++++++++Hitting Ball +++++++++++++++++++++++++\t\t\t\t\t" + numOfHits);
            float accumulativeReward = 0;
            if (lives != 0)
            {
                accumulativeReward = (numOfHits * .5f) * lives + 1;
                // accumulativeReward = (numOfHits * 1f) * lives + 1;

            }
            else
            {
                // accumulativeReward = (numOfHits * 1f) + 1;
                accumulativeReward = (numOfHits * .5f) + 1;
            }
            // Debug.Log("reward = " + accumulativeReward);
            AddReward(accumulativeReward);
            numOfHits++;
            if (numOfHits > 2)
            {
                Debug.Log("\t\t\t\t\t+++++++++++++++++++++++++MORE THAN 2 Hits+++++++++++++++++++++++++");
                AddReward(+1f);
            }
            if (numOfHits == 10)
            {
                Debug.Log("\t\t\t\t\t+++++++++++++++++++++++++MORE THAN 10 Hits+++++++++++++++++++++++++");
                AddReward(+10f);
            }
        }
    }
}