using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("追跡対象のオブジェクト")]
    private GameObject m_chaseTarget = default;
    [SerializeField]
    [Tooltip("対象との距離")]
    private Vector3 m_chaseOffset = default;
    [SerializeField]
    [Tooltip("追跡を開始する距離")]
    private float m_startChaseDistance = 2f;
    [SerializeField]
    [Tooltip("追跡時間")]
    private float m_chaseTime = 2f;

    [SerializeField]
    [Tooltip("回転時間")]
    private float m_chaseLookTime = 0.5f;

    private float distance;

    private GameObject m_attackTarget;

    // Start is called before the first frame update
    void Start()
    {
        m_attackTarget = GameObject.FindGameObjectWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        LookEnemy();
        distance = Vector3.Distance(transform.position,m_chaseTarget.transform.position);
        if (distance >= m_startChaseDistance) Chase();
    }

    private void Chase()
    {
        Vector3 destination = m_chaseTarget.transform.position + m_chaseOffset;
        transform.position = Vector3.Slerp(transform.position,destination,m_chaseTime * Time.deltaTime);
    }

    private void LookEnemy()
    {
        if (!m_attackTarget) return;
        Vector3 diff = m_attackTarget.transform.position - transform.position;
        Quaternion lookAngle = Quaternion.LookRotation(diff);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookAngle,m_chaseLookTime *Time.deltaTime);
    }
}
