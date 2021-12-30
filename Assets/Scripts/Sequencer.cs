using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class Sequencer : CompositeNode
    {
        /// <summary>
        /// s
        /// </summary>
        /// <param name="childstatus"></param>
        public override void OnChildExcuted(BehaviorStatus childstatus)
        {
            m_currentChildIndex++;
            base.OnChildExcuted(childstatus);
        }

        /// <summary>
        /// 小ノードの実行が全て実行されたかどうか返す
        /// </summary>
        /// <returns></returns>
        public override bool CanExecute()
        {
            return m_currentChildIndex < m_children.Count && m_status != BehaviorStatus.Failure;
        }
    }

}
