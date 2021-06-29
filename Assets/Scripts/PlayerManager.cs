using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float m_hp = 100;
    [SerializeField] GameObject m_charactor;
    [SerializeField] Text m_hptext = null;
    /// <summary>
    /// 武器のNo.
    /// </summary>
    private int m_weaponNum = 0;
    private float keyInterval = 0f;
    private WeaponManager m_weaponManager;

    public enum WeaponTypes: int
    {
        RIFLE,
        CANNON,
        NUM,
    }

    private string[] m_weaponPath = new string[] {
        "PlayerRifle",
        "PlayerCannon"
    };

    void Start()
    {
        m_weaponManager = m_charactor.GetComponent<WeaponManager>();
    }

    // Update is called once per frame
    void Update()
    {
        m_hptext.text = "HP:" + m_hp.ToString();

        if (Time.time - keyInterval > 0.5f)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) == true)
            {
                keyInterval = Time.time;
                PrevWeapon();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) == true)
            {
                keyInterval = Time.time;
                NextWeapon();
            }
        }
    }

    public void Damage (float damage)
    {
        m_hp -= damage;
        Debug.Log(m_hp);
    }

    private void PrevWeapon()
    {
        m_weaponNum--;
        if (m_weaponNum < 0)
        {
            m_weaponNum = (int)WeaponTypes.NUM - 1;
        }

        m_weaponManager.EquipWeapon(m_weaponPath[m_weaponNum]);
    }

    private void NextWeapon()
    {
        m_weaponNum++;
        if (m_weaponNum >= (int)WeaponTypes.NUM)
        {
            m_weaponNum = 0;
        }

        m_weaponManager.EquipWeapon(m_weaponPath[m_weaponNum]);
    }

    public void AddDamage(int damage)
    {
        throw new System.NotImplementedException();
    }
}
