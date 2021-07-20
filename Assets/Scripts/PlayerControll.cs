using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

//プレイヤーの動きを制御する
public class PlayerControll : ColliderGenerater
{
    // Start is called before the first frame update
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
    /// <summary>攻撃の当たり判定</summary>
    [SerializeField] GameObject m_attackCollider = null;
    /// <summary>プレイヤーオブジェクト</summary>
    [SerializeField] GameObject m_player = null;
    /// <summary>スピードアップエフェクト</summary>
    [SerializeField] GameObject m_speedup = null;
    [SerializeField] Animator m_anim = null;
    /// <summary>しゃがみ時の減速割合</summary>
    [SerializeField] float m_crouchSlow = 1;
    /// <summary>スキルクールダウンタイム</summary>
    [SerializeField] float m_skillWaitTime = 1;
    Rigidbody m_rb;
    Vector3 dir;
    Vector3 velo;
    CapsuleCollider collider;

    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        m_anim = GetComponent<Animator>();
        collider = GetComponent<CapsuleCollider>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        //方向の入力を取得し、方向を求める
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        // 入力方向のベクトルを組み立てる
        dir = Vector3.forward * v + Vector3.right * h;
        if (dir == Vector3.zero)
        {
            // 方向の入力がニュートラルの時は、y 軸方向の速度を保持するだけ
            m_rb.velocity = new Vector3(0f, m_rb.velocity.y, 0f);


        }
        else
        {
            dir = Camera.main.transform.TransformDirection(dir);    // メインカメラを基準に入力方向のベクトルを変換する
            dir.y = 0;  // y 軸方向はゼロにして水平方向のベクトルにする
            Running(); // 入力した方向に移動する
            velo.y = m_rb.velocity.y;   // ジャンプした時の y 軸方向の速度を保持する
            m_rb.velocity = velo;   // 計算した速度ベクトルをセットする
        }

        if (IsGrounded())
        {
            if (Input.GetButtonDown("Jump"))
            {
                m_anim.SetTrigger("JumpFlag");
                m_rb.useGravity = false;
                m_rb.AddForce(Vector3.up * m_jumpPower, ForceMode.Impulse);
                m_rb.constraints = RigidbodyConstraints.FreezeRotation;
                m_rb.useGravity = true;
            }
            if (Input.GetButton("Crouch"))
            {
                m_anim.SetTrigger("CrouchFlag");
                m_crouchSlow = 0.5f;
                collider.height = 0.7f;
            }
            else if (Input.GetButtonUp("Crouch"))
            {
                m_crouchSlow = 1f;
                collider.height = 1f;
            }
            if (v == 0 && h == 0)
            {
                m_anim.SetFloat("Speed", 0);
                m_speedup.SetActive(false);

            }
            else
            {
                Running();
            }
        }
        else
        {
            m_anim.SetFloat("Speed", 0);
        }


        if (Input.GetButton("Fire1"))
        {
            m_anim.SetTrigger("ShootFlag");
        }
        if (Input.GetButtonDown("Fire2"))
        {
            m_anim.SetTrigger("PunchFlag");
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

    /// <summary>
    /// 移動状態を制御する
    /// </summary>
    void Running()
    {
        if (Input.GetButton("Splint"))
        {
            velo = dir.normalized * m_runningSpeed * m_crouchSlow;
            m_anim.SetFloat("Speed", m_runningSpeed);
            m_speedup.SetActive(true);
            Camera.main.fieldOfView = 80;

        }
        else
        {
            velo = dir.normalized * m_movingSpeed * m_crouchSlow;
            m_anim.SetFloat("Speed", m_movingSpeed);
            m_speedup.SetActive(false);
            Camera.main.fieldOfView = 60;
        }
    }

    public void GenerateCollider()
    {
        StartCoroutine(ColliderGenerater.Instance.GenerateCollider(m_attackCollider, m_skillWaitTime));
    }
}
