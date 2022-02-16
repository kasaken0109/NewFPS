using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackGenerator : MonoBehaviour
{
    [Header("頭のコライダー")]
    [SerializeField] Collider m_head;
    [SerializeField] Collider m_body;
    [SerializeField] Collider m_crow;
    [SerializeField] Collider m_horn;
    // Start is called before the first frame update
    public void GenerateHeadAttackCollider(float activeTime)
    {
        m_head.gameObject.SetActive(true);
        StartCoroutine(WaitCount(m_head.gameObject, activeTime));
    }

    public void GenerateBodyAttackCollider(float activeTime)
    {
        m_body.gameObject.SetActive(true);
        StartCoroutine(WaitCount(m_body.gameObject, activeTime));
    }

    public void GenerateHornAttackCollider(float activeTime)
    {
        m_horn.gameObject.SetActive(true);
        StartCoroutine(WaitCount(m_horn.gameObject, activeTime));
    }
    public void GenerateCrowAttackCollider(GameObject collider, float activeTime)
    {
        m_crow.gameObject.SetActive(true);
        StartCoroutine(WaitCount(m_crow.gameObject, activeTime));
    }

    IEnumerator WaitCount(GameObject collider, float activeTime)
    {
        yield return new WaitForSeconds(activeTime);
        collider.SetActive(false);
        StopCoroutine(WaitCount(collider,activeTime));

    }
}
