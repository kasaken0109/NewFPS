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
        private void CallOnAwake(BehaviorTree.Node node)
        {
            //node.Index =
        }
    }
}
