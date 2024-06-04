using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentBrick : MonoBehaviour
{
    private int row_number;

    // Start is called before the first frame update
    void Start()
    {
        GameObject parent_row = transform.parent.gameObject;
        row_number = parent_row.GetComponent<Row>().row_number;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // updating remote variable in Ball singleton!
        // AgentBall.agentInstance.agent_score += row_number;
        // AgentBall.agentInstance.agent_brick_count -= 1;
        AgentBall.instance.score += row_number;
        AgentBall.instance.brick_count -= 1;
        Destroy(gameObject);
    }
}