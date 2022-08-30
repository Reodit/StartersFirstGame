using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    public GameObject[] grenades;

    public int ammo =0;
    public int coin = 0;
    public int health = 100;
    public int hasGrenade;

    public int maxAmmo =1000;
    public int maxCoin = 100000;
    public int maxHealth = 100;
    public int maxHasGrenade = 4;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Item"))
        {
            Item item = other.GetComponent<Item>();

            switch (item.type)
            {
                case Item.Type.Ammo:
                    ammo += item.value;
                    if (ammo > maxAmmo)
                        ammo = maxAmmo;
                    break;
                case Item.Type.Coin:
                    coin += item.value;
                    if (coin > maxCoin)
                        coin = maxCoin;
                    break;
                case Item.Type.Heart:
                    health += item.value;
                    if (health > maxHealth)
                        health = maxHealth;
                    break;
                case Item.Type.Grenade:
                    grenades[hasGrenade].SetActive(true);
                    hasGrenade += item.value;
                    if (hasGrenade > maxHasGrenade)
                        hasGrenade = maxHasGrenade;
                    break;
            }
            Destroy(other.gameObject);
        }
    }
}
