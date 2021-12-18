using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class Selector : CompositeNode
    {
        public override void OnChildExcuted(BehaviorStatus childstatus)
        {
            m_currentChildIndex++;
            m_status = childstatus;
        }

        public override bool CanExecute()
        {
            return m_currentChildIndex < m_children.Count && m_status != BehaviorStatus.Success;
        }
    }
}

