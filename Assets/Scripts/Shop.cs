using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    PlayerInteraction enterPlayer;
    public RectTransform uiGroup;
    public Animator anim;

    public GameObject[] itemObj;
    public int[] itmePrice;
    public Transform[] itemPos;
    public string[] talkData;
    public Text talkText;
    public GameManager instance;

    public void Enter(PlayerInteraction player)
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
            //상점열
        }
    }

    public void Buy(int index)
    {
        int price = itmePrice[index];
        if(price > instance.Gold)
        {
            StopCoroutine(Talk());
            StartCoroutine(Talk());
            Debug.Log("Buy Item");
            return;
        }

        instance.Gold -= price;
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
