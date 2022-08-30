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
        if (!isRock && collision.gameObject.tag == "Floor")
        {
            Destroy(gameObject, 3);
        }
        if (!isMelee && collision.gameObject.tag == "Wall")
        {
            Debug.Log(1);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isMelee && other.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
        if(!isMelee && other.gameObject.tag == "Player")
        {
            Debug.Log(2);
            Destroy(gameObject);
        }
    }
}
