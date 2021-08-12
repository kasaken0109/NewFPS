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
        //IAnimationClipSource;
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
            //Debug.Log("Step1");
            m_agent.SetDestination(m_cachedTargetPosition); // Navmesh Agent に目的地をセットする（Vector3 で座標を設定していることに注意。Transform でも GameObject でもなく、Vector3 で目的地を指定する)
            //Debug.Log("Step2");
            gameObject.transform.LookAt(GameManager.Player.transform);
            //Debug.Log("Step4");
            animator.SetFloat("Speed", m_agent.velocity.magnitude);
            //Debug.Log("Step5");
            Debug.Log(GameManager.Player.transform.position);
            yield return new WaitForEndOfFrame();
        }
         
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
