using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Range(0,20)]public float y = 1f; 
    Vector3 offset;
    public enum Type { A, B, C,D };

    public Type _enemyType;
    
    public int maxHealth;
    public int curHealth;
    public Transform _target;
    public BoxCollider meleeArea;
    public GameObject _bullet;
    public bool isChase;
    public bool isAttack;
    public bool isDead;
   
     public Rigidbody _rigid;
     public BoxCollider _boxCollider;
     public MeshRenderer[] _mesh;
    public NavMeshAgent _nav;
     public Animator _anim;
    public int _gold;
    


     private void Awake()
     {
         _anim = GetComponentInChildren<Animator>();
         _rigid = GetComponent<Rigidbody>();
         _boxCollider = GetComponent<BoxCollider>();
         _mesh = GetComponentsInChildren<MeshRenderer>();
         _nav = GetComponent<NavMeshAgent>();
          
         if(_enemyType != Type.D)
            Invoke("ChaseStart",2f);
         offset = new Vector3(0,y,0);
     }

     void ChaseStart()
     {
         isChase = true;
         _anim.SetBool("isWalk",true);
     }
     private void Update()
     {
         if (_nav.enabled && _enemyType != Type.D)
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
         Targeting();
         FreezeVelocity();
     }

     void Targeting()
     {
         if (!isDead && _enemyType != Type.D)
         {
             float targetRadius = 0f;
             float targetRange = 0f;
             switch (_enemyType)
             {
                 case  Type.A:
                     Debug.Log("A");
                     targetRadius = 1.5f;
                     targetRange = 3f;
                     break;
                 case Type.B:
                     Debug.Log("B");
                     targetRadius = 1f;
                     targetRange = 12f;
                     break;
                 case Type.C:
                     Debug.Log("C");
                     targetRadius = 0.5f;
                     targetRange = 25f;
                     break;
             }

        

             RaycastHit[] rayHits = Physics.SphereCastAll(transform.position,
                 targetRadius,
                 transform.forward,
                 targetRange,
                 p);
             //LayerMask.GetMask("Player"));
             foreach (var hit in  rayHits)
             {
                 currentHitDistance = hit.distance;
             }
             if (rayHits.Length > 0 && !isAttack)
             {
                 Debug.Log("hey");
                 StartCoroutine(Attack());
             }
         }
      

     }

     private float currentHitDistance = 25;
     public LayerMask p;
     IEnumerator Attack()
     {
         Debug.Log("Attack Method");
         isChase = false;
         isAttack = true;
         _anim.SetBool("isAttack", true);
         switch (_enemyType)
         {
             case  Type.A:
                 yield return new WaitForSeconds(0.2f);
                 meleeArea.enabled = true;
                 yield return new WaitForSeconds(1f);
                 meleeArea.enabled = false;
                 yield return new WaitForSeconds(1f);
                
                 break;
             case Type.B:
                 yield return new WaitForSeconds(0.1f);
                 _rigid.AddForce(transform.forward * 20, ForceMode.Impulse);
                 meleeArea.enabled = true;
                 
                 yield return new WaitForSeconds(0.5f);
                 _rigid.velocity = Vector3.zero;
                 meleeArea.enabled = false;

                 yield return new WaitForSeconds(3f);
                 
                 break;
             case Type.C:
                 Debug.Log(2);
                 yield return new WaitForSeconds(0.5f);
                 GameObject instantBullet = Instantiate(_bullet, transform.position, transform.rotation  );
                 Rigidbody rigidBullet = instantBullet.GetComponent<Rigidbody>();
                 rigidBullet.velocity = transform.forward * 20;
                 yield return new WaitForSeconds(2f);
                 
                 break;
         }
       
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
         foreach (var mesh in _mesh)
         {
             mesh.material.color = Color.red;
         }
       
         yield return new WaitForSeconds(0.1f);

         if (curHealth > 0)
         {
             foreach (var mesh in _mesh)
             {
                 mesh.material.color = Color.red;
             }
         }
         else
         {
             foreach (var mesh in _mesh)
             {
                 mesh.material.color = Color.red;
             }
             gameObject.layer = 14;
             isDead = true;
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
             if(_enemyType != Type.D)
                Destroy(gameObject, 4);
         }
     }

     void OnDrawGizmos()
     {
         Gizmos.color = Color.red;
         Debug.DrawRay(transform.position + offset, transform.forward * currentHitDistance);
         Gizmos.DrawWireSphere(transform.position + offset + transform.forward * currentHitDistance, 0.5f);
     }
}
