using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateBase
{
    StateBase _idleState;
    [SerializeField] Animator m_animator = null;
    [SerializeField] string m_stateDistance;
    [SerializeField] int m_maxBreathCount;
    [SerializeField] StateBase m_moveState;
    [SerializeField] int[] attackValue;
    [SerializeField] GameObject m_enemy = null;
    int breathCount;
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
            var e = GetComponentInParent<EnemyBossManager>();
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

    void SetActionVariable()
    {
        int value = attackValue[Random.Range(0, attackValue.Length)];
        m_animator.SetInteger("AttackCombo", value);
    }

    IEnumerator AttackRoutine()
    {
        yield return new WaitForFixedUpdate();
        while (true)
        {
            if (!EnemyBossManager.Instance.m_froznBody.activeSelf)
            {
                float distance = Vector3.Distance(GameManager.Player.transform.position, transform.position);
                m_enemy.transform.LookAt(GameManager.Player.transform);
                if (distance <= triggerDistance[0])
                {
                    m_animator.SetInteger("AttackType", 0);
                    SetActionVariable();
                    //yield return new WaitForSeconds(2f);
                    //_actionCtrl.SetCurrent(_attackState);

                }
                else
                {
                    if (triggerDistance[1] <= distance)
                    {
                        //if(m_maxBreathCount <= breathCount)
                        //{
                        //    Debug.Log("MoVe");
                        //    breathCount = 0;
                        //    _actionCtrl.SetCurrent(m_moveState);
                        //}
                        m_animator.SetInteger("AttackType", 2);
                        SetActionVariable();
                        breathCount++;
                    }
                    else
                    {
                        m_animator.SetInteger("AttackType", 1);
                        SetActionVariable();
                    }
                    //yield return new WaitForSeconds(2f);
                }
            }
            yield return null;
        }

    }
}
