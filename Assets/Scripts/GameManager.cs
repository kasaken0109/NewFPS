using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public PlayerManager m_player;
    [SerializeField] GameObject m_bossEnemy = null;
    [SerializeField] Transform m_bossSpawn = null;

    public static GameManager Instance = null;
    static public PlayerManager Player => Instance.m_player;

    //適当なのでちゃんとしたシングルトンではない
    void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Spawn"))
        {
            Instantiate(m_bossEnemy,m_bossSpawn.position,m_bossSpawn.rotation);
        }
    }

    public void ShakeCamera()
    {
        iTween.ShakePosition(Camera.main.gameObject, iTween.Hash("x", 0.5, "y", 0.5, "time", 1));
        StartCoroutine("SetPlayerCameraInput");
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
