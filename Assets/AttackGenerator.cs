using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackGenerator : MonoBehaviour
{
    [SerializeField] Collider m_head;
    [SerializeField] Collider m_crow;
    // Start is called before the first frame update
    public void GenerateHeadAttackCollider(float activeTime)
    {
        m_head.gameObject.SetActive(true);
        StartCoroutine(WaitCount(m_head.gameObject, activeTime));
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
