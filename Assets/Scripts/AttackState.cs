using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateBase
{
    StateBase _idleState;
    [SerializeField] private Animator m_animator = null;
    [SerializeField] private string m_stateDistance;
    [SerializeField] private int m_maxBreathCount;
    [SerializeField] private StateBase m_moveState;
    [SerializeField] private int[] attackValue;
    [SerializeField] private GameObject m_enemy = null;
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
            //e.transform.LookAt(GameManager.Player.transform.position);
        }
        Debug.Log("EnterAttack");
        return 0;
    }

    int ExitCallback()
    {
        m_animator.SetBool("Attack", false);
        StopCoroutine(nameof(AttackRoutine));
        Debug.Log("ExitAttack");
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
            float time = 0f;
            Vector3 target = m_enemy.transform.position;
            if (!EnemyBossManager.Instance.m_froznBody.activeSelf)
            {
                while (time < 5f)
                {
                    target += (GameManager.Player.transform.position - m_enemy.transform.position) * Time.deltaTime / 5f;
                    m_enemy.transform.LookAt(target);
                    Debug.Log(time);
                    time += Time.deltaTime;
                    yield return new WaitForSeconds(Time.deltaTime);
                }
                if (!EnemyBossManager.Instance.IsCritical)
                {
                    float distance = Vector3.Distance(GameManager.Player.transform.position, transform.position);
                    //m_enemy.transform.LookAt(GameManager.Player.transform);
                    if (distance <= triggerDistance[0])
                    {
                        m_animator.SetInteger("AttackType", 0);
                        SetActionVariable();
                    }
                    else
                    {
                        if (triggerDistance[1] <= distance)
                        {
                            m_animator.SetInteger("AttackType", 2);
                            SetActionVariable();
                            breathCount++;
                        }
                        else
                        {
                            m_animator.SetInteger("AttackType", 1);
                            SetActionVariable();
                        }
                    }
                }
            }
            yield return null;
        }

    }
}
