using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// static な変数・関数の使用例
/// </summary>
public class StaticExample : MonoBehaviour
{
    void Start()
    {
        Test();
    }

    void Test()
    {
        StaticExampleClass x = new StaticExampleClass();
        StaticExampleClass y = new StaticExampleClass();

        StaticExampleClass.a = 65535;   // static なメンバ変数に外部からアクセスするにはインスタンスではなくクラスを指定してアクセスする
        x.b = 255;  // 普通のメンバ変数にアクセスする

        Debug.LogFormat("y.b: {0}, y.c: {1}", y.b, y.c);    // y.c の結果に注目

        Debug.LogFormat("StaticExampleClass.Func(): {0}", StaticExampleClass.Func());   // static なメンバ関数もやはりインスタンスではなくクラスを指定してアクセスする
    }
}

public class StaticExampleClass
{
    public static int a = 0;
    public int b = 0;
    public int c { get { return a; } }

    public static int Func()
    {
        // return b;    // これはできない
        // return c;    // これもできない
        return a;       // これはできる（a は static だから）
    }
}
