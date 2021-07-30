using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveState : StateBase
{
    StateBase _idleState;
    Vector3 m_cachedTargetPosition;
    Animator animator;
    float distance;
    NavMeshAgent m_agent;
    // Start is called before the first frame update
    protected override void Setup()
    {
        _idleState = GetComponent<IdleState>();
        animator = GetComponentInParent<Animator>();
        m_agent = GetComponentInParent<NavMeshAgent>();
        animator.SetBool("Move", true);
        m_cachedTargetPosition = GameManager.Player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(GameManager.Player.transform.position,this.transform.position);
        m_cachedTargetPosition = GameManager.Player.transform.position;
        m_agent.SetDestination(m_cachedTargetPosition); // Navmesh Agent に目的地をセットする（Vector3 で座標を設定していることに注意。Transform でも GameObject でもなく、Vector3 で目的地を指定する)
        gameObject.transform.LookAt(GameManager.Player.transform);
        animator.SetFloat("Speed", m_agent.velocity.magnitude);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_opponentTag == "") return;
        if (other.gameObject == null) return;
        if (other.gameObject.CompareTag(_opponentTag))
        {
            _actionCtrl.SetCurrent(_idleState);
        }
    }
}
