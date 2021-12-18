using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BehaviorTree
{
    public class ActionNode : Node
    {
        private Func<GameObject, BehaviorStatus> m_action;

        public ActionNode(Func<GameObject,BehaviorStatus> action)
        {
            m_action = action;
        }

        public override BehaviorStatus OnUpdate()
        {
            base.OnUpdate();
            m_status = m_action.Invoke(Owner);
            return m_status;
        }
    }
}

