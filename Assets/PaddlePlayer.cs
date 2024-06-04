using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddlePlayer : MonoBehaviour
{
    // serializing move_speed in case we want power-ups or something later
    [SerializeField] float move_speed = 7.0f;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float min_x = -7.64f;
        float max_x = 7.64f;
        float speed_amount = Input.GetAxis("Horizontal") * move_speed * Time.deltaTime;

        if (transform.position.x <= min_x && speed_amount < 0)
        {
            speed_amount = 0;
        }

        if (transform.position.x >= max_x && speed_amount > 0)
        {
            speed_amount = 0;
        }

        transform.Translate(speed_amount, 0, 0);
    }
}
