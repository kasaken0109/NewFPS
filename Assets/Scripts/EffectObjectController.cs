using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectObjectController : MonoBehaviour,IDamage
{
    [SerializeField] GameObject m_effect;
    [SerializeField] GameObject m_item;
    [SerializeField] GameObject m_mine;
    public void AddDamage(int damage)
    {
        StartCoroutine(nameof(InstanceObj));
    }

    IEnumerator InstanceObj()
    {
        
        Instantiate(m_effect, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.2f);
        Instantiate(m_item, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
