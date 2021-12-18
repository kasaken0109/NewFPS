using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BehaviorTree
{
    public class ConditionalNode : Node
    {
        private Func<GameObject, BehaviorStatus> m_condition;

        public ConditionalNode(Func<GameObject,BehaviorStatus> condition)
        {
            m_condition = condition;
        }

        public override BehaviorStatus OnUpdate()
        {
            base.OnUpdate();
            m_status = m_condition.Invoke(Owner);
            return m_status;
        }
    }
}

