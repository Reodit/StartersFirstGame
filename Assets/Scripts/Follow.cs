using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;


    // Update is called once per frame
    void Update()
    {
        // 플레이어 따라가는 카메라
        transform.position = target.position + offset;
    }
}
