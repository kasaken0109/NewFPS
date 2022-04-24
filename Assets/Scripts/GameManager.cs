using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using DG.Tweening;
public class GameManager : MonoBehaviour
{

    [SerializeField]
    private GameObject m_bossEnemy = null;

    [SerializeField]
    [Tooltip("プレイヤーのバーチャルカメラ")]
    private CinemachineVirtualCamera virtualCamera = default;

    [SerializeField]
    [Tooltip("メニュー")]
    private GameObject menu = default;

    [SerializeField]
    private GameObject m_win = null;

    [SerializeField]
    private GameObject m_gate = null;

    [SerializeField]
    private GameObject m_lose = null;

    public static GameManager Instance = null;

    public PlayerManager m_player;
    static public PlayerManager Player => Instance.m_player;

    //適当なのでちゃんとしたシングルトンではない
    void Awake()
    {
        Instance = this;
        PlayerPrefs.SetString("SceneName",SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Menu"))
        {
            myGameState = !menu.activeInHierarchy ? GameState.STOP : GameState.RESUME;
            SetGameConnditoin();
        }
    }

    private void SetGameConnditoin()
    {
        switch (myGameState)
        {
            case GameState.PLAYING:
                break;
            case GameState.STOP:
                Cursor.visible = true;
                Time.timeScale = 0f;
                virtualCamera.enabled = false;
                menu.SetActive(true);
                break;
            case GameState.RESUME:
                Cursor.visible = false;
                Time.timeScale = 1;
                virtualCamera.enabled = true;
                menu.SetActive(false);
                break;
            case GameState.PLAYERWIN:
                m_win.SetActive(true);
                m_gate?.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                break;
            case GameState.PLAYERLOSE:
                m_lose.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                break;
            default:
                break;
        }
    }

    public void SetGameState(GameState gameState)
    {
        myGameState = gameState;
        SetGameConnditoin();
    }

    private GameState myGameState;

    public GameState GameStatus => myGameState;

    public enum GameState
    {
        PLAYING,
        STOP,
        RESUME,
        PLAYERWIN,
        PLAYERLOSE,

    }
}
