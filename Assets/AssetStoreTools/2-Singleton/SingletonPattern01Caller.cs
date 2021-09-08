using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SingletonPattern01 を「使う」クラス
/// </summary>
public class SingletonPattern01Caller : MonoBehaviour
{
    SingletonPattern01 singleton = null;

    void Start()
    {
        singleton = GameObject.FindObjectOfType<SingletonPattern01>();
        
        if (singleton)
        {
            Debug.Log(singleton.ToString());
        }
        else
        {
            Debug.LogError("オブジェクトが見つかりませんでした");
        }
    }

    /// <summary>
    /// ランダムな 10 文字の文字列を生成し、保存する
    /// </summary>
    public void SetRandomName()
    {
        // ランダムな 10 文字の文字列を生成する
        string random = System.Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper();
        singleton.Name = random;
        Debug.Log(singleton.ToString());
    }

    /// <summary>
    /// ランダムな整数値を保存する
    /// </summary>
    public void SetRandomLife()
    {
        singleton.Life = Random.Range(int.MinValue, int.MaxValue);
        Debug.Log(singleton.ToString());
    }

    public void SetName(string name)
    {
        singleton.Name = name;
    }

    public void SetLife(int life)
    {
        singleton.Life = life;
    }
}
