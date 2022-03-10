using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Bullets/Create Bullet")]
public class Bullet : ScriptableObject
{
    [SerializeField]
    private GameObject m_bullet;

    [SerializeField]
    private int m_damage = 1;

    [SerializeField]
    private float m_delay = 0.2f;

    [SerializeField]
    [Tooltip("消費するスタンス値")]
    [Range(-1, 1)]
    private float m_consumeStanceValue;

    [SerializeField]
    private BulletType m_bulletType = default;

    [SerializeField]
    private int m_bulletID = 0;

    [SerializeField]
    private string m_name = default;


    public GameObject MyBullet => m_bullet;

    public int Damage => m_damage;

    public float ConsumeStanceValue => m_consumeStanceValue;

    public BulletType BulletType => m_bulletType;

    public int BulletID => m_bulletID;

    public float Delay => m_delay;

    public string Name => m_name;
}
