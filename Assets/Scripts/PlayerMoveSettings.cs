using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Player/Settings")]
public class PlayerMoveSettings : ScriptableObject
{
    [SerializeField]
    [Tooltip("動く速さ")]
    private float m_movingSpeed = 5f;

    [SerializeField]
    [Tooltip("走る速さ")]
    private float m_runningSpeed = 8f;

    [SerializeField]
    [Tooltip("ターンの速さ")]
    private float m_turnSpeed = 3f;

    [SerializeField]
    [Tooltip("ジャンプ力")]
    private float m_jumpPower = 5f;

    [SerializeField]
    [Tooltip("滞空時の水平移動速度の軽減率")]
    private float m_midairSpeedRate = 0.7f;

    [SerializeField]
    [Tooltip("突進力")]
    private float m_dushPower = 10f;

    [SerializeField]
    [Tooltip("突進攻撃力")]
    private int m_dushAttackPower = 15;

    [SerializeField]
    [Tooltip("回避距離")]
    private float m_dodgeLength = 5;

    [SerializeField]
    [Tooltip("回避のクールダウン")]
    private float m_dodgeCoolDown = 0.3f;

    [SerializeField]
    [Tooltip("animator")]
    private RuntimeAnimatorController m_anim = default;

    public float MovingSpeed => m_movingSpeed;

    public float RunningSpeed => m_runningSpeed;

    public float TurnSpeed => m_turnSpeed;

    public float JumpPower => m_jumpPower;

    public float MidairSpeedRate => m_midairSpeedRate;

    public float DushPower => m_dushPower;

    public int DushAttackPower => m_dushAttackPower;

    public float DodgeLength => m_dodgeLength;

    public float DodgeCoolDown => m_dodgeCoolDown;

    public RuntimeAnimatorController Anim => m_anim;
}
