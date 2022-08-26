using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    Player player;
    GameManager instance;
    public RectTransform uiGroup;
    public Animator anim;

    public GameObject[] itemObj;
    public Transform[] itemPos;

    //public void Enter(Player player)
    //{
    //    enterPlayer = player;
    //    uiGroup.anchoredPosition = Vector3.zero;
    //}

    public void Exit()
    {
        anim.SetTrigger("doHello");
        uiGroup.anchoredPosition = Vector3.down * 1000;
    }

    void Start()
    {
       GameObject myPlayer = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            //상점 UI 열
        }
    }
}
