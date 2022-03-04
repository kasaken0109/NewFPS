using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


[RequireComponent(typeof(Rigidbody))]

//プレイヤーの動きを制御する
public class PlayerControll : ColliderGenerater
{
    public static PlayerControll Instance { get; private set; }

    [SerializeField]
    [Tooltip("動く速さ")]
    private float m_movingSpeed = 5f;

    [SerializeField]
    [Tooltip("走る速さ")]
    private float m_runningSpeed = 8f;

    [SerializeField]
    [Tooltip("ターンの速さ")]
    private float m_turnSpeed = 3f;

    [SerializeField]
    [Tooltip("ジャンプ力")]
    private float m_jumpPower = 5f;

    [SerializeField]
    [Tooltip("滞空時の水平移動速度の軽減率")]
    private float m_midairSpeedRate = 0.7f;

    [SerializeField]
    [Tooltip("突進力")]
    private float m_dushPower = 10f;

    [SerializeField]
    [Tooltip("突進攻撃力")]
     private int m_dushAttackPower = 15;

    /// <summary>回避距離</summary>
    [SerializeField]
    private float m_dodgeLength = 5;

    [SerializeField]
    [Tooltip("接地判定の際、中心 (Pivot) からどれくらいの距離を「接地している」と判定するかの長さ")]
    float m_isGroundedLength = 1.1f;
    
    [SerializeField]
    [Tooltip("攻撃の当たり判定")]
    GameObject m_attackCollider = null;

    [SerializeField]
    [Tooltip("キック攻撃の当たり判定")]
    GameObject m_legAttackCollider = null;

    [SerializeField]
    [Tooltip("コンボ攻撃判定")]
    GameObject m_comboEffect = null;

    [SerializeField]
    [Tooltip("コンボ攻撃成功エフェクト")]
    GameObject m_successEffect = null;

    [SerializeField]
    [Tooltip("プレイヤーオブジェクト")]
    GameObject m_player = null;

    [SerializeField]
    [Tooltip("スピードアップエフェクト")]
    GameObject m_speedup = null;

    [SerializeField]
    [Tooltip("ラッシュエフェクト")]
    GameObject m_rush = null;

    [SerializeField]
    Animator m_anim = null;

    [SerializeField]
    [Tooltip("スキルクールダウンタイム")]
    private float m_skillWaitTime = 1;

    [SerializeField]
    private float m_powerUpRate = 3f;

    [SerializeField]
    [Tooltip("当たるレイヤー")]
    private LayerMask m_layerMask = 0;

    [SerializeField]
    private Volume m_Volume;

    private bool IsButtonHold = false;
    private Rigidbody m_rb;
    private Vector3 dir;
    private Vector3 velo;
    private Vector3 latestPos;
    /// <summary>照準</summary>
    RectTransform m_crosshairUi = null;
    Ray ray;
    RaycastHit hit;
    LensDistortion distortion;

    private　bool m_isMoveActive = true;
    public void SetMoveActive(bool IsMoveActive) { m_isMoveActive = IsMoveActive; }

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        m_anim = GetComponent<Animator>();
        m_crosshairUi = GameObject.Find("Targetaim").GetComponent<RectTransform>();

