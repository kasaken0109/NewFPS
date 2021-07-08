using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] NavMeshAgent m_navMeshAgent = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 敵の行動種類
    /// </summary>
    public enum ActionType
    {
        PATROL,//巡回
        CHASE,//追跡
        ATTACK,//攻撃
    } 
}
