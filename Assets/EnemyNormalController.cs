using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNormalController : MonoBehaviour,IDamage
{
    [SerializeField] int m_hp = 150;
    [SerializeField] GameObject m_deadBody = null;
    [SerializeField] GameObject m_attackCol = null;
    [SerializeField] GameObject m_gate = null;
    [SerializeField] float m_skillWaitTime = 0.5f;
    Animator m_anim;
    public void AddDamage(int damage)
    {
        if(damage <= m_hp)
        {
            m_hp -= damage;
        }
        else
        {
            Instantiate(m_deadBody, transform.position, transform.rotation);
            m_gate.SetActive(true);
            Destroy(this.gameObject);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        m_anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void ActiveCol()
    {
        ColliderGenerater.Instance.StartActiveCollider(m_attackCol, m_skillWaitTime);
    }
}
