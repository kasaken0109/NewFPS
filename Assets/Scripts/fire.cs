using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fire : MonoBehaviour
{

    [SerializeField] GameObject m_bulletPrefab = null;
    /// <summary>弾の発射位置</summary>
    [SerializeField] Transform m_muzzle;
    /// <summary>一画面の最大段数 (0 = 無制限)</summary>
    [SerializeField, Range(0, 10)] int m_bulletLimit = 0;

    [SerializeField] float m_fireInterval = 0.15f;
    [SerializeField] AudioClip m_shootSound = null;
    [SerializeField] Animator m_shootAnim = null;
    Coroutine m_coroutine;
    Rigidbody2D m_rb;
    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_shootAnim = GetComponent<Animator>();
        if (m_muzzle == null)
        {
            m_muzzle = GameObject.FindGameObjectWithTag("Muzzle").transform;
        }
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            m_coroutine = StartCoroutine(Fire());
            m_shootAnim.SetTrigger("ShootFlag");
            
            //if (m_bulletLimit == 0 || this.GetComponentsInChildren<PlayerBulletController>().Length < m_bulletLimit)    // 画面内の弾数を制限する
            //{
            //    Fire1();
            //}
        }
        else if(Input.GetButtonUp("Fire1"))
        {
            if (m_coroutine != null)
            {
                StopCoroutine(m_coroutine);
            }
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
                GameObject go = Instantiate(m_bulletPrefab, m_muzzle.position, m_bulletPrefab.transform.rotation);  // インスペクターから設定した m_bulletPrefab をインスタンス化する
                Debug.Log("Fire");
                PlayShootSound();
                yield return new WaitForSeconds(m_fireInterval);
            }
            
        }
    }

    void BulletInstance()
    {
        GameObject go = Instantiate(m_bulletPrefab, m_muzzle.position, m_bulletPrefab.transform.rotation);  // インスペクターから設定した m_bulletPrefab をインスタンス化する
    }

}
