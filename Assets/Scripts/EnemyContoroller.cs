using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyContoroller : MonoBehaviour
{
    Rigidbody m_rb;
    [SerializeField] GameObject attackCollider;
    [SerializeField] float m_hitTime = 1f;

    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    public void BasicAttack()
    {
        Debug.Log("Basic");
        //this.transform.DOMove(this.transform.position + this.transform.forward * 2, 0.2f);
        m_rb.DOMove(this.transform.position + this.transform.forward * 2, 1f);
        //m_rb.velocity = this.transform.forward * 100;
    }

    public void JumpAttack()
    {
        m_rb.DOMove(this.transform.position + this.transform.up * 2, 1f);
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
