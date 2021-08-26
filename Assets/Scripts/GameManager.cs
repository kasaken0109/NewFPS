using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public PlayerManager _player;
    [SerializeField] GameObject m_bossEnemy = null;
    [SerializeField] Transform m_bossSpawn = null;

    public static GameManager _instance = null;
    static public PlayerManager Player => _instance._player;

    //適当なのでちゃんとしたシングルトンではない
    void Awake()
    {
        //_player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        _instance = this;
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
    }
}
