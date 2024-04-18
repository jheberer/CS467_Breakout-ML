using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    // serializing move_speed in case we want power-ups or something later
    [SerializeField] float move_speed = 8.0f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float speed_amount = Input.GetAxis("Horizontal") * move_speed * Time.deltaTime;

        transform.Translate(speed_amount,0,0);
    }
}
