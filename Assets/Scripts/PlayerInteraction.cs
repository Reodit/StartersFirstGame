using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public GameObject[] weapons;
    public bool[] hasWeapons;

    private bool interDown;

    private Player player;
    GameObject nearObject;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        interDown = Input.GetButtonDown("Interaction");

        Interaction();
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
        if (other.tag == "Weapon")
            nearObject = other.gameObject;

        Debug.Log(nearObject.name);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon")
            nearObject = null;
    }
}
