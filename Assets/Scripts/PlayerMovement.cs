using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��ũ��Ʈ���� region, endregion�� ����Ͽ� Ȯ�� �� ��� �Ҽ��ִ� �ڵ� ����� �����Ҽ��ִ�.
public class PlayerMovement : MonoBehaviour
{
    #region Player Variables
    public Camera followCamera;

    [SerializeField] private float speed = 5.0f;  // �̵� �ӵ�
    [SerializeField] private float jumpPower = 15.0f; // ���� ��

    // �÷��̾� �̵�, �ȱ�, ����
    private float h;
    private float v;
    private bool walkDown; 
    private bool jumpDown;  

    private Vector3 moveVec; // �̵� ����
    private Vector3 dodgeVec; // ȸ�� ����

    // internal : ���� �����(������Ʈ) ������ ���� ���
    internal bool isJump;
    internal bool isDodge;

    internal Rigidbody rigid;
    internal Animator anim;
    private  PlayerInteraction pi;
    #endregion

    #region Init (�ʱ�ȭ)
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
        GetInput();     // �Է�
        Move();         // �̵�
        Rotate();       // ȸ��
        Jump();         // ����
        Dodge();        // ȸ��
    }
    #endregion

    #region Input (�Է�)
    void GetInput()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        walkDown = Input.GetButton("Walk");
        jumpDown = Input.GetButtonDown("Jump");
    }
    #endregion

    #region Move (�̵�)
    void Move()
    {
        moveVec = new Vector3(h, 0, v).normalized;

        if (pi.isSwap || !pi.isFireReady || pi.isReload)
            moveVec = Vector3.zero;

        // ���� ȸ�����̸�? moveVec�� dodgeVec ����
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

    #region Rotate (ȸ��)
    void Rotate()
    {
        // LookAt() : ������ ���͸� ���ؼ� ȸ�������ִ� �Լ�
        // Ű���忡 ���� ȸ��
        transform.LookAt(transform.position + moveVec);

        // ���콺�� ���� ȸ��
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

    #region Jump (����)
    void Jump()
    {
        if (jumpDown && !isJump && moveVec == Vector3.zero && !isDodge && !pi.isSwap)
        {
            // ForceMode.Impulse : ������� ���� ����
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;
        }
    }
    #endregion

    #region Dodge (ȸ��)
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

    #region Ground Check (���� üũ)
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
