using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpPower = 15.0f;

    private float h;
    private float v;

    private bool walkDown;
    private bool jumpDown;
    public bool isJump;
    public bool isDodge;

    Vector3 moveVec;
    Vector3 dodgeVec;

    Animator anim;
    Rigidbody rigid;

    PlayerInteraction playerInteraction;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
        playerInteraction = GetComponent<PlayerInteraction>();  
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
        walkDown = Input.GetButton("Walk");
        jumpDown = Input.GetButtonDown("Jump");
    }
    void Move()
    {
        moveVec = new Vector3(h, 0, v).normalized;

        if (playerInteraction.isSwap)
            moveVec = Vector3.zero;

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
        if (jumpDown && !isJump && moveVec == Vector3.zero && !isDodge && !playerInteraction.isSwap)
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
        if (jumpDown && !isJump && moveVec != Vector3.zero && !isDodge && !playerInteraction.isSwap)
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            anim.SetBool("isJump", false);
            isJump = false;
        }
    }
}
