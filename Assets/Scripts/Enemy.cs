using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum Type { A, B, C };

    public Type _enemyType;
    
    public int maxHealth;
    public Transform _target;
    public int curHealth;
    private NavMeshAgent _nav;
    private Animator _anim;
    private int _gold;
    public BoxCollider meleeArea;
    public bool isChase;
    public bool isAttack;
    

     Rigidbody _rigid;
     private BoxCollider _boxCollider;
     public Material _material;

     private void Awake()
     {
         _anim = GetComponentInChildren<Animator>();
         _rigid = GetComponent<Rigidbody>();
         _boxCollider = GetComponent<BoxCollider>();
         _material = GetComponentInChildren<MeshRenderer>().material;
         _nav = GetComponent<NavMeshAgent>();
         Invoke("ChaseStart",2f);
     }

     void ChaseStart()
     {
         isChase = true;
         _anim.SetBool("isWalk",true);
     }
     private void Update()
     {
         if (_nav.enabled)
         {
             _nav.SetDestination(_target.position);
             _nav.isStopped = !isChase;
         }
        
     }
     

     void FreezeVelocity()
     {
         if (isChase)
         {
             _rigid.velocity = Vector3.zero;
             _rigid.angularVelocity = Vector3.zero;
         }
       
     }

     private void FixedUpdate()
     {
         FreezeVelocity();
     }

     void Targeting()
     {
         float targetRadius = 1.5f;
         float targetRange = 3f;
         RaycastHit[] rayHits = Physics.SphereCastAll(transform.position,
             targetRadius,
             transform.forward,
             targetRange,
             LayerMask.GetMask("Player"));
         if (rayHits.Length > 0 && !isAttack)
         {
             
         }

     }

     IEnumerator Attack()
     {
         isChase = false;
         isAttack = true;
         _anim.SetBool("isAttack", true);

         yield return new WaitForSeconds(0.2f);
         meleeArea.enabled = true;
         yield return new WaitForSeconds(1f);
         meleeArea.enabled = false;
         yield return new WaitForSeconds(1f);
         isChase = true;
         isAttack = false;
         _anim.SetBool("isAttack", false);
         //_anim.SetBool("isWalk",true);
     }
         
         
     

     public void OnTriggerEnter(Collider other)
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

     IEnumerator OnDamage(Vector3 rectVec, bool isGrendade)
     {
         _material.color = Color.red;
         yield return new WaitForSeconds(0.1f);

         if (curHealth > 0)
         {
             _material.color = Color.white;
         }
         else
         {
             _material.color = Color.gray;
             gameObject.layer = 14;
             isChase = false;
             _nav.enabled = false;
             _anim.SetTrigger("doDie");
             
             if (isGrendade)
             {
                 rectVec = rectVec.normalized;
                 rectVec += Vector3.up * 3;
                 _rigid.freezeRotation = false;
                 _rigid.AddForce(rectVec * 5, ForceMode.Impulse);
                 _rigid.AddTorque(rectVec * 15, ForceMode.Impulse);

             }
             else
             {
                 rectVec = rectVec.normalized;
                 rectVec += Vector3.up;
                 _rigid.AddForce(rectVec * 5 , ForceMode.Impulse);
             }
             Destroy(gameObject, 4);
         }
     }

   
}
