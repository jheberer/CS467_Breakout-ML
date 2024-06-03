using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class PaddleAgent : Agent
{
    float ballReward = 20f;
    float ballMiss = -10f;
    float brickReward = 10f;
    float startPaddleY = -4f;
    float startPaddleX = -20f;
    float leftBoundary = -28f;
    float rightBoundary = -12f;
    float speed = 25f;

    [SerializeField] private AgentBall ball;
    private int previousLives;

    public override void OnEpisodeBegin()
    {
        // Reassign the ball reference
        ball = AgentBall.agentInstance;

        if (ball == null)
        {
            Debug.LogError("Agent Ball reference is missing in PaddleAgent.");
            previousLives = ball.agent_lives;
        }
        else
        {
            ball.ResetBall();
            ball.StartBallMovement();
        }

        ResetPaddlePosition();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        ball = AgentBall.agentInstance;
        sensor.AddObservation(transform.localPosition);

        if (ball != null)
        {
            sensor.AddObservation(ball.transform.localPosition);
            sensor.AddObservation(ball.GetComponent<Rigidbody2D>().velocity);

            sensor.AddObservation(ball.transform.localPosition.x - transform.localPosition.x);
            sensor.AddObservation(ball.transform.localPosition.y - transform.localPosition.y);
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float move = actions.ContinuousActions[0];
        Debug.Log($"Received action: {move}");

        float newXPos = Mathf.Clamp(transform.localPosition.x + move * speed * Time.deltaTime, leftBoundary, rightBoundary);
        transform.localPosition = new Vector2(newXPos, startPaddleY);
        Debug.Log($"New paddle position: {transform.localPosition}");

        if (ball != null && ball.agent_lives > 0)
        {
            float distanceX = Mathf.Abs(transform.localPosition.x - ball.transform.localPosition.x);

            // Calculate a reward based on the combined distance
            // The closer the paddle is to the ball, the higher the reward
            float maxDistance = 3.5f;
            float reward;
            if (distanceX <= 1.25f)
            {
                reward = (1.25f - distanceX) * 100f;
            }
            else if (distanceX <= maxDistance)
            {
                // Positive reward for being close to the ball
                reward = (maxDistance - distanceX) * 10f;
            }
            else
            {
                // Negative penalty for being far from the ball
                reward = -((distanceX - maxDistance) * 10f);
            }

            // Log the reward for debugging purposes
            Debug.Log($"Reward: {reward}");

            // Add the reward to the agent
            AddReward(reward);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("AgentBall"))
        {
            AddReward(ballReward);
        }

        if (collision.gameObject.CompareTag("AgentBrick"))
        {
            AddReward(brickReward);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("AgentBall"))
        {
            AddReward(ballMiss);
        }

        if (AgentBall.agentInstance != null && AgentBall.agentInstance.agent_lives <= 0)
        {
            EndEpisode();
        }
    }

    private void Update()
    {
        if (ball != null && ball.agent_lives != previousLives)
        {
            if (ball.agent_lives < previousLives)
            {
                ResetPaddlePosition();
            }
            previousLives = ball.agent_lives;
        }
    }

     private void ResetPaddlePosition()
    {
        transform.localPosition = new Vector2(startPaddleX, startPaddleY);
        Debug.Log($"Paddle reset to position: {transform.localPosition}");
    }
}
