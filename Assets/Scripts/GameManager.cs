using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class GameManager : MonoBehaviour
{
    public PlayerManager m_player;
    [SerializeField] GameObject m_bossEnemy = null;
    [SerializeField] Transform m_bossSpawn = null;
    public GameObject m_shootweaponImage;
    [SerializeField] GameObject m_win = null;
    [SerializeField] GameObject m_gate = null;
    [SerializeField] GameObject m_lose = null;

    public static GameManager Instance = null;
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

    private void Update()
    {
        if (Input.GetButtonDown("Spawn"))
        {
            Instantiate(m_bossEnemy,m_bossSpawn.position,m_bossSpawn.rotation);
        }
    }

    private void FixedUpdate()
    {
        switch (myGameState)
        {
            case GameState.PLAYERWIN:m_win.SetActive(true);
                m_gate?.SetActive(true);
                //Cursor.visible = true;
                //Cursor.lockState = CursorLockMode.Confined;
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
        //Camera.main.transform.DOMove(pos, 0.5f);


    }

    public void CinemaMode()
    {
        StartCoroutine(nameof(SetPlayerCameraInput));
    }

    IEnumerator SetPlayerCameraInput()
    {
        CameraController.Instance.SetMoveActive(false);
        PlayerControll.Instance.SetMoveActive(false);
        yield return new WaitForSeconds(1.5f);
        CameraController.Instance.SetMoveActive(true);
        PlayerControll.Instance.SetMoveActive(true);
    }
}
