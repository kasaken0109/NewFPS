using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletType
{
    Lay,
    Physics,

}

[System.Serializable]
public class Bullet
{
    [SerializeField]
    private GameObject m_bullet;

    [SerializeField]
    private int m_damage = 1;

    [SerializeField]
    private BulletType m_bulletType = default;

    [SerializeField]
    private int m_bulletID = 0;

    public GameObject MyBullet => m_bullet;

    public int Damage => m_damage;

    public BulletType BulletType => m_bulletType;

    public int BulletID => m_bulletID;
}

[System.Serializable]
[CreateAssetMenu(menuName = "Bullet/Create Bullet")]
public class Gun : ScriptableObject
{
    [SerializeField]
    private Bullet m_bullet = default;

    [SerializeField]
    private int m_magNum = 4;

    [SerializeField]
    private int m_ID = 0;

    [SerializeField]
    private float m_delay = 0.2f;

    public Bullet MyBullet => m_bullet;

    public int MagNum => m_magNum;

    public int ID => m_ID;

    public float Delay => m_delay;
}

public class BulletFire : MonoBehaviour
{
    [SerializeField]
    [Tooltip("")]
    private List<Gun> m_guns;

    [SerializeField]
    private int m_initNum = 10;

    private List<int> m_bulletNum;

    private Bullet m_equip;
    // Start is called before the first frame update
    void Start()
    {
        m_guns = new List<Gun>();
        BulletInit();
    }

    private void BulletInit()
    {
        for (int i = 0; i < m_guns.Count; i++)
        {
            m_bulletNum.Add(m_initNum);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EquipBullet(Bullet bullet)
    {
        m_equip = bullet;
    }

    public void AddBullet(Bullet bullet, int getNum)
    {
        m_bulletNum[bullet.BulletID] += getNum;
    }
}
