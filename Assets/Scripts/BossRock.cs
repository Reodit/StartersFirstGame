using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRock : Bullets
{
    private Rigidbody _rigidbody;
    private float angularPower = 2;
    private float scaleValue = 0.1f;
    private bool isShoot;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        StartCoroutine(GainPowerTime());
        StartCoroutine(GainPower());
    }

    IEnumerator GainPowerTime()
    {
        yield return new WaitForSeconds(2.2f);
        isShoot = true;
    }

    IEnumerator GainPower()
    {
        while (!isShoot)
        {
            angularPower += 0.02f;
            scaleValue += 0.005f;
            transform.localScale = Vector3.one * scaleValue;
            _rigidbody.AddTorque(transform.right * angularPower, ForceMode.Acceleration);
            yield return null;
        }
      
    }

    void Update()
    {
        
    }
}
