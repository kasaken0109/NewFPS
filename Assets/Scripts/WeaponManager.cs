using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] string weaponTempleteName = "RigPistolRight";
    [SerializeField] GameObject m_weaponTemplete;
    [SerializeField] GameObject m_weapon = null;

    // Start is called before the first frame update
    void Start()
    {
        EquipWeapon("PlayerRifle");
    }

    // Update is called once per frame
    public void EquipWeapon(string name)
    {
        if (m_weapon != null)
        {
            Destroy(m_weapon);
            m_weapon = null;
            Resources.UnloadUnusedAssets();
        }

        m_weapon = Instantiate(Resources.Load(name), m_weaponTemplete.transform.position,m_weaponTemplete.transform.rotation ) as GameObject;
        m_weapon.transform.parent = m_weaponTemplete.transform;
    }
}
