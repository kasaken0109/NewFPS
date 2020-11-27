﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletController : MonoBehaviour
{
    // Start is called before the first frame update
    
    /// <summary>弾の飛ぶ速度</summary>
    [SerializeField] float m_bulletSpeed = 10f;
    Rigidbody m_rb;
    [SerializeField] float m_bulletPower = 12f;
    GameObject m_player;

    void Start()
    {
        //m_player = GameObject.FindWithTag("Player");
        m_rb = GetComponent<Rigidbody>();
        this.m_rb.velocity = Camera.main.transform.forward * m_bulletSpeed;
        //this.m_rb.velocity = m_player.transform.forward * m_bulletSpeed;

    }
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<EnemyContoroller>())
        {
            collision.gameObject.GetComponent<EnemyContoroller>().Hit(m_bulletPower);
        }
        Destroy(this.gameObject);
    }
}
