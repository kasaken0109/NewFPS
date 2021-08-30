using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveState : StateBase
{
    /// <summary>発生エフェクト</summary>
    [SerializeField] GameObject m_roarEffect = null;
    /// <summary>エフェクト発生地点</summary>
    [SerializeField] Transform m_spawnEffect = null;
    [SerializeField] StateBase _attackState;
    [SerializeField] EnemyManager enemy;
    /// <summary>移動対象</summary>
    Vector3 m_cachedTargetPosition;
    StateBase _idleState;
    Animator animator;
    NavMeshAgent m_agent;

    protected override void Setup()
    {
        _idleState = GetComponent<IdleState>();
        animator = GetComponentInParent<Animator>();
        m_agent = GetComponentInParent<NavMeshAgent>();
        SetDelegate(Event.Enter, EnterCallback);
        SetDelegate(Event.Leave, ExitCallback);
    }

    int EnterCallback()
    {
        StartCoroutine("routine");
        Debug.Log("EnterRoutine");
        return 0;
    }

    int ExitCallback()
    {
        Debug.Log("ExitRoutine");
        StopCoroutine("routine");
        animator.SetFloat("Speed", 0);
        m_agent.SetDestination(this.transform.position);
        return 0;
    }
    IEnumerator routine()
    {
        yield return new WaitForFixedUpdate();
        while (true)
        {
            m_cachedTargetPosition = GameManager.Player.transform.position;
            float distance = Vector3.Distance(m_cachedTargetPosition, transform.position);
            m_agent.SetDestination(m_cachedTargetPosition); // Navmesh Agent に目的地をセットする（Vector3 で座標を設定していることに注意。Transform でも GameObject でもなく、Vector3 で目的地を指定する)
            enemy.gameObject.transform.LookAt(GameManager.Player.transform);
            animator.SetFloat("Speed", m_agent.velocity.magnitude);
            if (distance <= m_agent.stoppingDistance)
            {
                animator.Play("Roar");
                
                yield return new WaitForSeconds(2f);
                _actionCtrl.SetCurrent(_attackState);

            }
            else
            {
                Debug.Log(m_agent.velocity.magnitude);
                if(distance > 8)
                {
                    m_agent.SetDestination(m_cachedTargetPosition);
                    m_agent.speed = 6;
                    animator.SetFloat("Speed", 6);
                }
                else
                {
                    m_agent.SetDestination(m_cachedTargetPosition);
                    animator.SetFloat("Speed", 3);
                }
            }
            yield return null;
        }
         
    }

    public void SpawnEffect()
    {
        StartCoroutine("SpawnWait");
        GameManager.Instance.ShakeCamera();
    }

    IEnumerator SpawnWait()
    {
        Instantiate(m_roarEffect, m_spawnEffect.transform.position, m_spawnEffect.transform.rotation);
        yield return new WaitForSeconds(2f);
    }
    private void OnTriggerExit(Collider other)
    {
        if (_opponentTag == "") return;
        if (other.gameObject == null) return;
        if (other.gameObject.CompareTag(_opponentTag))
        {
            _actionCtrl.SetCurrent(_idleState);
        }
    }
}
