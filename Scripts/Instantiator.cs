using System;
using UnityEngine;

public class Instantiator : MonoBehaviour
{
    public GameObject Menu;

    private void Awake()
    {
        if (!FindObjectOfType<Menu>())
            Menu = Instantiate(Menu);

        Destroy(this);
    }
}