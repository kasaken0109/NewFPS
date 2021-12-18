using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree{


    public abstract class Node
    {
        protected GameObject m_owner;

        public GameObject Owner { get => m_owner; set {m_owner = value; } }

        protected int m_index = -1;

        public int Index { get => m_index;set { m_index = value; } }

        protected Node m_parentNode;

        public Node ParentNode { get => m_parentNode; set { m_parentNode = value; } }

        protected string m_name;

        public string Name { get => m_name; set { m_name = value; } }

        protected BehaviorStatus m_status = BehaviorStatus.InActive;

        public BehaviorStatus Status => m_status;

        public Node()
        {
            m_name = GetType().ToString();
        }

        /// <summary>
        /// Tree起動時に一回だけ呼ぶ関数
        /// </summary>
        public virtual void OnAwake()
        {
            ///今は何もしない
        }

        /// <summary>
        /// Node実行時に一回だけ呼ばれる
        /// </summary>
        public virtual void OnStart()
        {
            Debug.Log($"{Name}のNode行動開始");
            m_status = BehaviorStatus.Running;
        }

        public virtual BehaviorStatus OnUpdate()
        {
            if(m_status == BehaviorStatus.Completed)
            {
                Debug.Log("このTaskは終了しました");
                return m_status;
            }
            if(m_status == BehaviorStatus.InActive)
            {
                OnStart();
            }
            return m_status;
        }

        /// <summary>
        /// Task完了時に一回だけ呼ばれる
        /// </summary>
        public virtual void OnEnd()
        {
            if(m_status == BehaviorStatus.Completed)
            {
                return;
            }

            m_status = BehaviorStatus.InActive;
        }

        /// <summary>
        /// Task中断時に呼ばれる
        /// </summary>
        public virtual void OnAbort()
        {
            OnEnd();
        }

        /// <summary>
        /// Nodeを追加
        /// </summary>
        /// <param name="child">子のNode</param>
        public virtual void AddNode(Node child)
        {
            ///何もしない
        }

        /// <summary>
        /// 複数のNodeを追加
        /// </summary>
        /// <param name="Children">対象のNode</param>
        public virtual void AddNodes(Node[] Children)
        {
            ///何もしない
        }



        


    }
}
