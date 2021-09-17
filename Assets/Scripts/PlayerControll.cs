using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]

//プレイヤーの動きを制御する
public class PlayerControll : ColliderGenerater
{
    public static PlayerControll Instance { get; private set; }
    // Start is called before the first frame update
    /// <summary>動く速さ</summary>
    [SerializeField] float m_movingSpeed = 5f;
    /// <summary> 走る速さ</summary>
    [SerializeField] float m_runningSpeed = 8f;
    /// <summary>ターンの速さ</summary>
    [SerializeField] float m_turnSpeed = 3f;
    /// <summary>ジャンプ力</summary>
    [SerializeField] float m_jumpPower = 5f;
    /// <summary>突進力</summary>
    [SerializeField] float m_dushPower = 10f;
    /// <summary>突進攻撃力</summary>
    [SerializeField] int m_dushAttackPower = 15;
    /// <summary>接地判定の際、中心 (Pivot) からどれくらいの距離を「接地している」と判定するかの長さ</summary>
    [SerializeField] float m_isGroundedLength = 1.1f;
    /// <summary>照準</summary>
    RectTransform m_crosshairUi = null;
    /// <summary>攻撃の当たり判定</summary>
    [SerializeField] GameObject m_attackCollider = null;
    /// <summary>ラッシュ攻撃の当たり判定</summary>
    [SerializeField] GameObject m_rushAttackCollider = null;
    /// <summary>プレイヤーオブジェクト</summary>
    [SerializeField] GameObject m_player = null;
    /// <summary>スピードアップエフェクト</summary>
    [SerializeField] GameObject m_speedup = null;
    /// <summary>ラッシュエフェクト</summary>
    [SerializeField] GameObject m_rush = null;
    [SerializeField] Animator m_anim = null;
    /// <summary>しゃがみ時の減速割合</summary>
    [SerializeField] float m_crouchSlow = 1;
    /// <summary>スキルクールダウンタイム</summary>
    [SerializeField] float m_skillWaitTime = 1;
    /// <summary>当たるレイヤー</summary>
    [SerializeField] LayerMask m_layerMask = 0;
    bool IsButtonHold = false;
    Rigidbody m_rb;
    Vector3 dir;
    Vector3 velo;
    CapsuleCollider collider;
    Ray ray;
    RaycastHit hit;
    WeaponManager weaponManager;
    bool m_isMoveActive = true;
    float powerUpRate = 3;
    public void SetMoveActive(bool IsMoveActive) { m_isMoveActive = IsMoveActive; }

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        m_anim = GetComponent<Animator>();
        weaponManager = GetComponent<WeaponManager>();
        collider = GetComponent<CapsuleCollider>();
        m_crosshairUi = GameObject.Find("Targetaim").GetComponent<RectTransform>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (!m_isMoveActive) return;
        //方向の入力を取得し、方向を求める
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        if (PlayerManager.Instance.stanceTypes == PlayerManager.StanceTypes.GOD) powerUpRate = 2f;
        else
        {
            powerUpRate = 1f;
        }

        // 入力方向のベクトルを組み立てる
        dir = Vector3.forward * v + Vector3.right * h;
        ray = Camera.main.ScreenPointToRay(m_crosshairUi.position);

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
            if (Input.GetButtonDown("Fire1"))
            {
                if (weaponManager.NowWeapon.name.Contains("Sword"))
                {
                    m_anim.Play("Basic");
                }
            }
            if (Input.GetButtonDown("Jump"))
            {
                m_anim.SetTrigger("JumpFlag");
                m_rb.useGravity = false;
                //m_rb.AddForce(Vector3.up * m_jumpPower * powerUpRate, ForceMode.Impulse);
                m_rb.DOMoveY(5, 0.5f);
                m_rb.constraints = RigidbodyConstraints.FreezeRotation;
                m_rb.useGravity = true;
            }
            if (Input.GetButtonDown("Crouch"))
            {
                m_anim.SetTrigger("CrouchFlag");
                //m_crouchSlow = 0.5f;
                //collider.height = 0.7f;
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
            if (Input.GetButtonDown("Fire1") && weaponManager.NowWeapon.name.Contains("Sword"))
            {
                m_anim.Play("JumpAttack");
            }
        }


