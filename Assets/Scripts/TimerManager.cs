using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// タイマーの表示と制御を行う
/// </summary>
public class TimerManager : MonoBehaviour
{
    public float timeLimit = 20f;
    public bool m_menu = false;
    [SerializeField]
    private Text m_text = null;
    [SerializeField]
    private GameObject m_gameOver = null;
    [SerializeField]
    private GameObject m_gameClear = null;
    
    Animation m_anim = null;
    int maxTimeLimit;
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("MaxTime", (int)timeLimit);
        PlayerPrefs.Save();
        m_gameClear.SetActive(false);
        maxTimeLimit = (int)timeLimit;
        m_anim = gameObject.GetComponent<Animation>();
        m_menu = false;
    }

    // Update is called once per frame
    void Update()
    {
        m_text.text = "制限時間：" + Mathf.CeilToInt(timeLimit);
        if (GameManager.Instance.GameStatus == GameManager.GameState.PLAYERWIN)
        {
            PlayerPrefs.SetInt("TimeScore", maxTimeLimit - Mathf.CeilToInt(timeLimit));
            PlayerPrefs.Save();
        }
        else if (GameManager.Instance.GameStatus == GameManager.GameState.PLAYERLOSE)
        {
            PlayerPrefs.SetInt("TimeScore", maxTimeLimit);
            PlayerPrefs.Save();
        }
        else
        {
            timeLimit -= Time.deltaTime;
        }
        
        if (timeLimit <= 10f)
        {
            m_text.color = Color.red;
            m_anim.Play();
            if (timeLimit <= 0)
            {
                m_text.text = "のこり時間：" + 0;
                m_gameOver.SetActive(true);
                m_menu = true;
            }
        }
    }

    public enum GameState
    {
        PLAYING,
        PAUSE,
        CLEAR,
        GAMEOVER,
    }
}
