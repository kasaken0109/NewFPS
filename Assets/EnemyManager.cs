using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour,IDamage
{
    [SerializeField] int m_hp = 100;
    [SerializeField] int m_attackPower = 10;
    [SerializeField] Animator m_animator = null;
    [SerializeField] Text m_HpText = null;
    ActionCtrl actionCtrl = null;
 
    public void AddDamage(int damage)
    {
        if(m_hp > damage)
        {
            m_hp -= damage;
            m_animator.SetInteger("HP", 1);
            m_animator.SetTrigger("Hit");
            Debug.Log($"Hit!:{m_hp}");
        }
        else
        {
            m_hp = 0;
            Debug.Log("EnemyDeath");
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        actionCtrl = new ActionCtrl();
        actionCtrl.SetCurrent(GetComponentInChildren<IdleState>());
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_HpText) return;
        m_HpText.text = "HP :" + m_hp;
    }


}
