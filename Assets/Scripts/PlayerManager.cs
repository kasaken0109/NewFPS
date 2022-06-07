using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerManager : MonoBehaviour,IDamage
{
    public static PlayerManager Instance { get; private set; }

    [SerializeField]
    [Tooltip("プレイヤーの体力")]
    private int m_hp = 100;

    [SerializeField]
    [Tooltip("回避時の無敵時間")]
    private float m_godTime = 0.4f;

    [SerializeField]
    [Tooltip("回避成功時の無敵時間")]
    private float m_changeTime = 2f;

    [SerializeField]
    [Tooltip("回復時に発生するエフェクト")]
    private GameObject m_healEffect;

    [SerializeField]
    [Tooltip("死亡時に発生するプレイヤーの死体")]
    private GameObject m_dead;

    [SerializeField]
    [Tooltip("プレイヤーのアニメーター")]
    private Animator m_animator = null;

    [SerializeField]
    [Tooltip("体力バー")]
    private Image hpslider = null;

    [SerializeField]
    private PostEffect postEffect = null;
    
    private int m_maxhp;
    private bool IsInvisible = false;
    public bool IsAlive = true;

    public enum StanceTypes
    {
        NORMAL,
        GOD,
    }

    public StanceTypes stanceTypes;

    private void Awake()
    {
        Instance = this;
        stanceTypes = StanceTypes.NORMAL;
        m_maxhp = m_hp;

    }

    void Update()
    {
        if (!hpslider) return;
        hpslider.fillAmount = (float)m_hp / m_maxhp;
    }

    public void AddDamage(int damage)
    {
        if (IsInvisible)
        {
            if (ActiveDodge)
            {
                StopCoroutine("GodTime");
                StartCoroutine("GodTime");
            }
            if (damage > 0) return;
        }
        if (m_hp > damage)
        {
            if(damage < 0)
            {
                Heal(damage);
            }
            else
            {
                if (TryGetComponent(out PlayerControll p) && damage > 1f)
                {
                    m_animator.Play("Damage", 0);
                    GetComponent<PlayerControll>().BasicHitAttack();
                }
                m_hp -= damage;
                if(damage > 1)SoundManager.Instance.PlayPlayerHit();
            }

            DOTween.To(
                () => hpslider.fillAmount, // getter
                x => hpslider.fillAmount = x, // setter
                (float)(float)m_hp / m_maxhp, // ターゲットとなる値
                1f  // 時間（秒）
                ).SetEase(Ease.OutCubic);
        }
        else
        {
            //死亡時に呼ばれる処理
            DOTween.To(
                () => hpslider.fillAmount, // getter
                x => hpslider.fillAmount = x, // setter
                0, // ターゲットとなる値
                1f  // 時間（秒）
                ).SetEase(Ease.OutCubic);
            var m =Instantiate(m_dead,transform.position,transform.rotation);
            GameManager.Instance.SetGameState(GameManager.GameState.PLAYERLOSE);
            gameObject.SetActive(false);
        }
    }

    private void Heal(int damage)
    {
        m_hp -= damage;
        if (m_hp >= m_maxhp) m_hp = m_maxhp;
        Instantiate(m_healEffect, transform.position, Quaternion.identity);
        SoundManager.Instance.PlayHeal();
    }

    public void SetInvisible()
    {
        StartCoroutine(nameof(Invisible));
    }

    bool ActiveDodge = false;
    IEnumerator Invisible()
    {
        SoundManager.Instance.PlayDodge();
        IsInvisible = true;
        ActiveDodge = true;
        yield return new WaitForSeconds(m_godTime);
        IsInvisible = false;
        ActiveDodge = false;
    }
    IEnumerator Invisible(float time)
    {
        IsInvisible = true;
        yield return new WaitForSeconds(time);
        IsInvisible = false;
    }
    bool IsGod = false;
    bool IsActiveCoroutine = false;
    float timer = 0;
    IEnumerator GodTime()
    {
        SoundManager.Instance.PlayFrost();
        timer = 0;
        StopCoroutine(nameof(Invisible));
        ActiveDodge = false;
        IsInvisible = true;
        while (timer < m_changeTime)
        {
            timer += 0.02f;
            yield return new WaitForSeconds(0.02f);

        }
        //postEffect.enabled = false;
        IsInvisible = false;
        IsActiveCoroutine = false;
    }
}
