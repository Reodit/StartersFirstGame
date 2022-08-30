using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public GameObject[] weapons;
    public bool[] hasWeapons;
    
    private bool interDown;
    public bool isSwap;
    private int weaponIndex = -1;
    private int equipWeaponIndex = -1;
    private bool s1, s2, s3; // 무기 교체

    private PlayerMovement player;
    GameObject nearObject;
    GameObject equipWeapon;

    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerMovement>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Interaction();
        Swap();
    }
    void GetInput()
    {
        interDown = Input.GetButtonDown("Interaction");
        s1 = Input.GetButtonDown("Swap1");
        s2 = Input.GetButtonDown("Swap2");
        s3 = Input.GetButtonDown("Swap3");

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

        if ((s1 || s2 || s3) && !player.isJump && !player.isDodge)
        {
            if (equipWeapon != null)
            {
                equipWeapon.SetActive(false);
            }
            equipWeaponIndex = weaponIndex;
            equipWeapon = weapons[weaponIndex];
            equipWeapon.SetActive(true);

            animator.SetTrigger("doSwap");
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
        if (interDown && nearObject != null && !player.isJump && !player.isDodge)
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
