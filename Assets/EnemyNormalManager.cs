using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNormalManager : MonoBehaviour
{
    NavMeshAgent navMeshAgent = null;
    Animator m_animator = null;
    GameObject m_attack = null;
    [SerializeField]GameObject m_attackCol = null;
    float m_timer = 0f;
    [SerializeField] float m_skillWaitTime = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponentInParent<NavMeshAgent>();
        m_animator = GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        m_timer += Time.deltaTime;
        m_animator.SetFloat("Speed",navMeshAgent.velocity.magnitude);
        Debug.Log($"ss:{navMeshAgent.velocity.magnitude}");
        if(navMeshAgent.velocity.magnitude <= 0.1f)
        {
            m_animator.SetTrigger("Attack");
        }
        if (m_attack && m_timer >= 2f)
        {
            navMeshAgent.SetDestination(m_attack.gameObject.transform.position);
            transform.LookAt(m_attack.transform);
            m_timer = 0f;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            navMeshAgent.SetDestination(other.gameObject.transform.position);
            m_attack = other.gameObject;
        }
    }

    
}
