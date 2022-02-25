using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

/// <summary>
/// 敵の挙動を定義する
/// </summary>
public class EnemyContoroller : MonoBehaviour
{
    [SerializeField]
    private GameObject attackCollider;

    [SerializeField]
    private GameObject m_breath;

    [SerializeField]
    private GameObject m_effect;
    
    [SerializeField]
    private GameObject m_finalBreath;
    
    [SerializeField]
    private Transform m_spwanBreath;
    
    [SerializeField]
    private Transform m_spwanEffect;

    [SerializeField]
    private float m_hitTime = 1f;

    Rigidbody m_rb;
    NavMeshAgent agent;

    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
    }

    public void BasicAttack()
    {
        Debug.Log("Basic");
        //this.transform.DOMove(this.transform.position + this.transform.forward * 2, 0.2f);
        //this.transform.DOMove(gameObject.transform.position + gameObject.transform.forward * 2, 1f);
        //m_rb.velocity = this.transform.forward * 100;
    }

    public void BasicAttackEffect()
    {
        var m = Instantiate(m_effect);
        m.transform.position = m_spwanEffect.position;
        m.transform.rotation = Quaternion.Euler(0, 0, -90);
    }

    public void CriticalAttack()
    {
        //this.transform.DOMove(this.transform.position + this.transform.forward * 2, 0.2f);
        transform.LookAt(GameManager.Player.transform);
        this.transform.DOMove(gameObject.transform.position + gameObject.transform.forward * 3, 1f);
        //m_rb.velocity = this.transform.forward * 100;
    }

    public void JumpAttack()
    {
        agent.SetDestination(GameManager.Player.transform.position);
        agent.speed = 20;
        agent.acceleration = 100;
    }

    public void SetPosition()
    {
        agent.SetDestination(transform.position);
    }

    public void BreathAttack()
    {
        Instantiate(m_breath,m_spwanBreath.position, m_spwanBreath.rotation);
        SoundManager.Instance.PlayFireB();
    }

    public void FinalBreathAttack()
    {
        Instantiate(m_finalBreath, m_spwanBreath.position, m_spwanBreath.rotation);
        SoundManager.Instance.PlayFireB();
    }

    public void JumpAttackEffect()
    {
        StartCoroutine("WaitNonActive");
        //iTween.ShakePosition(Camera.main.gameObject, iTween.Hash("x", 0, "y", 1, "time", 1));
    }

    IEnumerator WaitNonActive()
    {
        //Debug.Log("Star");
        attackCollider.SetActive(true);
        yield return new WaitForSeconds(m_hitTime);
        attackCollider.SetActive(false);
    }
}
