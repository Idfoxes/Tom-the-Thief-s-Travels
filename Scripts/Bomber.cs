using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber : MonoBehaviour
{
    public GameObject bullet;
    public Transform shoot;
    public float timeShot = 4f;
    void Start()
    {
        shoot.transform.position = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
        StartCoroutine(Shooting());
    }
    IEnumerator Shooting()
    {
        yield return new WaitForSeconds(timeShot);
        Instantiate(bullet, shoot.transform.position, transform.rotation);
        StartCoroutine(Shooting());
    }
}