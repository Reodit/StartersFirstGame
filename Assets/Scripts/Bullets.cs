using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    public int damage;

    public bool isMelee;
    public bool isRock;

    private void OnCollisionEnter(Collision collision)
    {
        if (!isRock&&collision.gameObject.tag == "Floor")
        {
            Debug.Log(11);
            Destroy(gameObject, 3);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isMelee && other.gameObject.tag == "Wall")
        {
            Debug.Log(1);
            Destroy(gameObject);
        }
    }
}
