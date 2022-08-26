using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpPower = 15.0f;

    //public GameObject[] weapons ;
    //public bool[] hasWeapons;

    private float h;
    private float v;

    private bool walkDown;
    private bool jumpDown;
    //private bool interDown;
    public bool isJump;
    public bool isDodge;

    Vector3 moveVec;
    Vector3 dodgeVec;

    Animator anim;
    Rigidbody rigid;

    //GameObject nearObject;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        GetInput();
        Move();
        Rotate();
        Jump();
        Dodge();

    }
    void GetInput()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        //interDown = Input.GetButtonDown("Interaction");
        walkDown = Input.GetButton("Walk");
        jumpDown = Input.GetButtonDown("Jump");
    }
    void Move()
    {
        moveVec = new Vector3(h, 0, v).normalized;

        // 만약 회피중이면? moveVec에 dodgeVec 대입
        if (isDodge)
            moveVec = dodgeVec;

        if (walkDown)
            transform.position += moveVec * speed * 0.3f * Time.deltaTime;
        else
            transform.position += moveVec * speed * Time.deltaTime;

        anim.SetBool("isRun", moveVec != Vector3.zero);
        anim.SetBool("isWalk", walkDown);
    }
    void Rotate()
    {
        // LookAt() : 지정된 벡터를 향해서 회전시켜주는 함수
        transform.LookAt(transform.position + moveVec);
    }
    void Jump()
    {
        if (jumpDown && !isJump && moveVec == Vector3.zero && !isDodge)
        {
            // ForceMode.Impulse : 즉발적인 힘을 가함
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;
        }
    }
    void Dodge()
    {
        if (jumpDown && !isJump && moveVec != Vector3.zero && !isDodge)
        {
            dodgeVec = moveVec;
            speed *= 2f;
            anim.SetTrigger("doDodge");
            isDodge = true;

            Invoke("DodgeOut", 0.5f);

        }
    }
    void DodgeOut()
    {
        speed *= 0.5f;
        isDodge = false;
    }
    //void Interaction()
    //{
    //    if (interDown && nearObject != null && !isJump && !isDodge)
    //    {
    //        if (nearObject.CompareTag("Weapon"))
    //        {
    //            Item item = nearObject.GetComponent<Item>();
    //            int weaponIndex = item.value;
    //            hasWeapons[weaponIndex] = true;

    //            Destroy(nearObject);
    //        }

    //    }
    //}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            anim.SetBool("isJump", false);
            isJump = false;
        }
    }
    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.tag =="Weapon")
    //        nearObject = other.gameObject;

    //    Debug.Log(nearObject.name);
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.tag == "Weapon")
    //        nearObject = null;
    //}
}