        if (Input.GetButton("Fire1"))
        {
            if (!weaponManager.NowWeapon.name.Contains("Sword"))
            {
                m_anim.SetTrigger("ShootFlag");
            }
        }
        
        if (Input.GetButton("Fire2"))
        {
            m_rush.SetActive(false);
            StartCoroutine(HoldAttack());
            IsButtonHold = true;
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            IsButtonHold = false;
            StopCoroutine(HoldAttack());
            //m_anim.SetBool("PunchBool", false);
        }
    }

    float timer = 0;
    GameObject m_hit;
    IEnumerator HoldAttack()
    {
        bool IsFirstAttack = true;
        yield return new WaitForSeconds(0.1f);
        if (IsButtonHold)
        {
            timer += 0.1f;
        }
        else
        {
            if (timer >= 2)
            {
                Debug.Log("HoldAttack");
                //StartCoroutine(ColliderGenerater.Instance.GenerateCollider(m_rushAttackCollider, m_skillWaitTime / 5));
                m_anim.SetTrigger("FlipTrigger");
                m_rb.DOMove(transform.position + Camera.main.transform.forward* 10, 1);
                //m_rb.AddForce(this.gameObject.transform.forward * m_dushPower,ForceMode.Impulse);
                bool Ishit = Physics.Raycast(ray, out hit, 15f, m_layerMask);
                //Debug.Log(Ishit);
                if (Ishit)
                {
                    Debug.Log("Hit");
                    m_hit = hit.collider.gameObject;
                    m_hit.GetComponentInParent<IDamage>().AddDamage(20);
                }
                m_rush.SetActive(true);
            }
            else if(timer != 0 && IsFirstAttack)
            {
                m_anim.SetTrigger("PunchFlag");
                yield return new WaitForSeconds(0.1f);
                m_attackCollider.SetActive(true);
                yield return new WaitForSeconds(0.5f);
                m_attackCollider.SetActive(false);
                IsFirstAttack = false;
            }
            timer = 0;
            yield break;
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
            velo = dir.normalized * m_runningSpeed * m_crouchSlow * powerUpRate;
            m_anim.SetFloat("Speed", m_runningSpeed * powerUpRate);
            m_speedup.SetActive(true);
            Camera.main.fieldOfView = 100;

        }
        else
        {
            velo = dir.normalized * m_movingSpeed * m_crouchSlow * powerUpRate;
            m_anim.SetFloat("Speed", m_movingSpeed * powerUpRate);
            m_speedup.SetActive(false);
            Camera.main.fieldOfView = 80;
        }
    }

    public void GenerateCollider()
    {
        //StartCoroutine(ColliderGenerater.Instance.GenerateCollider(m_attackCollider, m_skillWaitTime));
    }

    public void BasicHitAttack()
    {
        m_rb.DOMove(this.transform.position - this.transform.forward * 2,1f);
    }

    public void JumpAttackMove()
    {
        //m_rb.AddForce(Vector3.down * 10, ForceMode.Impulse);
    }

    public void BasicWeaponAttack()
    {
        weaponManager.NowWeapon.GetComponent<IWeapon>().BasicAttack();
    }

    public void SpecialWeaponAttack()
    {
        weaponManager.NowWeapon.GetComponent<IWeapon>().SpecialAttack();
    }

    public void StartEmit()
    {
        weaponManager.NowWeapon.GetComponent<Sword>().StartEmitting();
    }

    public void StopEmit()
    {
        weaponManager.NowWeapon.GetComponent<Sword>().StopEmitting();
    }
    public void StopFloat()
    {
        m_rb.useGravity = true;
    }

    public void StartFloat()
    {
        weaponManager.NowWeapon.GetComponent<Sword>().FloatUp();
    }
    public void LargeHitAttack()
    {

    }
}
