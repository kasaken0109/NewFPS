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
    private float m_delay = 0.2f;
}

public class BulletFire : MonoBehaviour
{
    [SerializeField]
    private Gun m_gun;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
