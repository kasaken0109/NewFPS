﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fire : MonoBehaviour
{

    [SerializeField] GameObject m_bulletPrefab = null;
    GameObject go;
    /// <summary>弾の発射位置</summary>
    [SerializeField] Transform m_muzzle;
    /// <summary>一画面の最大段数 (0 = 無制限)</summary>
    [SerializeField, Range(0, 10)] int m_bulletLimit = 0;

    [SerializeField] float m_fireInterval = 0.15f;
    [SerializeField] AudioClip m_shootSound = null;
    [SerializeField] Animator m_shootAnim = null;
    [SerializeField] Animation m_reload = null;

    public int m_bulletNum;
    [SerializeField] public int m_bulletMaxNum = 4;
    Coroutine m_coroutine;
    Rigidbody2D m_rb;
    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_shootAnim = GetComponent<Animator>();
        m_reload = GetComponent<Animation>();
        m_bulletNum = m_bulletMaxNum;
        if (m_muzzle == null)
        {
            m_muzzle = GameObject.FindGameObjectWithTag("Muzzle").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (m_bulletNum > 0)
            {
                m_shootAnim.SetTrigger("ShootFlag");
                m_coroutine = StartCoroutine(Fire());
            }
            else
            {
                Debug.Log("リロードしてください");
            }
        }
        else if(Input.GetButtonUp("Fire1")|| m_bulletNum <= 0)
        {
            if (m_coroutine != null)
            {
                StopCoroutine(m_coroutine);
            }
        }
        if (Input.GetButtonDown("Reload"))
        {
            Reload();
        }
    }

    void PlayShootSound()
    {
        if (m_shootSound)
        {
            AudioSource.PlayClipAtPoint(m_shootSound, m_muzzle.position);
        }
    }


    IEnumerator Fire()
    {
        while (true)
        {
            if (m_bulletPrefab && m_muzzle) // m_bulletPrefab にプレハブが設定されている時 かつ m_muzzle に弾の発射位置が設定されている時
            {
                go = Instantiate(m_bulletPrefab, m_muzzle.position, m_bulletPrefab.transform.rotation);  // インスペクターから設定した m_bulletPrefab をインスタンス化する
                //Debug.Log("Fire");
                m_bulletNum -= 1;
                PlayShootSound();
                yield return new WaitForSeconds(m_fireInterval);
            }
            
        }
    }

    void BulletInstance()
    {
        go = Instantiate(m_bulletPrefab, m_muzzle.position, m_bulletPrefab.transform.rotation);  // インスペクターから設定した m_bulletPrefab をインスタンス化する
    }

    public void Reload()
    {
        Debug.Log("リロード中");
        m_bulletNum = m_bulletMaxNum;
        m_reload.Play();
    }

}
