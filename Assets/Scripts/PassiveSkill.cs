using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "PassiveSkill")]
public class PassiveSkill:ScriptableObject
{
    [SerializeField]
    [Tooltip("適用する効果の種類")]
    private PassiveType passiveType = default;

    [SerializeField]
    [Tooltip("適用される効果量")]
    [Range(-1, 1)]
    private float effectAmount = 0; 
}

public enum PassiveType
{
    AttackBuf,
    DefenceBuf,
    SpeedBuf,
    Stancebuf,
    AddEffect,
}
