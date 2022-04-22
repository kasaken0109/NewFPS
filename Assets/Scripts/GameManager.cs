using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class GameManager : MonoBehaviour
{
    
    [SerializeField]
    private GameObject m_bossEnemy = null;
    [SerializeField]
    private Transform m_bossSpawn = null;
    public GameObject m_shootweaponImage;
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
        PlayerPrefs.SetInt("Bullet1", 4);
        PlayerPrefs.SetInt("Bullet2", 4);
        PlayerPrefs.SetString("SceneName",SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();
    }

    private void FixedUpdate()
    {
        switch (myGameState)
        {
            case GameState.PLAYERWIN:m_win.SetActive(true);
                m_gate?.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                break;
            case GameState.PLAYERLOSE:m_lose.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                break;
            default:
                break;
        }
    }

    public void ShakeCamera()
    {
        StartCoroutine(nameof(FixCameraPos));
    }

    private GameState myGameState;

    public GameState GameStatus
    {
        get { return myGameState; }
        set { myGameState = value; }
    }

    public enum GameState
    {
        PLAYING,
        STOP,
        RESUME,
        PLAYERWIN,
        PLAYERLOSE,

    }

    IEnumerator FixCameraPos()
    {
        Vector3 pos = Camera.main.transform.position;
        Quaternion rot = Camera.main.transform.rotation;
        iTween.ShakePosition(Camera.main.gameObject, iTween.Hash("x", 0.5, "y", 0.5, "time", 1));
        yield return new WaitForSeconds(1f);
        Camera.main.transform.position = pos;
        Camera.main.transform.rotation = rot;
    }

    public void CinemaMode()
    {
        StartCoroutine(nameof(SetPlayerCameraInput));
    }

    IEnumerator SetPlayerCameraInput()
    {
        PlayerControll.Instance.SetMoveActive(false);
        yield return new WaitForSeconds(1.5f);
        PlayerControll.Instance.SetMoveActive(true);
    }
}
