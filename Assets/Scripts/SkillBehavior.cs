using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SkillBehavior : MonoBehaviour
{
    public static SkillBehavior Instance { get; private set; }

    [SerializeField]
    AttackcolliderController[] m_attackControllers = default;

    [SerializeField]
    AttackcolliderController[] m_defenceControllers = default;

    [SerializeField]
    UnityEvent _speedBuf;
    private void Awake()
    {
        Instance = this;
    }
    
    public void CallPassive(ref PassiveSkill passiveSkill)
    {
        switch (passiveSkill.PassiveType)
        {
            case PassiveType.SwordAttackBuf:
                foreach (var item in m_attackControllers)
                {
                    item.StartAttackCorrectionValue(passiveSkill.EffectAmount, passiveSkill.EffectableTime);
                }
                break;
            case PassiveType.DefenceBuf:
                foreach (var item in m_attackControllers)
                {
                    item.StartDefenceCorrectionValue(passiveSkill.EffectAmount, passiveSkill.EffectableTime);
                }
                break;
            case PassiveType.MoveSpeedBuf:
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControll>().SpeedStart(passiveSkill.EffectableTime,passiveSkill.EffectAmount);
                break;
            case PassiveType.AttackSpeedBuf:
                break;
            case PassiveType.BulletAttackBuf:
                break;
            case PassiveType.DodgeDistanceBuf:
                break;
            case PassiveType.AttackReachBuf:
                break;
            default:
                break;
        }
    }
}
