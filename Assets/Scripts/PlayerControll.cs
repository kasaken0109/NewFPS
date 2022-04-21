using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Rigidbody))]

//プレイヤーの動きを制御する
public class PlayerControll : ColliderGenerater
{
    public static PlayerControll Instance { get; private set; }

    [SerializeField]
    [Tooltip("プレイヤーの値の設定、0,１,２要設定")]
    private PlayerMoveSettings[] m_settings = default;

    [SerializeField]
    [Tooltip("接地判定の際、中心 (Pivot) からどれくらいの距離を「接地している」と判定するかの長さ")]
    private float m_isGroundedLength = 1.1f;

    [SerializeField]
    [Tooltip("攻撃の当たり判定")]
     private GameObject m_attackCollider = null;

    [SerializeField]
    [Tooltip("キック攻撃の当たり判定")]
    private GameObject m_legAttackCollider = null;

    [SerializeField]
    [Tooltip("コンボ攻撃判定")]
    private GameObject m_comboEffect = null;

    [SerializeField]
    [Tooltip("コンボ攻撃成功エフェクト")]
    private GameObject m_successEffect = null;

    [SerializeField]
    [Tooltip("スピードアップエフェクト")]
    private GameObject m_speedup = null;

    [SerializeField]
    [Tooltip("ラッシュエフェクト")]
    GameObject m_rush = null;

    [SerializeField]
    Animator m_anim = null;

    [SerializeField]
    private float m_powerUpRate = 3f;

    [SerializeField]
    [Tooltip("当たるレイヤー")]
    private LayerMask m_layerMask = 0;

    [SerializeField]
    [Tooltip("スタンス値のSlider")]
    private Image m_slider = default;

    [SerializeField]
    [Tooltip("滑空時の処理")]
    private UnityEvent m_floatAction;

    [SerializeField]
    [Tooltip("滑空終了時の処理")]
    private UnityEvent m_stopFloatAction;

    [SerializeField]
    private Volume m_Volume;

    private bool IsButtonHold = false;
    private float stanceValue;
    private Rigidbody m_rb;
    private Vector3 dir;
    private Vector3 velo;
    private Vector3 latestPos;
    private PlayerMoveSettings m_current;
    /// <summary>照準</summary>
    RectTransform m_crosshairUi = null;
    Ray ray;
    RaycastHit hit;
    LensDistortion distortion;

    private　bool m_isMoveActive = true;

    public float StanceValue => stanceValue;
    public void SetMoveActive(bool IsMoveActive) { m_isMoveActive = IsMoveActive; }

    #region AnimationHash

    static readonly int SpeedHash = Animator.StringToHash("Speed");

    static readonly int JumpHash = Animator.StringToHash("JumpTrigger");

    static readonly int DodgeHash = Animator.StringToHash("CrouchFlag");

    static readonly int BasicAttackHash = Animator.StringToHash("Basic");

    static readonly int JumpAttackHash = Animator.StringToHash("JumpAttack");

    static readonly int JumpPowerAttackHash = Animator.StringToHash("JumpPowerAttack");

    static readonly int ComboHash = Animator.StringToHash("Combo");

    static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
    #endregion

    public enum MoveState
    {
        OnField,
        InAir,
        Stun,
    }

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        stanceValue = 0.5f;
        m_rb = GetComponent<Rigidbody>();
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
        SetStance();
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
            m_anim.SetFloat(SpeedHash, 0);
        }
        else
        {
            MovePos();
            SetPlayerAngle();
        }

        MoveAction();
        m_speedup.SetActive(m_rb.velocity.sqrMagnitude >= 100);

        SetStance();
    }

    private void SetStance()
    {
        stanceValue = m_slider.fillAmount;
        m_current = m_settings[1];
        m_anim.runtimeAnimatorController = m_current.Anim;
    }

    private void SetPlayerAngle()
    {
        Vector3 diff = transform.position - latestPos;   //前回からどこに進んだかをベクトルで取得
        diff.y = 0;
        latestPos = transform.position;  //前回のPositionの更新

        //ベクトルの大きさが0.01以上の時に向きを変える処理をする
        if (diff.magnitude > 0.01f)
        {
            //transform.rotation = Quaternion.LookRotation(diff); //向きを変更する
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, Time.deltaTime * m_current.TurnSpeed);  // Slerp を使うのがポイント
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
                m_anim.Play(BasicAttackHash);
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
            m_anim.SetFloat(SpeedHash, 0);
            float veloY = m_rb.velocity.y *(Input.GetButton("Jump") ? m_current.FloatPower : 1);//空中でジャンプキーを押すと滑空出来る
            if (Input.GetButton("Jump")) m_anim.Play("Idle");
            var midAirSpeed = Input.GetButton("Jump") ? 1 : m_current.MidairSpeedRate;//滑空時は減速しない
            m_rb.velocity = new Vector3(m_rb.velocity.x * midAirSpeed, veloY, m_rb.velocity.z * midAirSpeed);
            if (Input.GetButton("Fire1"))
            {
                StartCoroutine(MidAirAttack());
                IsButtonHold = true;
            }
            else if (Input.GetButtonUp("Fire1"))
            {
                IsButtonHold = false;
            }
            if (Input.GetButtonDown("Jump"))
            {
                AirFloat();
            }
            else if(Input.GetButton("Jump"))
            {
                AddStanceValue(m_current.MidairConsumeRate);
            }
            else
            {
                m_stopFloatAction?.Invoke();
            }
        }
    }

    private void Jump()
    {
        if (!IsGrounded()) return;
        m_anim.SetTrigger(JumpHash);
        m_rb.useGravity = false;
        m_rb.DOMoveY(5, 0.5f);
        m_rb.constraints = RigidbodyConstraints.FreezeRotation;
        m_rb.useGravity = true;
    }

    private void AirFloat()
    {
        m_floatAction?.Invoke();
    }

    private void Dodge()
    {
        if (CanUse)
        {
            StartCoroutine(nameof(SetCoolDown), m_current.DodgeCoolDown);
            m_anim.SetTrigger(DodgeHash);
            if (dir.magnitude <= 0.01f)
            {
                m_rb.DOMove(transform.position + transform.forward * m_current.DodgeLength, 1f);
            }
            else
            {
                m_rb.DOMove(transform.position + dir.normalized * m_current.DodgeLength, 1f);
            }

        }
    }

    public void AddStanceValue(float value)
    {
        if (value + stanceValue < 1) stanceValue += value;
        else stanceValue = 1f;
        m_slider.fillAmount = stanceValue;
    }


    bool CanUse = true;
    IEnumerator SetCoolDown(float cooldown)
    {
        CanUse = false;
        yield return new WaitForSeconds(cooldown);
        CanUse = true;
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
            m_anim.Play(JumpPowerAttackHash);
        }
        else if (timer != 0)
        {
            m_anim.Play(JumpAttackHash);
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
        m_anim.SetBool(IsGroundedHash, isGrounded);
        return isGrounded;
    }

     bool IsRunning = false;
    /// <summary>
    /// 移動状態を制御する
    /// </summary>
    void Running()
    {
        if (Input.GetButtonDown("Splint")) IsRunning = !IsRunning ? true : false;
        //入力に応じてスピード、アニメーションを変更する
        velo = dir.normalized * (IsRunning ? m_current.RunningSpeed : m_current.MovingSpeed) * m_powerUpRate;
        m_anim.SetFloat(SpeedHash, (IsRunning ? m_current.RunningSpeed : m_current.MovingSpeed) * m_powerUpRate);
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
                m_anim.SetTrigger(ComboHash);
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
