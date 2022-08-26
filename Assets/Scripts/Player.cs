using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5.0f;

    float h;
    float v;
    
    Vector3 moveVec;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        moveVec = new Vector3(h, 0, v).normalized;

        transform.position += moveVec * speed;

    }

}
