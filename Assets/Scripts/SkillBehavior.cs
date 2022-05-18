using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBehavior : MonoBehaviour
{
    public static SkillBehavior Instance { get; private set; }

    [SerializeField]
    AttackcolliderController[] m_attackControllers = default;

    [SerializeField]
    AttackcolliderController[] m_defenceControllers = default;
    private void Awake()
    {
        Instance = this;
    }
    
    public void CallPassive(ref PassiveSkill passiveSkill)
    {
        switch (passiveSkill.PassiveType)
        {
            case PassiveType.AttackBuf:
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
            case PassiveType.SpeedBuf:
                break;
            case PassiveType.HealBuf:
                break;
            case PassiveType.Stancebuf:
                break;
            case PassiveType.AddEffect:
                break;
            default:
                break;
        }
    }
}
