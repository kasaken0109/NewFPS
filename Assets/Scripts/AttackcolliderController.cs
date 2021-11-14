using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackcolliderController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("攻撃力")]
    private int m_attackPower = 15;

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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == m_opponentTagName && CanHit)
        {
            collision.gameObject.GetComponentInParent<IDamage>().AddDamage(m_attackPower);
            CanHit = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item")) other.gameObject.GetComponentInParent<IDamage>().AddDamage(m_attackPower);
        
        if (other.tag == m_opponentTagName && CanHit)
        {
            other.gameObject.GetComponentInParent<IDamage>().AddDamage(m_attackPower);
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
