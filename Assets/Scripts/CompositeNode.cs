using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class CompositeNode : Node
    {
        private bool m_needsConditionalAbort = false;
        public bool NeedsConditionalAbort { get => m_needsConditionalAbort; set { m_needsConditionalAbort = value; } }

        private bool m_hasConditionalNode = false;

        protected int m_currentChildIndex = 0;
        public int CurrentChildIndex => m_currentChildIndex;

        protected List<Node> m_children = new List<Node>();

        public List<Node> Children => m_children;

        public CompositeNode() { }

        #region CompositeNode

        public virtual bool CanExecute()
        {
            return true;
        }

        /// <summary>
        /// 中断検知時に呼び出される
        /// </summary>
        /// <param name="childNodeIndex"></param>
        public virtual void OnConditionalAbort(int childNodeIndex)
        {
            OnEnd();
            m_currentChildIndex = 0;
        }

        public virtual void OnChildExcuted(BehaviorStatus childstatus)
        {
            if (m_currentChildIndex < m_children.Count || NeedsConditionalAbort && m_hasConditionalNode) return;
            if(childstatus == BehaviorStatus.Completed || childstatus == BehaviorStatus.Success)
            {
                m_status = BehaviorStatus.Completed;
            }
        }

        #endregion 

        public override void OnAwake()
        {
            m_currentChildIndex = 0;

            for (int i = 0; i < m_children.Count; i++)
            {
                if(m_children[i] is ConditionalNode)
                {
                    m_hasConditionalNode = true;
                    return;
                }
            }
        }

        public override void OnStart()
        {
            base.OnStart();
            m_currentChildIndex = 0;
            if (CanExecute())
            {
                Node current = m_children[m_currentChildIndex];
            }
        }

        public override void OnAbort()
        {
            base.OnAbort();
            m_currentChildIndex = 0;
        }

        public override BehaviorStatus OnUpdate()
        {
            base.OnUpdate();
            return m_status;
        }

        public override void AddNode(Node child)
        {
            if (!Children.Contains(child))
            {
                child.ParentNode = this;
                m_children.Add(child);
            }
        }

        public override void AddNodes(Node[] Children)
        {
            foreach (var item in Children)
            {
                AddNode(item);
            }
        }
    }

}