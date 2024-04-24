using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Row : MonoBehaviour
{
    // Start is called before the first frame update
    public int row_number;
    void Start()
    {
        SpriteRenderer[] brick_renderers = GetComponentsInChildren<SpriteRenderer>();

        float red_value = Random.Range(0f, 1f);
        float green_value = Random.Range(0f, 1f);
        float blue_value = Random.Range(0f, 1f);

        foreach (SpriteRenderer brick_renderer in brick_renderers)
        {
            brick_renderer.color = new Color(red_value, green_value, blue_value, 1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
