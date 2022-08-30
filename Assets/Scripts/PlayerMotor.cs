using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerMotor : MonoBehaviour
{

    #region Variables
    public GameObject[] grenades;  // ����ź�� ���� �迭

    public int ammo = 0; // �Ѿ� ����
    public int coin = 0; // ����
    public int health = 100; // ü��
    public int hasGrenade; // ������ �ִ� ����ź

    public int maxAmmo = 1000; // �ִ� �Ѿ� ����
    public int maxCoin = 100000; // �ִ� ���� ����
    public int maxHealth = 100; // �ִ� ü��
    public int maxHasGrenade = 4; // �ִ� ����ź ����

    #endregion

    #region ������ �ݱ� (�Ѿ�, ����, ü��, ����ź ��)
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Item"))
        {
            Item item = other.GetComponent<Item>();

            switch (item.type)
            {
                case Item.Type.Ammo: // �Ѿ�
                    ammo += item.value;
                    if (ammo > maxAmmo)
                        ammo = maxAmmo;
                    break;
                case Item.Type.Coin: // ����
                    coin += item.value;
                    if (coin > maxCoin)
                        coin = maxCoin;
                    break;
                case Item.Type.Heart: // ü��
                    health += item.value;
                    if (health > maxHealth)
                        health = maxHealth;
                    break;
                case Item.Type.Grenade: // ����ź
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
