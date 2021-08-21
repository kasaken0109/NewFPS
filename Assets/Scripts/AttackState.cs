using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateBase
{
    StateBase _idleState;
    [SerializeField] Animator _animator = null;
    [SerializeField] Dictionary<string, bool> stateInfo = new Dictionary<string, bool>();
    Vector3 m_cachedTargetPosition;

    protected override void Setup()
    {
        _idleState = GetComponent<IdleState>();
        SetDelegate(Event.Enter, EnterCallback);
        SetDelegate(Event.Leave, ExitCallback);
    }

    int EnterCallback()
    {
        _animator.SetBool("Attack", true);
        Debug.Log("EnterAttack");
        StartCoroutine(nameof(routine));
        if (_opponentTag == "Player")
        {
            var e = GetComponentInParent<EnemyManager>();
            e.transform.LookAt(GameManager.Player.transform.position);
        }
        return 0;
    }

    int ExitCallback()
    {
        _animator.SetBool("Attack", false);
        StopCoroutine(nameof(routine));
        return 0;
    }

    IEnumerator routine()
    {
        yield return new WaitForFixedUpdate();
        while (true)
        {
            m_cachedTargetPosition = GameManager.Player.transform.position;
            float distance = Vector3.Distance(m_cachedTargetPosition, transform.position);
            gameObject.transform.LookAt(GameManager.Player.transform);
            if (distance <= 10)
            {
                _animator.Play("Roar");
                yield return new WaitForSeconds(2f);
                //_actionCtrl.SetCurrent(_attackState);

            }
            yield return new WaitForEndOfFrame();
        }

    }

    //private void OnCollisionExit(Collision collision)
    //{
    //    if (_opponentTag == "") return;
    //    if (collision.gameObject == null) return;
    //    if (collision.gameObject.CompareTag(_opponentTag))
    //    {
    //        _actionCtrl.SetCurrent(_idleState);
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (_opponentTag == "") return;
    //    if (other.gameObject == null) return;
    //    if (other.gameObject.CompareTag(_opponentTag))
    //    {
    //        _actionCtrl.SetCurrent(_idleState);
    //    }
    //}
}
