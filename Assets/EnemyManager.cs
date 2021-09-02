﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour,IDamage
{
    [SerializeField] int m_hp = 100;
    [SerializeField, Range(1, 100)] int m_rate;
    [SerializeField] int m_attackPower = 10;
    [SerializeField] Animator m_animator = null;
    [SerializeField] GameObject m_deathBody = null;
    [SerializeField] Text m_HpText = null;
    [SerializeField] MoveState _moveState = null;
    ActionCtrl actionCtrl = null;
    int hitRate = 0;
 
    public void AddDamage(int damage)
    {
        if(m_hp > damage)
        {
            m_hp -= damage;
            hitRate += damage;
            if(hitRate >= m_rate)
            {
                m_animator.SetInteger("HP", 1);
                m_animator.SetTrigger("Hit");
                hitRate = 0;
            }
            Debug.Log($"Hit!:{m_hp}");
        }
        else
        {
            m_hp = 0;
            //Debug.Log("EnemyDeath");
            Instantiate(m_deathBody,this.transform.position,this.transform.rotation);
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

    public void SpawnEffects()
    {
        _moveState.SpawnEffect();
    }

    public void StopPlayer()
    {
        GameManager.Instance.CinemaMode();
    }


}
