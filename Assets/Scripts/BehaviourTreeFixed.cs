using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


namespace BehaviourAI
{
    public interface IBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        GameObject SetTarget();
        /// <summary>
        /// BehaviorTreeの呼び出し
        /// </summary>
        /// <param name="Set">セットするアクション</param>
        void Call(IAction Set);
    }
    public interface IConditional
    {
        GameObject target { set; }

        /// <summary>
        /// 遷移条件にあっているかどうか
        /// </summary>
        /// <returns></returns>
        bool Check();
    }

    public interface IAction
    {
        GameObject target { set; }

        /// <summary>
        /// 実行時に呼ばれる関数
        /// </summary>
        void Execute();

        /// <summary>
        /// 終了時に呼ばれる関数
        /// </summary>
        /// <returns>終了したかどうか</returns>
        bool End();

        bool Reset { set; }
    }

    public class BehaviourTreeManager : MonoBehaviour
    {
        enum State
        {
            Run,
            Set,
            None,
        }

        State state = State.None;
        [SerializeField]
        List<Selector> selectors = new List<Selector>();

        [Serializable]
        public class Selector
        {
            [SerializeReference, SubclassSelector]
            public List<Sequence> Sequences = new List<Sequence>();
            
            [Serializable]
            public class Sequence
            {
                [SerializeReference, SubclassSelector]
                public IConditional Conditional;
                [SerializeReference, SubclassSelector]
                public IAction Action;
            }
        }

        public void Repeater<T>(T get) where T:IBehaviour
        {
            GameObject target = get.SetTarget();
            switch (state)
            {
                case State.Run:
                    break;
                case State.Set:
                    break;
                case State.None:
                    break;
                default:
                    break;
            }
        }

        class SelectorNode
        {
            public int GetID => m_id;

            private int m_id = 0;
        }
    }
}
