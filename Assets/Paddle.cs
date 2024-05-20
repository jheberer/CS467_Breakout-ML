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
    public override void Initialize()
    {
        temp = GameObject.Find("Ball");
        lives = temp.GetComponent<Ball>().lives;
    }
    // *===========================CHANGES-JP==========================================*
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
            // if(temp.GetComponent<Score>())
            // Debug.Log(temp.GetComponent<Ball>().is_moving);
            // Component balltemp = temp.GetComponent<Ball>();
            // Debug.Log(balltemp.Is_moving);
        }
        // bool hasGameBegan = true;
        // if (!isInPlay && lives >= 5)
        // {
        //     hasGameBegan = false;
        // }
        // else
        // {
        //     hasGameBegan = true;
        // }
        // Debug.Log("inside the onactionreeived");
        // float moveX = actions.ContinuousActions[0];// * Time.deltaTime * move_speed; //testing
        float min_x = -7.64f;
        float max_x = 7.64f;
        float speed_amount = actions.ContinuousActions[0] * move_speed * Time.deltaTime;

        // Debug.Log(transform.position);

        if (transform.position.x <= min_x && speed_amount < 0)
        {
            speed_amount = 0;
        }

        if (transform.position.x >= max_x && speed_amount > 0)
        {
            speed_amount = 0;
        }


        // Debug.Log(temp.GetComponent<is_moving>);
        // Debug.Log(temp.GetComponent<bool>());
        transform.Translate(speed_amount, 0, 0);
        if (!isInPlay)
        {
            if (lives != temp.GetComponent<Ball>().lives)
            {
                if (numOfHits == 0)
                {
                    Debug.Log("+++++++++++++++++++++++++LOST NO HITS+++++++++++++++++++++++++\t\t\t\t\t-1");

                    AddReward(-1f);
                    numOfHits = 0;
                    lives = lives - 1;
                    EndEpisode();
                }
                else
                {
                    Debug.Log("+++++++++++++++++++++++++LOST, ATLEAST 1 HIT+++++++++++++++++++++++++\t\t\t\t\t-1");

                    // Debug.Log("+++++++++++++++++++++++++loss reward+++++++++++++++++++++++++");
                    // Debug.Log(ballTransform.position);
                    AddReward(-1f);
                    numOfHits = 0;
                    // Debug.Log("+++++++++++++++++++++++++end epsisode after loss+++++++++++++++++++++++++");
                    lives = lives - 1;
                    EndEpisode();
                }
            }
        }


        // transform.position += new Vector3(moveX, 0, 0) * Time.deltaTime * move_speed;
        // Debug.Log("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++paddle position after change : " + transform.position.x);
        speed_amount = 0;
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(ballTransform.position);
        // Debug.Log("inside collectObservations");
        // Debug.Log(transform.position);

    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {

        ActionSegment<float> continousActions = actionsOut.ContinuousActions;
        continousActions[0] = Input.GetAxisRaw("Horizontal");
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Debug.Log("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++paddle position on collision : " + transform.position.x);

        // Debug.Log("+++++++++++++++++++++++++inside the oncollisionfunc+++++++++++++++++++++++++");

        // Debug.Log(other.gameObject.name);
        if (other.gameObject.CompareTag("Ball"))
        {

            numOfHits = numOfHits + 1;
            if (numOfHits > 1)
            {
                Debug.Log("+++++++++++++++++++++++++reward HIT=1+++++++++++++++++++++++++\t\t\t\t\t5");
                // AddReward(+.5f);
                // if (numOfHits > 2)
                // {
                AddReward(+1f);
                // }
                // Debug.Log("+++++++++++++++++++++++++end epsisode+++++++++++++++++++++++++");
                // EndEpisode();
            }
            if (numOfHits > 2)
            {
                // Debug.Log("+++++++++++++++++++++++++reward HIT=2+++++++++++++++++++++++++\t\t\t\t\t4");
                // AddReward(+.5f);
                // if (numOfHits > 2)
                // {
                AddReward(+1f);
                // }
                // Debug.Log("+++++++++++++++++++++++++end epsisode+++++++++++++++++++++++++");
                // EndEpisode();
            }
            if (numOfHits > 3)
            {
                // Debug.Log("+++++++++++++++++++++++++reward HIT=3+++++++++++++++++++++++++\t\t\t\t\t2");
                // AddReward(+.5f);
                // if (numOfHits > 2)
                // {
                AddReward(+1f);
                // }
                // Debug.Log("+++++++++++++++++++++++++end epsisode+++++++++++++++++++++++++");
                // EndEpisode();
            }
            if (numOfHits > 4)
            {
                // Debug.Log("+++++++++++++++++++++++++reward HIT=4+++++++++++++++++++++++++\t\t\t\t\t2");
                // AddReward(+.5f);
                // if (numOfHits > 2)
                // {
                AddReward(+1f);
                // }
                // Debug.Log("+++++++++++++++++++++++++end epsisode+++++++++++++++++++++++++");
                // EndEpisode();
            }
            if (numOfHits > 5)
            {
                Debug.Log("+++++++++++++++++++++++++reward HIT=5+++++++++++++++++++++++++\t\t\t\t\t2");
                // AddReward(+.5f);
                // if (numOfHits > 2)
                // {
                AddReward(+1f);
                // }
                // Debug.Log("+++++++++++++++++++++++++end epsisode+++++++++++++++++++++++++");
                // EndEpisode();
            }
        }
        // else if (other.gameObject.CompareTag("Paddle"))
        // {
        //     Debug.Log("+++++++++++++++++++++++++hit the ball");
        // }

    }
    // *===========================CHANGES-JP==========================================*








    // void Start()
    // {
    // }

    // Update is called once per frame
    // void Update()
    // {
    //     float min_x = -7.64f;
    //     float max_x = 7.64f;
    //     float speed_amount = Input.GetAxis("Horizontal") * move_speed * Time.deltaTime;

    //     if (transform.position.x <= min_x && speed_amount < 0)
    //     {
    //         speed_amount = 0;
    //     }

    //     if (transform.position.x >= max_x && speed_amount > 0)
    //     {
    //         speed_amount = 0;
    //     }

    //     transform.Translate(speed_amount, 0, 0);
    // }

}
