using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameManager : MonoBehaviour
{ 
    private static GameManager instance = null;

    public GameObject mItemShop;
    public GameObject mWeaponShop;
    public GameObject mStartZone;

    private PlayerMovement mPlayer;
    private Enemy mEnemy;
    public Enemy[] mEnemyGroup;
    public GameObject[] mEnemies;
    private static int mScore;
    private static float mTime;
    private static int mEnemyCount;
    private static int mStage;
    public static int mGold;
    private static int mCurrentGold;
    private static bool mIsBattle;


    public int mEnemyCntA;
    public int mEnemyCntB;
    public int mEnemyCntC;
    public int mEnemyCntD;


    public Transform[] mEnemyZones;
    public List<int> mEnemyList;

    private GameManager()
    {
        // 외부에서 생성하지 못하도록 private로 생성
    }

    public static GameManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    private void Awake()
    {
        mEnemyList = new List<int>();

        if (instance = null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);

        }
    }

    private void Start()
    {
        mIsBattle = true;
        StartCoroutine(InBattle());
    }

    public void StageStart()
    {
        mItemShop.SetActive(false);
        mWeaponShop.SetActive(false);
        mStartZone.SetActive(false);

        foreach (Transform zone in mEnemyZones)
        {
            zone.gameObject.SetActive(true);
        }

        mIsBattle = true;
        StartCoroutine(InBattle());
    }

    IEnumerator InBattle()
    {
        if(mStage % 5 == 0)
        {
            mEnemyCntD++;
            GameObject instantEnemy = Instantiate(mEnemies[3], mEnemyZones[0].position, mEnemyZones[0].rotation);
            Enemy enemy = instantEnemy.GetComponent<Enemy>();
            enemy._target = mPlayer.transform;
            //Boss boss = instantEnemy.GetComponent<Boss>();
        }
        else
        {
            for (int i = 0; i < mStage; i++)
            {
                int ran = Random.Range(0, 3);
                mEnemyList.Add(ran);

                switch (ran)
                {
                    case 0:
                        mEnemyCntA++;
                        break;
                    case 1:
                        mEnemyCntB++;
                        break;
                    case 2:
                        mEnemyCntC++;
                        break;
                }
            }

            while (mEnemyList.Count > 0)
            {
                int ranZone = Random.Range(0, 4);
                GameObject instantEnemy = Instantiate(mEnemies[mEnemyList[0]], mEnemyZones[ranZone].position, mEnemyZones[ranZone].rotation);
                Enemy enemy = instantEnemy.GetComponent<Enemy>();
                enemy._target = mPlayer.transform;
                mEnemyList.RemoveAt(0);
                yield return new WaitForSeconds(4f);
            }
        }
        while(mEnemyCntA + mEnemyCntB + mEnemyCntC + mEnemyCntD > 0)
        {
            yield return null;
        }
    }

    public void StageEnd()
    {
        mPlayer.transform.position = Vector3.up * 0.8f;

        foreach (Transform zone in mEnemyZones)
        {
            zone.gameObject.SetActive(false);
        }

        mIsBattle = false;
        mItemShop.SetActive(true);
        mWeaponShop.SetActive(true);
        mStartZone.SetActive(true);
        mStage++;
    }

    public void GameOver()
    {
        EditorApplication.isPlaying = true;
        //Application.Quit();
    }

    public void GamePause()
    {
        EditorApplication.isPaused = true;
    }

    private void Update()
    {
        mTime = Time.time;
        if(CheckEnemyDie(mEnemyGroup) == true)
        {
            GetGold(mEnemy);
        }
    }

    public void GetGold(Enemy enemy)
    {
        mCurrentGold += enemy._gold;
    }

    private bool CheckEnemyDie(Enemy[] enemies)
    {
        for (int i = 0; i < enemies.GetLength(0); ++i)
        {
            if (enemies[i].curHealth <= 0)
            {
                mEnemy = enemies[i];
                return true;
            }
        }
        return false;
    }
}
