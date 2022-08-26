using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    Player enterPlayer;
    public RectTransform uiGroup;
    public Animator anim;

    public GameObject[] itemObj;
    public int[] itmePrice;
    public Transform[] itemPos;
    public string[] talkData;
    public Text talkText;

    public void Enter(Player player)
    {
        enterPlayer = player;
        uiGroup.anchoredPosition = Vector3.zero;
    }

    public void Exit()
    {
        anim.SetTrigger("doHello");
        uiGroup.anchoredPosition = Vector3.down * 1000;
    }

    void Start()
    {
       GameObject myPlayer = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            //상점 UI 열
        }
    }

    public void Buy(int index)
    {
        int price = itmePrice[index];
        if(price > GameManager.mGold)
        {
            StopCoroutine(Talk());
            StartCoroutine(Talk());
            return;
        }

        GameManager.mGold -= price;
        Vector3 ranVec = Vector3.right * Random.Range(-3, 3) + Vector3.forward * Random.Range(-3, 3);
        Instantiate(itemObj[index], itemPos[index].position + ranVec,itemPos[index].rotation);
    }

    IEnumerator Talk()
    {
        talkText.text = talkData[1];
        yield return new WaitForSeconds(2f);
        talkText.text = talkData[0];
    }
}
