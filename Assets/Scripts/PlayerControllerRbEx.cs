﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// Rigidbody を使ってプレイヤーを動かすコンポーネント
/// 入力を受け取り、それに従ってオブジェクトを動かす。
/// PlayerControllerRb との違いは以下の通り。
/// 1. Rigidbody.AddForce() ではなく Rigidbody.velocity で動かしている（※１）
/// 2. World 座標系ではなく、カメラの座標系に対して動かしている（※２）
/// 3. 方向転換時に Quartenion.Slerp() を使って滑らかに方向転換している
/// （※１）AddForce() 動かすことは問題ではなく、挙動や実装を比較するために変えている。
/// （※２）World 座標系で動かすと、カメラの回転に対応できないため
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerControllerRbEx : MonoBehaviour
{
    /// <summary>動く速さ</summary>
    [SerializeField] float m_movingSpeed = 5f;
    /// <summary> 走る速さ</summary>
    [SerializeField] float m_runningSpeed = 8f;
    /// <summary>ターンの速さ</summary>
    [SerializeField] float m_turnSpeed = 3f;
    /// <summary>ジャンプ力</summary>
    [SerializeField] float m_jumpPower = 5f;
    /// <summary>接地判定の際、中心 (Pivot) からどれくらいの距離を「接地している」と判定するかの長さ</summary>
    [SerializeField] float m_isGroundedLength = 1.1f;
    [SerializeField] GameObject m_player = null;
    [SerializeField] Animator m_anim = null;
    Rigidbody m_rb;
    Vector3 dir;
    Vector3 velo;

    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        m_anim = GetComponent<Animator>();
    }

    void Update()
    {
        //方向の入力を取得し、方向を求める
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        // 入力方向のベクトルを組み立てる
        dir = Vector3.forward * v + Vector3.right * h;
        Vector3 dirBody = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;    // メインカメラを基準に入力方向のベクトルを変換する
                                                                                                    // 入力方向に滑らかに回転させる
        if (dirBody != Vector3.zero)
        {
            this.transform.rotation = Quaternion.LookRotation(dirBody);
        }

        if (dir == Vector3.zero)
        {
            // 方向の入力がニュートラルの時は、y 軸方向の速度を保持するだけ
            m_rb.velocity = new Vector3(0f, m_rb.velocity.y, 0f);
            

        }
        else
        {
            // カメラを基準に入力が上下=奥/手前, 左右=左右にキャラクターを向ける
            dir = Camera.main.transform.TransformDirection(dir);    // メインカメラを基準に入力方向のベクトルを変換する
            dir.y = 0;  // y 軸方向はゼロにして水平方向のベクトルにする

            // 入力方向に滑らかに回転させる
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, Time.deltaTime * m_turnSpeed);  // Slerp を使うのがポイント
            IsRunning(); // 入力した方向に移動する
            velo.y = m_rb.velocity.y;   // ジャンプした時の y 軸方向の速度を保持する
            m_rb.velocity = velo;   // 計算した速度ベクトルをセットする
        }

        // ジャンプの入力を取得し、接地している時に押されていたらジャンプする
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            m_anim.SetTrigger("JumpFlag");
            m_rb.AddForce(Vector3.up * m_jumpPower, ForceMode.Impulse);
        }

        if(Input.GetButtonDown("Crouch"))
        {
            m_anim.SetTrigger("CrouchFlag");
        }
        if (v == 0 && h == 0)
        {
            m_anim.SetFloat("Speed", 0);
        }
        else
        {
            if (IsRunning() == true)
            {
                m_anim.SetFloat("Speed", m_runningSpeed);
            }
            else
            {
                m_anim.SetFloat("Speed", m_movingSpeed);
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            m_anim.SetTrigger("ShootFlag");
        }

    }

    /// <summary>
    /// 地面に接触しているか判定する
    /// </summary>
    /// <returns></returns>
    bool IsGrounded()
    {
        // Physics.Linecast() を使って足元から線を張り、そこに何かが衝突していたら true とする
        Vector3 start = this.transform.position;   // start: オブジェクトの中心
        Vector3 end = start + Vector3.down * m_isGroundedLength;  // end: start から真下の地点
        Debug.DrawLine(start, end); // 動作確認用に Scene ウィンドウ上で線を表示する
        bool isGrounded = Physics.Linecast(start, end); // 引いたラインに何かがぶつかっていたら true とする
        return isGrounded;
    }

    bool IsRunning()
    {
        if (Input.GetButton("Splint"))
        {
            velo = dir.normalized * m_runningSpeed;
            return true;
            
        }
        else
        {
            velo = dir.normalized * m_movingSpeed;
            return false;
            
        }
    }
}
