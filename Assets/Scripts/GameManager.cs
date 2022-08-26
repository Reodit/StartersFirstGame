using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    private GameManager()
    {
        // 외부에서 생성하지 못하도록 private로 생성
        // 변수 넣어주기
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

    public void GameOver()
    {
        EditorApplication.isPlaying = true;
        //Application.Quit();
    }

    public void GamePause()
    {
        EditorApplication.isPaused = true;
    }
}
