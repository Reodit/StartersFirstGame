using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 스크립트에서 region, endregion를 사용하여 확대 및 축소 할수있는 코드 블록을 지정할수있다.
public class PlayerMovement : MonoBehaviour
{
    #region Player Variables
    public Camera followCamera;

    [SerializeField] private float speed = 5.0f;  // 이동 속도
    [SerializeField] private float jumpPower = 15.0f; // 점프 힘

    // 플레이어 이동, 걷기, 점프
    private float h;
    private float v;
    private bool walkDown; 
    private bool jumpDown;  

    private Vector3 moveVec; // 이동 방향
    private Vector3 dodgeVec; // 회피 방향

    // internal : 같은 어셈블리(프로젝트) 에서만 접근 허용
    internal bool isJump;
    internal bool isDodge;

    internal Rigidbody rigid;
    internal Animator anim;
    private  PlayerInteraction pi;
    #endregion

    #region Init (초기화)
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
        pi = GetComponent<PlayerInteraction>();  
    }
    #endregion

    #region Update
    void Update()
    {
        GetInput();     // 입력
        Move();         // 이동
        Rotate();       // 회전
        Jump();         // 점프
        Dodge();        // 회피
    }
    #endregion

    #region Input (입력)
    void GetInput()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        walkDown = Input.GetButton("Walk");
        jumpDown = Input.GetButtonDown("Jump");
    }
    #endregion

    #region Move (이동)
    void Move()
    {
        moveVec = new Vector3(h, 0, v).normalized;

        if (pi.isSwap || !pi.isFireReady || pi.isReload)
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
    #endregion

    #region Rotate (회전)
    void Rotate()
    {
        // LookAt() : 지정된 벡터를 향해서 회전시켜주는 함수
        // 키보드에 의한 회전
        transform.LookAt(transform.position + moveVec);

        // 마우스에 의한 회전
        if (pi.fDown)
        {
            Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
            if (Physics.Raycast(ray, out rayHit, 100))
            {
                Vector3 nextVec = rayHit.point - transform.position;
                nextVec.y = 0;
                transform.LookAt(transform.position + nextVec);
            }
        }
    }
    #endregion

    #region Jump (점프)
    void Jump()
    {
        if (jumpDown && !isJump && moveVec == Vector3.zero && !isDodge && !pi.isSwap)
        {
            // ForceMode.Impulse : 즉발적인 힘을 가함
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;
        }
    }
    #endregion

    #region Dodge (회피)
    void Dodge() 
    {
        if (jumpDown && !isJump && moveVec != Vector3.zero && !isDodge && !pi.isSwap)
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
    #endregion

    #region Ground Check (지면 체크)
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            anim.SetBool("isJump", false);
            isJump = false;
        }
    }
    #endregion
}