        DisplayEffectInit();
    }

    private void DisplayEffectInit()
    {
        VolumeProfile profile = m_Volume.sharedProfile;
        foreach (var item in profile.components)
        {
            if (item as LensDistortion)
            {
                distortion = item as LensDistortion;
            }
        }
    }

    void Update()
    {
        //移動不可の際に
        if (!m_isMoveActive) return;

        m_powerUpRate = PlayerManager.Instance.stanceTypes == PlayerManager.StanceTypes.GOD ?
            2f : 1f;
        //方向の入力を取得し、方向を求める
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        // 入力方向のベクトルを組み立てる
        dir = Vector3.forward * v + Vector3.right * h;
        ray = Camera.main.ScreenPointToRay(m_crosshairUi.position);

        if (dir == Vector3.zero)
        {
            // 方向の入力がニュートラルの時は、y 軸方向の速度を保持するだけ
            m_rb.velocity = new Vector3(0f, m_rb.velocity.y, 0f);
            m_anim.SetFloat("Speed", 0);
        }
        else
        {
            MovePos();
            SetPlayerAngle();
        }

        MoveAction();
        m_speedup.SetActive(m_rb.velocity.sqrMagnitude >= 100);


    }

    private void SetPlayerAngle()
    {
        Vector3 diff = transform.position - latestPos;   //前回からどこに進んだかをベクトルで取得
        diff.y = 0;
        latestPos = transform.position;  //前回のPositionの更新

        //ベクトルの大きさが0.01以上の時に向きを変える処理をする
        if (diff.magnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(diff); //向きを変更する
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, Time.deltaTime * m_turnSpeed);  // Slerp を使うのがポイント
        }
    }

    private void MovePos()
    {
        dir = Camera.main.transform.TransformDirection(dir);    // メインカメラを基準に入力方向のベクトルを変換する
        dir.y = 0;  // y 軸方向はゼロにして水平方向のベクトルにする
        Running(); // 入力した方向に移動する
        velo.y = m_rb.velocity.y;   // ジャンプした時の y 軸方向の速度を保持する
        m_rb.velocity = velo;   // 計算した速度ベクトルをセットする
    }

    private void MoveAction()
    {
        //地上での入力
        if (IsGrounded())
        {
            if (Input.GetButtonDown("Fire1"))
            {
                //Attack
                m_anim.Play("Basic");
            }
            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }
            if (Input.GetButtonDown("Crouch"))
            {
                Dodge();
            }
        }
        //空中での入力
        else
        {
            m_anim.SetFloat("Speed", 0);
            float veloY = m_rb.velocity.y;
            m_rb.velocity = new Vector3(m_rb.velocity.x * m_midairSpeedRate, veloY, m_rb.velocity.z * m_midairSpeedRate);
            if (Input.GetButton("Fire1"))
            {
                StartCoroutine(MidAirAttack());
                IsButtonHold = true;
            }
            else if (Input.GetButtonUp("Fire1"))
            {
                IsButtonHold = false;
            }
        }
    }

    private void Jump()
    {
        if (!IsGrounded()) return;
        m_anim.SetTrigger("JumpFlag");
        m_rb.useGravity = false;
        m_rb.DOMoveY(5, 0.5f);
        m_rb.constraints = RigidbodyConstraints.FreezeRotation;
        m_rb.useGravity = true;
    }

    private void Dodge()
    {
        m_anim.SetTrigger("CrouchFlag");
        m_rb.DOMove(transform.position + transform.forward * m_dodgeLength, 0.5f);
    }

    float timer = 0;
    GameObject m_hit;
    IEnumerator MidAirAttack()
    {
        yield return new WaitForSeconds(0.1f);
        while(IsButtonHold)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        if (timer >= 0.3f)
        {
            m_anim.Play("JumpPowerAttack");
        }
        else if (timer != 0)
        {
            m_anim.Play("JumpAttack");
            m_legAttackCollider.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            m_legAttackCollider.SetActive(false);
        }
        timer = 0;
        yield break;
    }

    public void StepForward(float dushpower)
    {
        m_rb.DOMove(transform.position + transform.forward * dushpower, 1);
        bool Ishit = Physics.Raycast(ray, out hit, 15f, m_layerMask);

        if (Ishit)
        {
            m_hit = hit.collider.gameObject;
            m_hit.GetComponentInParent<IDamage>().AddDamage(20);
        }
        //移動エフェクトを有効にする
        m_rush.SetActive(true);
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
        //入力に応じてスピード、アニメーションを変更する
        velo = dir.normalized * (Input.GetButton("Splint") ? m_runningSpeed : m_movingSpeed) * m_powerUpRate;
        m_anim.SetFloat("Speed", (Input.GetButton("Splint") ? m_runningSpeed : m_movingSpeed) * m_powerUpRate);
    }


    /// <summary>
    /// 攻撃を受けたときのノックバック関数
    /// </summary>
    public void BasicHitAttack() => m_rb.DOMove(this.transform.position - this.transform.forward * 2, 1f);

    /// <summary>
    /// 基本攻撃の当たり判定の有効化
    /// </summary>
    public void BasicWeaponAttack() => GetComponentInChildren<Sword>()?.BasicAttack();

    /// <summary>
    /// 特別攻撃の当たり判定の有効化
    /// </summary>
    public void SpecialWeaponAttack() => GetComponentInChildren<Sword>()?.SpecialAttack();

    //地上で行われるコンボ攻撃
    public void Combo()
    {
        //入力コルーチンを開始
        StopCoroutine(nameof(WaitInput));
        StartCoroutine(nameof(WaitInput));
    }

    bool IsSucceeded = false;
    IEnumerator WaitInput()
    {
        float timer = 0;
        m_comboEffect?.SetActive(true);
        while(timer < 0.3f)
        {
            yield return new WaitForSeconds(0.01f);
            timer += 0.01f;
            if (Input.GetButtonDown("Fire1"))
            {
                m_anim.SetTrigger("Combo");
                IsSucceeded = true;
                m_successEffect?.SetActive(true);
                m_comboEffect?.SetActive(false);
                break;
            }
        }
        m_anim.speed = IsSucceeded ? m_anim.speed*= 1.1f : 1;//コンボ成功時に攻撃速度を上昇させる
        IsSucceeded = false;
        m_comboEffect?.SetActive(false);
        yield return new WaitForSeconds(2f);
        m_successEffect?.SetActive(false);
    }

    /// <summary>
    /// 剣の軌跡を有効化する
    /// </summary>
    public void StartEmit() => GetComponentInChildren<Sword>()?.StartEmitting();

    /// <summary>
    /// 剣の軌跡を無効化する
    /// </summary>
    public void StopEmit() => GetComponentInChildren<Sword>()?.StopEmitting();

    /// <summary>
    /// 重力有効化
    /// </summary>
    public void StopFloat() => m_rb.useGravity = true;

    /// <summary>
    /// 重力無効化
    /// </summary>
    public void StartFloat() => GetComponentInChildren<Sword>()?.FloatUp();

    public void PlayDodgeSE() => SoundManager.Instance.PlayDodge();

    /// <summary>
    /// 回避時に発生する画面エフェクトを有効にする
    /// </summary>
    public void SetBlur() => distortion.active = true;

    /// <summary>
    /// 回避時に発生する画面エフェクトを無効にする
    /// </summary>
    public void RemoveBlur() => distortion.active = false;
}
