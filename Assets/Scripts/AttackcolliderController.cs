using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 接触判定(攻撃)を制御する
/// </summary>
public class AttackcolliderController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("攻撃力")]
    private int m_attackPower = 15;

    [SerializeField]
    [Tooltip("スタンス値の上昇値")]
    [Range(-1, 1)]
    private float m_upStanceValue = 0.1f;

    [SerializeField]
    [Tooltip("衝突時に発生するエフェクト")]
    private GameObject m_hitEffect = null;

    [SerializeField]
    [Tooltip("ヒットサウンド")]
    private AudioClip m_hit;

    [SerializeField]
    [Tooltip("攻撃対象のタグ名")]
    private string m_opponentTagName = "Player";

    [Tooltip("攻撃が有効かどうか")]
    private bool CanHit;

    private void OnEnable()
    {
        //コライダーがアクティブになったときに攻撃を有効にする
        CanHit = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item")) other.gameObject.GetComponentInParent<IDamage>().AddDamage(m_attackPower);
        
        if (other.tag == m_opponentTagName && CanHit)
        {
            var frostAttack = other.GetComponentInChildren<FrostAttackController>();
            if (frostAttack)
            {
                other.gameObject.GetComponentInParent<IDamage>().AddDamage(frostAttack.Damage);
                Destroy(frostAttack.gameObject);
            }

            var stance = GetComponentInParent<PlayerControll>();
            if (stance)
            {
                stance.AddStanceValue(m_upStanceValue);
                other.gameObject.GetComponentInParent<IDamage>().AddDamage(Mathf.CeilToInt(m_attackPower * (stance.StanceValue >= 0.3f ? (stance.StanceValue >= 0.7 ? 1.5f : 1f) : 0.7f)));
            }
            else
            {
                var p = other.GetComponentInParent<PlayerControll>();
                other.gameObject.GetComponentInParent<IDamage>().AddDamage(Mathf.CeilToInt(m_attackPower * (p.StanceValue >= 0.3f ? (p.StanceValue >= 0.7 ? 0.7f : 1f) : 1.3f)));
            }
            
            if(m_hit)SoundManager.Instance.PlayHit(m_hit,gameObject.transform.position);
            if(m_hitEffect) Instantiate(m_hitEffect, other.ClosestPoint(transform.position), GameManager.Player.transform.rotation);
            CanHit = false;
        }
    }

    /// <summary>
    /// 追加ダメージを返す
    /// </summary>
    /// <param name="addDamage"></param>
    /// <returns></returns>
    public int AddDamageCount(int addDamage) { return m_attackPower + addDamage; }
}
