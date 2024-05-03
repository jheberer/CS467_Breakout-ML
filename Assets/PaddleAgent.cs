using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

//
public class PaddleAgent : Agent
{
    int ballNumber;
    float ballReward = 1f;
    float ballMiss = -1f;
    float brickReward = 1f;
    float startPaddleY = -4f;
    float startPaddleX = 0f;
    float leftBoundary = -7.64f;
    float rightBoundary = 7.64f;
    float speed = 7f;
    GameObject ball;

    //overriding predefined functions necessary to train our paddle agent
    //https://docs.unity3d.com/Packages/com.unity.ml-agents@1.0/api/Unity.MLAgents.Agent.html
    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector2(startPaddleX, startPaddleY);
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition.x);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float move = actions.ContinuousActions[0];
    }
}
