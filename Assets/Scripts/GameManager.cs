using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] PlayerManager _player;

    static GameManager _instance = null;
    static public PlayerManager Player => _instance._player;

    //適当なのでちゃんとしたシングルトンではない
    void Awake()
    {
        _instance = this;
    }
}
