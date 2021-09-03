using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour,IWeapon
{
    [SerializeField] float[] m_activeColliderTime;
    [SerializeField] GameObject[] m_activeCollider;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NormalAttack()
    {
        StartCoroutine(ColliderGenerater.Instance.GenerateCollider(m_activeCollider[0], 1f));
    }

    public void BasicAttack()
    {
        ColliderGenerater.Instance.StartActiveCollider(m_activeCollider[0], 1f);
    }

    public void SpecialAttack()
    {
        ColliderGenerater.Instance.StartActiveCollider(m_activeCollider[1], 2f);
    }
}
