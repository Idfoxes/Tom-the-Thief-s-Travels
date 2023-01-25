using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bg : MonoBehaviour
{
    float length, startPosition;
    public GameObject cam;
    public float paralaxEffect;
    void Start()
    {
        startPosition = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void LateUpdate()
    {
            float temp = cam.transform.position.x * (1 - paralaxEffect);
            float dist = cam.transform.position.x * paralaxEffect;

            transform.position = new Vector3(startPosition + dist, transform.position.y, transform.position.z);

            if (temp > startPosition + length)
                startPosition += length;
            else if (temp < startPosition - length)
                startPosition -= length;
    }
}
