using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    #region Variables

    public GameObject[] weapons; // ���⸦ ���� �迭
    public bool[] hasWeapons; // ���� ������ �ִ� ����
    private bool interDown; // ��ȣ�ۿ� ��ư
    internal bool isSwap; // ���� ��ȯ
    internal bool fDown; // ���� Ű 
    internal bool isFireReady; // ���� �غ�
    private float fireDelay; // ���� ������
    private int weaponIndex = -1; 
    private int equipWeaponIndex = -1;
    private bool s1, s2, s3; // ���� ��ü
    bool isShop = false;

    private PlayerMovement pm;
    private GameObject nearObject; // ��ó�� �ִ� ������Ʈ 
    internal Weapon equipWeapon;
    #endregion

    #region Start
    void Start()
    {
        pm = GetComponent<PlayerMovement>();
        isFireReady = true;
    }
    #endregion

    #region Update
    void Update()
    {
        GetInput();
        Interaction();
        Swap();
        Attack();
    }
    #endregion

    #region Input
    void GetInput()
    {
        interDown = Input.GetButtonDown("Interaction");
        s1 = Input.GetButtonDown("Swap1");
        s2 = Input.GetButtonDown("Swap2");
        s3 = Input.GetButtonDown("Swap3");
        fDown = Input.GetButtonDown("Fire1");
    }
    #endregion

    #region Swap (���� ��ȯ)
    void Swap() // ���� ��ȯ
    {
        if (s1 && (!hasWeapons[0] || equipWeaponIndex == 0))
            return;
        if (s2 && (!hasWeapons[1] || equipWeaponIndex == 1))
            return;
        if (s3 && (!hasWeapons[2] || equipWeaponIndex == 2))
            return;

        if (s1) weaponIndex = 0;
        if (s2) weaponIndex = 1;
        if (s3) weaponIndex = 2;

        if ((s1 || s2 || s3) && !pm.isJump && !pm.isDodge)
        {
            if (equipWeapon != null)
            {
                equipWeapon.gameObject.SetActive(false);
            }
            equipWeaponIndex = weaponIndex;
            equipWeapon = weapons[weaponIndex].GetComponent<Weapon>();
            equipWeapon.gameObject.SetActive(true);

            pm.anim.SetTrigger("doSwap");
            isSwap = true;

            Invoke("SwapOut", 0.4f);
        }
    }
    void SwapOut()
    {
        isSwap = false;
    }
    #endregion

    #region Interaction (��ȣ �ۿ�)
    public void Interaction()
    {
        if (interDown && nearObject != null && !pm.isJump && !pm.isDodge)
        {
            if (nearObject.CompareTag("Weapon"))
            {
                Item item = nearObject.GetComponent<Item>();
                int weaponIndex = item.value;
                hasWeapons[weaponIndex] = true;

                Destroy(nearObject);
            }
            else if (nearObject.CompareTag("Shop"))
            {
                Shop shop = nearObject.GetComponent<Shop>();
                shop.Enter(this);
                Debug.Log("Enter the Shop");
                isShop = true;
            }
        }
    }
    #endregion

    #region Attack (����)
    void Attack()
    {
        if (equipWeapon == null)
        {
            return;
        }
        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;

        if (fDown && isFireReady && !pm.isDodge && !isSwap)
        {
            equipWeapon.Use();
            pm.anim.SetTrigger("doSwing");
            fireDelay = 0;
        }
    }
    #endregion

    #region Trigger (������ �ݱ�)
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon") || other.gameObject.CompareTag("Shop"))
            nearObject = other.gameObject;

        //Debug.Log(nearObject.name);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon"))
            nearObject = null;
        else if (other.gameObject.CompareTag("Shop")) {
            Shop shop = nearObject.GetComponent<Shop>();
            shop.Exit();
            isShop = false;
            nearObject = null;
        }
    }
    #endregion
}
