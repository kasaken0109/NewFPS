﻿using System.Collections;
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
    [Tooltip("プレイヤーの値の設定")]
    private PlayerMoveSettings m_settings = default;

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
    [Tooltip("アニメーター")]
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
    [Tooltip("PostprocessのVolume")]
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

    static readonly int JumpHash = Animator.StringToHash("JumpFlag");

    static readonly int DodgeHash = Animator.StringToHash("CrouchFlag");

    static readonly int BasicAttackHash = Animator.StringToHash("Basic");

    static readonly int JumpAttackHash = Animator.StringToHash("JumpAttack");

    static readonly int JumpPowerAttackHash = Animator.StringToHash("JumpPowerAttack");

    static readonly int ComboHash = Animator.StringToHash("Combo");

    static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");

    static readonly int IsAttackEndHash = Animator.StringToHash("IsAttackEnd");
    #endregion

    Coroutine current;

    public enum MoveState
    {
        OnField,
        InAir,
        Attacking,
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

        //方向の入力を取得し、方向を求める
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        // 入力方向のベクトルを組み立てる
        dir = Vector3.forward * v + Vector3.right * h;

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
        if(m_slider) stanceValue = m_slider.fillAmount;
    }

    /// <summary>
    /// プレイヤーのY軸角度を変更させる
    /// </summary>
    private void SetPlayerAngle()
    {
        Vector3 diff = transform.position - latestPos;   //前回からどこに進んだかをベクトルで取得
        diff.y = 0;
        latestPos = transform.position;  //前回のPositionの更新

        //ベクトルの大きさが0.01以上の時に向きを変える処理をする
        if (diff.magnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, Time.deltaTime * m_settings.TurnSpeed);  // 徐々にプレイヤーを回転させる
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
            float veloY = m_rb.velocity.y;// *(Input.GetButton("Jump") ? m_settings.FloatPower : 1);//空中でジャンプキーを押すと滑空出来る
            if (Input.GetButton("Jump")) m_anim.Play("Idle");
            var midAirSpeed = Input.GetButton("Jump") ? 1 : m_settings.MidairSpeedRate;//滑空時は減速しない
            m_rb.velocity = new Vector3(m_rb.velocity.x * midAirSpeed, veloY, m_rb.velocity.z * midAirSpeed);
            if (Input.GetButton("Fire1"))
            {
                if (current != null) current = null;
                current = StartCoroutine(MidAirAttack());
                IsButtonHold = true;
            }
            else if (Input.GetButtonUp("Fire1"))
            {
                IsButtonHold = false;
            }
            //if (Input.GetButtonDown("Jump"))
            //{
            //    AirFloat();
            //}
            //else if(Input.GetButton("Jump"))
            //{
            //    AddStanceValue(m_settings.MidairConsumeRate);//浮遊状態の際にはエネルギーを徐々に減少させる
            //}
            //else
            //{
            //    m_stopFloatAction?.Invoke();
            //}
        }
    }

    /// <summary>
    /// ジャンプの挙動を制御する
    /// </summary>
    private void Jump()
    {
        if (!IsGrounded()) return;
        m_anim.SetTrigger(JumpHash);
        m_rb.useGravity = false;
        m_rb.DOMoveY(m_settings.JumpPower, 0.2f);
        m_rb.constraints = RigidbodyConstraints.FreezeRotation;
        m_rb.useGravity = true;
    }

    /// <summary>
    /// 浮遊開始時に登録された関数を呼び出す
    /// </summary>
    private void AirFloat()
    {
        m_floatAction?.Invoke();
    }

    /// <summary>
    /// 回避時の挙動を制御する
    /// </summary>
    private void Dodge()
    {
        if (CanUse)
        {
            ///クールダウンの計測を開始
            StartCoroutine(nameof(SetCoolDown), m_settings.DodgeCoolDown);
            m_anim.SetTrigger(DodgeHash);

            ///回避時の移動入力に応じて移動距離を変更
            if (dir.magnitude <= 0.01f)
            {
                m_rb.DOMove(transform.position + transform.forward * m_settings.DodgeLength, 1f);
            }
            else
            {
                m_rb.DOMove(transform.position + transform.forward * m_settings.DodgeLength * 1.2f, 1f);
            }

        }
    }

    /// <summary>
    /// スキルの発動に必要なエネルギーを追加する
    /// </summary>
    /// <param name="value">エネルギーの回収率(0~1)</param>
    public void AddStanceValue(float value)
    {
        if (value + stanceValue < 1) stanceValue += value;
        else stanceValue = 1f;
        m_slider.fillAmount = stanceValue;
    }

    [Tooltip("行動出来るかどうか")]
    bool CanUse = true;
    /// <summary>
    ///　行動のクールダウンを制御する
    /// </summary>
    /// <param name="cooldown">クールダウン時間</param>
    /// <returns></returns>
    IEnumerator SetCoolDown(float cooldown)
    {
        CanUse = false;
        yield return new WaitForSeconds(cooldown);
        CanUse = true;
    }

    float timer = 0;
    GameObject m_hit;
    /// <summary>
    /// 空中時の攻撃を制御する
    /// </summary>
    /// <returns></returns>
    IEnumerator MidAirAttack()
    {
        timer = 0;
        yield return new WaitForSeconds(0.01f);
        while(IsButtonHold)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        if (timer >= 0.5f)
        {
            m_anim.Play(JumpPowerAttackHash);
            
        }
        else if (timer != 0)
        {
            m_anim.Play(JumpAttackHash);
            m_legAttackCollider.gameObject.GetComponent<AttackcolliderController>().SetActiveAttack(true);
            yield return new WaitForSeconds(0.5f);
            m_legAttackCollider.gameObject.GetComponent<AttackcolliderController>().SetActiveAttack(false);
            timer = 0;
        }
        yield break;
    }

    /// <summary>
    /// ステップ移動処理を行う
    /// </summary>
    /// <param name="dushpower">移動距離</param>
    public void StepForward(float dushpower)
    {
        ///引数分プレイヤーの正面方向に移動
        m_rb.DOMove(transform.position + transform.forward * dushpower, 1);
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
        //if (Input.GetButtonDown("Splint")) IsRunning = !IsRunning ? true : false;
        if (Input.GetButton("Splint")) IsRunning = true;
        else IsRunning = false;

        //入力に応じてスピード、アニメーションを変更する
        velo = dir.normalized * (IsRunning ? m_settings.RunningSpeed : m_settings.MovingSpeed) * speedRate;
        m_anim.SetFloat(SpeedHash, (IsRunning ? m_settings.RunningSpeed : m_settings.MovingSpeed) * speedRate);
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
    /// <summary>
    /// 入力に応じてコンボ攻撃を変化させる
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitInput()
    {
        float timer = 0;
        var wait = new WaitForSeconds(0.01f);//事前キャッシュ
        m_comboEffect?.SetActive(true);
        while(timer < 0.3f)
        {
            yield return wait;
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
        IsSucceeded = false;
        m_comboEffect?.SetActive(false);
        yield return new WaitForSeconds(2f);
        m_successEffect?.SetActive(false);
    }

    private float speedRate = 1;
    IEnumerator SpeedUp(float time,float value)
    {
        speedRate = value;
        yield return new WaitForSeconds(time);
        speedRate = 1;
    }

    public void SpeedStart(float time, float value)
    {
        StartCoroutine(SpeedUp(time,value));
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
