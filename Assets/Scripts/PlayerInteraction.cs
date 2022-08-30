using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public GameObject[] weapons;
    public bool[] hasWeapons;
    
    private bool interDown;
    internal bool isSwap;
    internal bool isFireReady;
    internal bool fDown;

    public float fireDelay;

    private int weaponIndex = -1;
    private int equipWeaponIndex = -1;
    private bool s1, s2, s3; // 무기 교체

    PlayerMovement playerMovement;
    GameObject nearObject;
    Weapon equipWeapon;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        isFireReady = true;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Interaction();
        Swap();
        Attack();
    }
    void GetInput()
    {
        interDown = Input.GetButtonDown("Interaction");
        s1 = Input.GetButtonDown("Swap1");
        s2 = Input.GetButtonDown("Swap2");
        s3 = Input.GetButtonDown("Swap3");
        fDown = Input.GetButtonDown("Fire1");

    }
    void Swap()
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

        if ((s1 || s2 || s3) && !playerMovement.isJump && !playerMovement.isDodge)
        {
            if (equipWeapon != null)
            {
                equipWeapon.gameObject.SetActive(false);
            }
            equipWeaponIndex = weaponIndex;
            equipWeapon = weapons[weaponIndex].GetComponent<Weapon>();
            equipWeapon.gameObject.SetActive(true);

            playerMovement.anim.SetTrigger("doSwap");
            isSwap = true;

            Invoke("SwapOut", 0.4f);
        }
    }
    void SwapOut()
    {
        isSwap = false;
    }
    void Interaction()
    {
        if (interDown && nearObject != null && !playerMovement.isJump && !playerMovement.isDodge)
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
    void Attack()
    {
        if (equipWeapon == null)
        {
            return;
        }
        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;

        if (fDown && isFireReady && !playerMovement.isDodge && !isSwap)
        {
            equipWeapon.Use();
            playerMovement.anim.SetTrigger("doSwing");
            fireDelay = 0;
        }
    }
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
}
