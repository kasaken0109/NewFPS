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
    [SerializeField] GameObject m_HpUI = null;
    [SerializeField] MoveState _moveState = null;
    ActionCtrl actionCtrl = null;
    int maxHp;
    int mp;
    int hitRate = 0;
    public Slider hpSlider;
 
    public void AddDamage(int damage)
    {
        if (actionCtrl.GetCurrentStateName() == "IdleState")
        {
            hpSlider.gameObject.SetActive(true);
            actionCtrl.SetCurrent(GetComponentInChildren<MoveState>());
            actionCtrl.SetCurrentName("MoveState");
        }
        mp -= (30 - damage);
        if (m_hp > damage)
        {
            m_hp -= damage;
            DOTween.To(
                () => hpSlider.value, // getter
                x => hpSlider.value = x, // setter
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
                () => hpSlider.value, // getter
                x => hpSlider.value = x, // setter
                (float)(float)0, // ターゲットとなる値
                1f  // 時間（秒）
                ).SetEase(Ease.OutCubic);
            //Debug.Log("EnemyDeath");
            Instantiate(m_deathBody,this.transform.position,this.transform.rotation);
            GameManager.Instance.GameStatus = GameManager.GameState.PLAYERWIN;
            Destroy(this.gameObject);
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

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(actionCtrl.GetCurrentStateName());
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
