using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    #region Variables

    public GameObject[] weapons; // 무기를 담을 배열 
    [SerializeField] private bool[] hasWeapons; // 현재 가지고 있는 무기
    private bool interDown; // 상호작용 버튼
    internal bool isSwap; // 무기 변환
    internal bool fDown; // 공격 키 
    internal bool isFireReady; // 공격 준비
    private float fireDelay; // 공격 딜레읻
    private int weaponIndex = -1; 
    private int equipWeaponIndex = -1;
    private bool s1, s2, s3; // 무기 교체

    private PlayerMovement pm;
    private GameObject nearObject; // 근처에 있는 오브젝트 
    private Weapon equipWeapon;
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

    #region Swap (무기 변환)
    void Swap() // 무기 변환
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

    #region Interaction (상호 작용)
    void Interaction()
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
        }
    }
    #endregion

    #region Attack (공격)
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

    #region Trigger (아이템 줍기)
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon"))
            nearObject = other.gameObject;

        //Debug.Log(nearObject.name);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon"))
            nearObject = null;
    }
    #endregion
}
