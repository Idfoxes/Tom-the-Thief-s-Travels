using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Water : MonoBehaviour
{
    float timer = 0f;
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 2f)                                                                                                                            
        {
            timer = 0;
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (timer >= 1f)
            transform.localScale = new Vector3(1f, 1f, 1f);
    }
}
