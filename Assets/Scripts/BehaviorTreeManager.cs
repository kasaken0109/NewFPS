using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public enum BehaviorStatus
    {
        InActive,
        Running,
        Success,
        Failure,
        Completed,
    }
    public class BehaviorTreeManager
    {
        
        public class ConditionalReevaluate
        {
            public int Index { get; set; }
            public BehaviorStatus Status { get; set; }
            public int CompositeIndex { get; set; }
            public int StackIndex { get; set; }

            public void Initialize(int index, BehaviorStatus status,int composite,int stack)
            {
                Index = index;
                Status = status;
                CompositeIndex = composite;
                StackIndex = stack;
            }

            


        }

        private GameObject m_owner;
        private Node m_rootNode;

        private List<Node> m_nodeList = new List<Node>();
        private List<ConditionalReevaluate> m_reevaluateList = new List<ConditionalReevaluate>();

        private Stack<int> m_activeStack = new Stack<int>();

        private bool IsCompleted = false;

        private int m_activeNodeIndex = -1;

        public BehaviorTreeManager(GameObject owner)
        {
            m_owner = owner;
        }
        
        /// <summary>
        /// NodeをPushする
        /// </summary>
        /// <param name="node">対象Node</param>
        private void PushNode(Node node)
        {
            if (m_activeStack.Count == 0 || m_activeStack.Peek() != node.Index)
            {
                m_activeStack.Push(node.Index);
                m_activeStack.Peek();
            }
        }

        /// <summary>
        /// NodeをPopする
        /// </summary>
        /// <param name="node">対象Node</param>
        private void PopNode()
        {
            m_activeStack.Pop();
            m_activeNodeIndex = m_activeStack.Peek();
        }

        private int CommonAncestorNode(int node1,int node2)
        {
            HashSet<int> parentNodes = new HashSet<int>();

            Node parent1 = m_nodeList[node1].ParentNode;
            return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        private void CallOnAwake(BehaviorTree.Node node)
        {
            //node.Index =
        }
    }
}
