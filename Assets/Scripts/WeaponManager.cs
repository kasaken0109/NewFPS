using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] string weaponTempleteName = "RigPistolRight";
    [SerializeField] private GameObject m_weaponTemplete;
    [SerializeField] private GameObject m_weaponSwordTemplete;
    [SerializeField] private GameObject m_weapon = null;

    public GameObject NowWeapon
    {
        get => m_weapon;
    }

    // Start is called before the first frame update
    void Start()
    {
        EquipWeapon("PlayerRifle");
    }

    // Update is called once per frame
    public void EquipWeapon(string name)
    {
        GameObject find = GameObject.Find($"{name}(Clone)");
        if (find != null) return;
        if (m_weapon != null)
        {
            Destroy(m_weapon);
            m_weapon = null;
            Resources.UnloadUnusedAssets();
        }
        if (name.Equals("Sword"))
        {
            m_weapon = Instantiate(Resources.Load(name), m_weaponSwordTemplete.transform.position,m_weaponSwordTemplete.transform.rotation ) as GameObject;
            m_weapon.transform.parent = m_weaponSwordTemplete.transform;
        }
        else
        {
            m_weapon = Instantiate(Resources.Load(name), m_weaponTemplete.transform.position, m_weaponTemplete.transform.rotation) as GameObject;
            m_weapon.transform.parent = m_weaponTemplete.transform;
        }
        //m_weapon = Instantiate(Resources.Load(name), m_weaponTemplete.transform.position,m_weaponTemplete.transform.rotation ) as GameObject;
        
    }
}
