﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateBase
{
    StateBase _idleState;
    [SerializeField] Animator m_animator = null;
    [SerializeField] string m_stateDistance;
    [SerializeField] StateBase m_moveState;
    [SerializeField] GameObject m_enemy = null;

    string[] triggersDistances;
    List<int> triggerDistance;

    protected override void Setup()
    {
        _idleState = GetComponent<IdleState>();
        triggersDistances = m_stateDistance.Split(' ');
        triggerDistance = new List<int>();
        foreach (var item in triggersDistances)
        {
            triggerDistance.Add(int.Parse(item));
        }
        SetDelegate(Event.Enter, EnterCallback);
        SetDelegate(Event.Leave, ExitCallback);
    }

    int EnterCallback()
    {
        m_animator.SetBool("Attack", true);
        StartCoroutine(nameof(AttackRoutine));
        if (_opponentTag == "Player")
        {
            var e = GetComponentInParent<EnemyManager>();
            e.transform.LookAt(GameManager.Player.transform.position);
        }
        return 0;
    }

    int ExitCallback()
    {
        m_animator.SetBool("Attack", false);
        StopCoroutine(nameof(AttackRoutine));
        return 0;
    }

    IEnumerator AttackRoutine()
    {
        yield return new WaitForFixedUpdate();
        while (true)
        {
            float distance = Vector3.Distance(GameManager.Player.transform.position, transform.position);
            m_enemy.transform.LookAt(GameManager.Player.transform);
            if (distance  <= triggerDistance[0])
            {
                m_animator.SetInteger("AttackType",0);
                //yield return new WaitForSeconds(2f);
                //_actionCtrl.SetCurrent(_attackState);

            }
            else
            {
                if (triggerDistance[1] <= distance)
                {
                    //_actionCtrl.SetCurrent(m_moveState);
                }
                else
                {
                    m_animator.SetInteger("AttackType", 1);
                }
                //yield return new WaitForSeconds(2f);
            }
            yield return null;
        }

    }
}
