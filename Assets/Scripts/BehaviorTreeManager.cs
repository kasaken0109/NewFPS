using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
            while (parent1 != null)
            {
                parentNodes.Add(parent1.Index);
                parent1 = parent1.ParentNode;
            }

            Node parent2 = m_nodeList[node1].ParentNode;
            int num = parent2.Index;
            while (!parentNodes.Contains(num))
            {
                parent2 = parent2.ParentNode;
                num = parent2.Index;
            }
            return num;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        private void CallOnAwake(BehaviorTree.Node node)
        {
            node.Index = m_nodeList.Count;
            m_nodeList.Add(node);
            node.Owner = m_owner;
            node.OnAwake();
            CompositeNode compositeNode = node as CompositeNode;
            if (compositeNode != null)
            {
                foreach (var child in compositeNode.Children)
                {
                    CallOnAwake(child);
                }
            }
        }

        private int ReevaluateConditionalTasks() 
        {
            for (int i = 0; i < m_reevaluateList.Count; i++)
            {
                ConditionalReevaluate cr = m_reevaluateList[i];
                BehaviorStatus status = m_nodeList[cr.Index].OnUpdate();

                if (cr.Status != status)
                {
                    CompositeNode cnode = m_nodeList[cr.CompositeIndex] as CompositeNode;
                    if (cnode != null)
                    {
                        cnode.OnConditionalAbort(cr.Index);
                    }
                    m_reevaluateList.Remove(cr);
                    return cr.Index;
                }
            }


            return -1;
        }

        private BehaviorStatus Execute(Node node)
        {
            PushNode(node);
            if(node is CompositeNode)
            {
                CompositeNode cnode = node as CompositeNode;

                while (cnode.CanExecute())
                {
                    Node child = cnode.Children[cnode.CurrentChildIndex];
                    BehaviorStatus childStatus = Execute(child);
                    if (cnode.NeedsConditionalAbort)
                    {
                        if (child is ConditionalNode)
                        {
                            m_reevaluateList.Add(new ConditionalReevaluate
                            {
                                Index = child.Index,
                                CompositeIndex = cnode.Index,
                                Status = childStatus
                            });
                        }
                    }

                    if (childStatus == BehaviorStatus.Running)
                    {
                        return BehaviorStatus.Running;
                    }

                    cnode.OnChildExcuted(childStatus);

                }
                if (cnode.Status != BehaviorStatus.Running)
                {
                    cnode.OnEnd();
                    PopNode();
                }
                return cnode.Status;
            }
            else
            {
                BehaviorStatus status = node.OnUpdate();
                if (status != BehaviorStatus.Running)
                {
                    node.OnEnd();
                    PopNode();
                }
                return status;
            }
        }

        public void SetRootNode(Node rootNode)
        {
            m_rootNode = rootNode;
        }

        public void Start()
        {
            Debug.LogWarning("Tree Start");
            CallOnAwake(m_rootNode);
        }

        public void Update()
        {
            if (IsCompleted)
            {
                return;
            }

            int abortIndex = ReevaluateConditionalTasks();
            if(abortIndex != -1)
            {
                int caIndex = CommonAncestorNode(abortIndex, m_activeNodeIndex);

                Node activeNode = m_nodeList[m_activeNodeIndex];
                activeNode.OnAbort();

                while (m_activeStack.Count != 0)
                {
                    PopNode();
                    activeNode = m_nodeList[m_activeNodeIndex];
                    activeNode.OnAbort();

                    if (m_activeNodeIndex == caIndex)
                    {
                        break;
                    }

                    ConditionalReevaluate cr = m_reevaluateList.FirstOrDefault(r => r.Index == m_activeNodeIndex);
                    if (cr != null)
                    {
                        m_reevaluateList.Remove(cr);
                    }
                }
            }

            BehaviorStatus status = BehaviorStatus.InActive;
            if (m_activeNodeIndex == -1)
            {
                status = Execute(m_rootNode);
            }
            else
            {
                Node node = m_nodeList[m_activeNodeIndex];
                status = Execute(node);
            }
            if (status == BehaviorStatus.Completed)
            {
                Debug.LogWarning("Status Completed");
                IsCompleted = true;
            }

        }
    }
}
