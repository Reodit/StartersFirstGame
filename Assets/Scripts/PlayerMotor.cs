using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerMotor : MonoBehaviour
{

    #region Variables
    public GameObject[] grenades;  // 수류탄을 담을 배열

    public int ammo = 0; // 총알 개수
    public int coin = 0; // 코인
    public int health = 100; // 체력
    public int hasGrenade; // 가지고 있는 수류탄

    public int maxAmmo = 1000; // 최대 총알 개수
    public int maxCoin = 100000; // 최대 코인 개수
    public int maxHealth = 100; // 최대 체력
    public int maxHasGrenade = 4; // 최대 수류탄 개수

    #endregion

    #region 아이템 줍기 (총알, 코인, 체력, 수류탄 등)
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Item"))
        {
            Item item = other.GetComponent<Item>();

            switch (item.type)
            {
                case Item.Type.Ammo: // 총알
                    ammo += item.value;
                    if (ammo > maxAmmo)
                        ammo = maxAmmo;
                    break;
                case Item.Type.Coin: // 코인
                    coin += item.value;
                    if (coin > maxCoin)
                        coin = maxCoin;
                    break;
                case Item.Type.Heart: // 체력
                    health += item.value;
                    if (health > maxHealth)
                        health = maxHealth;
                    break;
                case Item.Type.Grenade: // 수류탄
                    grenades[hasGrenade].SetActive(true);
                    hasGrenade += item.value; 
                    if (hasGrenade > maxHasGrenade)
                        hasGrenade = maxHasGrenade;
                    break;
            }
            Destroy(other.gameObject);
        }
    }
    #endregion
}
