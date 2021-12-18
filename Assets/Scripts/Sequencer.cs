using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class Sequencer : CompositeNode
    {
        public override void OnChildExcuted(BehaviorStatus childstatus)
        {
            m_currentChildIndex++;
            base.OnChildExcuted(childstatus);
        }

        public override bool CanExecute()
        {
            return m_currentChildIndex < m_children.Count && m_status != BehaviorStatus.Failure;
        }
    }

}s
