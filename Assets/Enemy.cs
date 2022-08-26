using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int maxHealth;
    public Transform _target;
    public int curHealth;
    private NavMeshAgent _nav;

     Rigidbody _rigid;
     private BoxCollider _boxCollider;
     private Material _material;

     private void Awake()
     {
         _rigid = GetComponent<Rigidbody>();
         _boxCollider = GetComponent<BoxCollider>();
         _material = GetComponentInChildren<MeshRenderer>().material;
         _nav = GetComponent<NavMeshAgent>();
     }

     private void Update()
     {
         _nav.SetDestination(_target.position);
     }

     void FreezeVelocity()
     {
         _rigid.velocity = Vector3.zero;
         _rigid.angularVelocity = Vector3.zero;
     }

     private void FixedUpdate()
     {
         FreezeVelocity();
     }

     private void OnTriggerEnter(Collider other)
     {
         if (other.tag == "Melee")
         {
             //Weapon weapon = other.GetComponent<Weapon>();
             //curHealth -= weapon.damage;
             // Vector3 reacVec = transform.position - other.transform.position;
             //
             // StartCoroutine(OnDamage(reacVec, false));
         }
         else if (other.tag == "Bullet")
         {
             // Bullet bullet = other.GetComponent<Bullet>();
             // curHealth -= bullet.damage;
         }
     }

   
}
