using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class EnemyAI : MonoBehaviour
{
    /// <summary>移動先となる位置情報</summary>
    //[SerializeField] Transform m_target;
    [SerializeField] GameObject e_target;
    /// <summary>移動先座標を保存する変数</summary>
    Vector3 m_cachedTargetPosition;
    /// <summary>キャラクターなどのアニメーションするオブジェクトを指定する</summary>
    [SerializeField] Animator m_animator;
    /// <summary>移動をやめ攻撃を開始する距離</summary>
    [SerializeField] float m_attackDistance = 3f;
    /// <summary>巡回をやめ追跡を開始する距離</summary>
    [SerializeField] float m_chaseDistance = 8f;
    /// <summary>移動をやめ攻撃を開始する距離</summary>
    [SerializeField] float m_actionWaitTime = 5f;
    float enemyDistance;
    NavMeshAgent m_agent;
    ActionType actionType;
    // Start is called before the first frame update
    void Start()
    {
        actionType = ActionType.PATROL;
        e_target = GameObject.FindGameObjectWithTag("Player");
        m_agent = GetComponent<NavMeshAgent>();
        enemyDistance = Vector3.Distance(transform.position, e_target.transform.position);
        m_cachedTargetPosition = e_target.transform.position; // 初期位置を保存する（※）
        StartCoroutine(SearchRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    List<ActionType> actionTypes = new List<ActionType>();
    /// <summary>
    /// 敵の行動種類
    /// </summary>
    public enum ActionType
    {
        PATROL,//巡回
        CHASE,//追跡
        ATTACK,//攻撃
    }

    IEnumerator SearchRoutine()
    {
        enemyDistance = Vector3.Distance(transform.position, e_target.transform.position);
        if(enemyDistance < m_chaseDistance)
        {
            actionType = ActionType.CHASE;
        }
        yield return new WaitForSeconds(m_actionWaitTime);
    }
}


