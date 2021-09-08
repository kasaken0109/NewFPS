using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// シングルトン パターンを使って値を保持するコンポーネント
/// </summary>
public class SingletonPattern01 : MonoBehaviour
{
    /// <summary>このクラスのインスタンスが既にあるかどうか</summary>
    static bool m_isExists = false;     // static 変数であることに注意
    public string Name = "Default";     // サンプルなのでカプセル化はしていない
    public int Life = 100;              // サンプルなのでカプセル化はしていない

    void Awake()
    {
        if (m_isExists)
        {
            Debug.LogWarningFormat("SingletonPattern01 を持ったゲームオブジェクトは既にあるので、{0} は破棄する", this.gameObject.name);
            Destroy(this.gameObject);
        }
        else
        {
            // SingletonPattern01 のインスタンスがない場合は、自分を DontDestroyOnload に置く。
            m_isExists = true;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    /// <summary>
    /// インスタンス内の情報を文字列として返す
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        string s = "Name: " + this.Name + "\r\n";
        s += "Life: " + this.Life + "\r\n";
        s += "GameObject: " + this.name;    // オブジェクト名も情報として返す
        return s;
    }
}
