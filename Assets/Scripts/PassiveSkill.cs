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
    [Tooltip("効果時間")]
    private float effectableTime = 5f;

    [SerializeField]
    [Tooltip("適用される効果量")]
    [Range(-1, 1)]
    private float effectAmount = 0;

    [SerializeField]
    [Tooltip("消費コスト")]
    [Range(-1, 1)]
    private float consumeCost = 0.2f;

    [SerializeField]
    [Tooltip("説明文")]
    private string explainText = default;

    [SerializeField]
    private string skillName = default;

    [SerializeField]
    [Tooltip("")]
    private Sprite image = default;
    public PassiveType PassiveType => passiveType;

    public float EffectableTime => effectableTime;

    public float EffectAmount => effectAmount;

    public float ConsumeCost => consumeCost;

    public string ExplainText => explainText;

    public Sprite ImageBullet => image;

    public string SkillName => skillName;
}

public enum PassiveType
{
    AttackBuf,
    DefenceBuf,
    SpeedBuf,
    HealBuf,
    Stancebuf,
    AddEffect,
}
