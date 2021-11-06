using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyBossManager : MonoBehaviour, IDamage
{
    public static EnemyBossManager Instance { get; private set; }
    [SerializeField] int m_hp = 100;
    [SerializeField] int m_mp = 200;
    [SerializeField, Range(1, 100)] int m_rate;
    [SerializeField] int m_attackPower = 10;
    [SerializeField] float m_freezeTime = 5f;
    [SerializeField] Animator m_animator = null;
    [SerializeField] GameObject m_deathBody = null;
    public GameObject m_froznBody = null;
    [SerializeField] MoveState _moveState = null;
    ActionCtrl actionCtrl = null;
    int maxHp;
    int mp;
    int hitRate = 0;
    public Image hpSlider;
    int rateTemp;
    public bool IsCritical = false;


    float hitSpeed = 1f;
    public void AddDamage(int damage)
    {
        if (actionCtrl.GetCurrentStateName() == "IdleState")
        {
            hpSlider.gameObject.SetActive(true);
            actionCtrl.SetCurrent(GetComponentInChildren<MoveState>());
            actionCtrl.SetCurrentName("MoveState");
        }
        mp -= (30 - damage);
        hitSpeed = (float)(Mathf.Abs(10 - damage) / 20f);
        StopCoroutine(HitStop());
        StartCoroutine(HitStop());
        if (m_hp > damage)
        {
            m_hp -= damage;
            DOTween.To(
                () => hpSlider.fillAmount, // getter
                x => hpSlider.fillAmount = x, // setter
                (float)(float)m_hp / maxHp, // ターゲットとなる値
                1f  // 時間（秒）
                ).SetEase(Ease.OutCubic);
            hitRate += damage;
            if(hitRate >= m_rate)
            {
                m_animator.SetInteger("HP", 1);
                m_animator.SetTrigger("Hit");
                hitRate = 0;
            }
            //Debug.Log($"Hit!:{mp}");
        }
        else
        {
            m_hp = 0;
            DOTween.To(
                () => hpSlider.fillAmount, // getter
                x => hpSlider.fillAmount = x, // setter
                (float)(float)0, // ターゲットとなる値
                1f  // 時間（秒）
                ).SetEase(Ease.OutCubic);
            //Debug.Log("EnemyDeath");
            Instantiate(m_deathBody,this.transform.position,this.transform.rotation);
            GameManager.Instance.GameStatus = GameManager.GameState.PLAYERWIN;
            Destroy(this.gameObject);
        }
    }

    IEnumerator HitStop()
    {
        var source = GetComponent<Cinemachine.CinemachineImpulseSource>();
        source.GenerateImpulse();
        Time.timeScale = 0f;
        yield return new WaitForSeconds(0.3f / hitSpeed);
        float timer = 0;
        while (Time.timeScale < 0.99f)
        {
            Time.timeScale += 0.2f;
            yield return null;
        } 
    }

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        actionCtrl = new ActionCtrl();
        maxHp = m_hp;
        actionCtrl.SetCurrent(GetComponentInChildren<IdleState>());
        actionCtrl.SetCurrentName("IdleState");
        mp = m_mp;
        StartCoroutine(nameof(FrostMode));
    }

    int count = 0;
    // Update is called once per frame
    void Update()
    {
        if (m_hp < maxHp * 0.5f && count == 0)
        {
            StartCoroutine(nameof(DeathCombo));
        }
        else if (m_hp < maxHp * 0.2f && count == 1)
        {
            StopCoroutine(nameof(DeathCombo));
            StartCoroutine(nameof(DeathCombo));
        }
    }

    IEnumerator DeathCombo()
    {
        IsCritical = true;
        count++;
        rateTemp = hitRate;
        hitRate = -500;
        yield return new WaitForSeconds(5f);
        float distance = Vector3.Distance(GameManager.Player.transform.position, gameObject.transform.position);
        
        int type = distance >= 7 ? 5 : 4;
        m_animator.SetTrigger("DeathAttack");
        m_animator.SetInteger("AttackType", type);
        yield return new WaitForSeconds(5f);
        IsCritical = false;
        Debug.Log("Warnimg");
        hitRate = rateTemp;
    }

    IEnumerator FrostMode()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if(mp <= 0)
            {
                m_froznBody.SetActive(true);
                m_animator.SetBool("IsFreeze",true);
                yield return new WaitForSeconds(m_freezeTime);
                m_animator.SetBool("IsFreeze", false);
                m_froznBody.SetActive(false);
                mp = m_mp;
            }
        }
    }

    public void SpawnEffects()
    {
        _moveState.SpawnEffect();
    }

    public void StopPlayer()
    {
        GameManager.Instance.CinemaMode();
    }


}
