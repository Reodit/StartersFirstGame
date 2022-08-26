using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameManager : MonoBehaviour
{ 
    private static GameManager instance = null;
    private Player mPlayer;
    private Enemy mEnemy;
    public Enemy[] mEnemyGroup;
    private static int mScore;
    private static float mTime;
    private static int mEnemyCount;
    private static int mStage;
    public static int mGold;
    private static int mCurrentGold;

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
        if(instance = null)
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
