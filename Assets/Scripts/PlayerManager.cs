using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float m_hp = 100;
    [SerializeField] GameObject m_charactor;

    public enum WeaponTypes: int
    {
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage (float damage)
    {
        m_hp -= damage;
        Debug.Log(m_hp);
    }
}
