using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class EnemyContorollerAi : MonoBehaviour
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
    float enemyDistance;
    NavMeshAgent m_agent;

    void Start()
    {
        e_target = GameObject.FindGameObjectWithTag("Player");
        enemyDistance = Vector3.Distance(transform.position, e_target.transform.position);
        m_agent = GetComponent<NavMeshAgent>();
        m_cachedTargetPosition = e_target.transform.position; // 初期位置を保存する（※）
    }

    /*
     * （※）m_cachedTargetPosition を使って座標を保存しているのは、以下の Update() 内で「毎フレーム座標をセットする」という処理を避け、負荷を下げるためである。
     * 毎フレーム座標をセットすることで経路の計算を毎フレームしてしまうことを避けるため、「Target が移動した時のみ」目的地をセットして経路の計算を行わせている。
     */

    void Update()
    {
        Debug.Log(enemyDistance);
        if (e_target != null)
        {
            if ((enemyDistance > 0.1f || enemyDistance > m_attackDistance) && !m_animator.GetBool("Attack")) // m_target が 10cm 以上移動したら
            {
                m_cachedTargetPosition = e_target.transform.position; // 移動先の座標を保存する
                m_agent.SetDestination(m_cachedTargetPosition); // Navmesh Agent に目的地をセットする（Vector3 で座標を設定していることに注意。Transform でも GameObject でもなく、Vector3 で目的地を指定する)
                gameObject.transform.LookAt(e_target.transform);
                if (m_animator)
                {
                    m_animator.SetFloat("Speed", m_agent.velocity.magnitude);
                }
            }

            // m_animator がアサインされていたら Animator Controller にパラメーターを設定する

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        m_animator.SetBool("Attack", true);
    }

    private void OnTriggerExit(Collider other)
    {
        m_animator.SetBool("Attack", false);
    }
}
