using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyContoroller : MonoBehaviour
{
    Rigidbody m_rb;
    [SerializeField] GameObject attackCollider;
    [SerializeField] GameObject m_breath;
    [SerializeField] GameObject m_finalBreath;
    [SerializeField] Transform m_spwanBreath;
    [SerializeField] float m_hitTime = 1f;

    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    public void BasicAttack()
    {
        Debug.Log("Basic");
        //this.transform.DOMove(this.transform.position + this.transform.forward * 2, 0.2f);
        this.transform.DOMove(gameObject.transform.position + gameObject.transform.forward * 2, 1f);
        //m_rb.velocity = this.transform.forward * 100;
    }

    public void JumpAttack()
    {
        this.transform.DOMove(GameManager.Player.transform.position, 2f);
    }

    public void SetPosition()
    {
        gameObject.transform.position = GameManager.Player.transform.position;
    }

    public void BreathAttack()
    {
        Instantiate(m_breath,m_spwanBreath.position, m_spwanBreath.rotation);
    }

    public void FinalBreathAttack()
    {
        Instantiate(m_finalBreath, m_spwanBreath.position, m_spwanBreath.rotation);
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
