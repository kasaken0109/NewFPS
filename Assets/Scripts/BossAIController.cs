using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

public class BossAIController : MonoBehaviour
{
    [SerializeReference, SubclassSelector(true)]
    ActionNode actionNode;

    [SerializeField]
    private Transform m_destination;

    private bool m_reachPoint = true;
    private Transform m_target;

    [SerializeField]
    private float m_speed = 12f;

    private float distance;
    private BehaviorTreeManager m_behaviorTree;
    private Animator m_anim;
    private NavMeshAgent m_nav;

    private void Start()
    {
        m_target = transform;
        m_anim = GetComponent<Animator>();
        m_nav = GetComponentInParent<NavMeshAgent>();

        m_behaviorTree = new BehaviorTreeManager(gameObject);
        m_destination = m_destination == null ? GameManager.Player.transform : m_destination;

        // 目的地へついたかの分岐
        ConditionalNode chaseCondition = new ConditionalNode((owner) =>
        {
            return m_reachPoint ? BehaviorStatus.Success : BehaviorStatus.Failure;
        });

        #region ### 目的地への移動 ###
        ActionNode rotateToPoint1 = new ActionNode((owner) =>
        {
            m_target.LookAt(m_destination);
            return BehaviorStatus.Success;
        });

        ActionNode movetoPoint1 = new ActionNode((owner) =>
        {
            m_nav.SetDestination(m_destination.position);

            float ep = m_nav.stoppingDistance;
            distance = Vector3.Distance(m_target.position, m_destination.position);
            m_nav.speed = distance >= ep * 2 ? m_speed : m_speed * 0.5f; 
            m_anim.SetFloat("Speed", m_nav.speed);
            if (distance < ep)
            {
                Debug.Log("Reach");
                m_anim.SetFloat("Speed", 0);
                m_reachPoint = true;
                return BehaviorStatus.Success;
            }
            return BehaviorStatus.Running;
        });

        ActionNode rotateToPoint2 = new ActionNode((owner) =>
        {
            Debug.Log("1");
            m_anim.Play("Roar");
            m_anim.Play("SpinAttack");
            EnemyBossManager.Instance.m_hpUI.SetActive(true);
            return BehaviorStatus.Success;
        });

        ActionNode attack1 = new ActionNode((owner) =>
        {
            Debug.Log("2");
            m_anim.SetBool("Attack", true);
            m_anim.SetInteger("AttackType", 0);
            return BehaviorStatus.Success;
        });

        ActionNode setIdle = new ActionNode((owner) =>
        {
            Debug.Log("3");
            m_anim.SetBool("Attack", false);
            return BehaviorStatus.Success;
        });

        ActionNode wait = new ActionNode((owner) =>
        {
            m_anim.SetBool("Attack", false);
            new WaitForSeconds(5f);
            m_anim.SetBool("Attack", true);
            return BehaviorStatus.Success;
        });

        Sequencer chasePlayer = new Sequencer();
        chasePlayer.NeedsConditionalAbort = true;
        chasePlayer.AddNodes(new Node[] { chaseCondition, movetoPoint1 ,rotateToPoint1,rotateToPoint2,wait,attack1});
        #endregion ### ポイント1への移動 ###

        // 定義したビヘイビアを設定
        Selector selector = new Selector();
        selector.NeedsConditionalAbort = true;
        selector.AddNode(chasePlayer);

        Repeater repeater = new Repeater();
        repeater.AddNode(selector);
        m_behaviorTree.SetRootNode(selector);

        m_behaviorTree.Start();
    }

    private void Update()
    {
        m_behaviorTree.Update();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

        }
    }
}
