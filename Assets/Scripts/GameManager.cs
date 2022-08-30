using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{ 
    private static GameManager instance = null;

    public GameObject menuCam;
    public GameObject gameCam;
    public PlayerMotor player;
    //public Boss boss;
    public int stage;

    public GameObject menuPanel;
    public GameObject gamePanel;
    public Text maxScoreTxt;
    public Text scoreTxt;
    public Text stageTxt;
    public Text playTimeTxt;
    public Text playerHealthTxt;
    public Text playerAmmoTxt;
    public Text playerCoinTxt;
    public Image weapon1Img;
    public Image weapon2Img;
    public Image weapon3Img;
    public Image weapon4Img;
    public Text enemyATxt;
    public Text enemyBTxt;
    public Text enemyCTxt;
    public RectTransform bossHealthGroup;
    public RectTransform bossHealthBar;


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
            //Destroy(this.gameObject);

        }

        //maxScoreTxt.text = string.Format("{0:n0}", PlayerPrefs.GetInt("MaxScore"));
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
        if (mIsBattle)
        {
            mTime = Time.time;
        }
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

    public void GameStart()
    {
        Debug.Log("Game Start");
        menuCam.SetActive(false);
        gameCam.SetActive(true);

        menuPanel.SetActive(false);
        gamePanel.SetActive(true);

        player.gameObject.SetActive(true);

    }

    private void LateUpdate()
    {
        //상단 UI
        //scoreTxt.text = string.Format("{0:n0}", player.score);
        stageTxt.text = "STAGE" + stage;

        int hour = (int)(mTime / 3600);
        int min = (int)((mTime - hour * 3600) / 60);
        int second = (int)(mTime % 60);
        playTimeTxt.text = string.Format("{0:00}", hour) + ":" + string.Format("{0:00}", min) + ":" + string.Format("{0:00}", second);

        //플레이어 UI
        playerHealthTxt.text = player.health + " / " + player.maxHealth;
        playerCoinTxt.text = string.Format("{0:n0}", player.coin);

        //if (player.equipWeapon == null)
        //{
        //    playerAmmoTxt.text = "- / " + player.ammo;
        //}
        //else if (player.equipWeapon.type == Weapon.Type.Melee)
        //{
        //    playerAmmoTxt.text = "- / " + player.ammo;
        //}
        //else
        //{
        //    playerAmmoTxt.text = player.equipWeapon.curAmmo + " / " + player.ammo;
        //}

        //무기 UI
        //weapon1Img.color = new Color(1, 1, 1, player.hasWeapons[0] ? 1 : 0);
        //weapon2Img.color = new Color(1, 1, 1, player.hasWeapons[0] ? 1 : 0);
        //weapon3Img.color = new Color(1, 1, 1, player.hasWeapons[0] ? 1 : 0);
        //weapon4Img.color = new Color(1, 1, 1, player.hasGrenades > 0 ? 1 : 0);

        //몬스터 수 UI
        enemyATxt.text = mEnemyCntA.ToString();
        enemyBTxt.text = mEnemyCntB.ToString();
        enemyCTxt.text = mEnemyCntC.ToString();

        //bossHealthBar.localScale = new Vector3(boss.curHealth / boss.maxHealth, 1, 1);

    }
}
