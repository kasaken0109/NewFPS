﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireLine : MonoBehaviour
{
    #region SerializeGameObject
    [SerializeField]
    GameObject m_muzzle = null;
    [SerializeField]
    GameObject m_effect = null;
    [SerializeField]
    GameObject m_bullet = null;
    [SerializeField]
    GameObject m_shield = null;
    #endregion
    /// <summary>LineRenderer 兼 Line の出発点</summary>
    [SerializeField]
    LineRenderer m_line = null;
    /// <summary>リロード時間</summary>
    [SerializeField]
    float m_seconds = 2f;
    /// <summary>射程距離</summary>
    [SerializeField]
    float m_shootRange = 15f;
    /// <summary>当たるレイヤー</summary>
    [SerializeField]
    LayerMask m_layerMask = 0;
    /// <summary>発射した時の音</summary>
    [SerializeField]
    AudioClip m_shootSound = null;
    /// <summary>命中した時の音</summary>
    [SerializeField] 
    AudioClip m_hitSound = null;
    [SerializeField]
    AudioClip m_reloadSound = null;
    [SerializeField]
    AudioClip m_airshoot = null;
    
    [SerializeField] Text m_text;
    [SerializeField] float m_chargeTime = 3f;
    [SerializeField] int m_attackpower = 10;
    /// <summary>マガジン内の弾数</summary>
    public int m_bulletNum;
    
    public int m_bulletMaxNum = 4;
    GameObject m_reload = null;
    GameObject m_particleMuzzle = null;
    GameObject m_textBox = null;
    GameObject m_fireLine = null;

    /// <summary>照準</summary>
    RectTransform m_crosshairUi = null;
    Transform m_shieldSpawn = null;
    bool IsSounded = false;
    bool IsHitSound = false;
    bool IsEndHit = false;
    bool CanShoot = true;
    bool IsCreate = false;
    bool IsReload = false;
    ShieldDisplayController m_shieldDisplay = null;
    Vector3 hitPosition;
    ParticleSystem particleSystem;
    
    
    AudioSource audio;

    void Start()
    {
        GameObject.Find("RifleImage").GetComponent<Image>().fillAmount = 1;
        m_bulletNum = PlayerPrefs.GetInt("Bullet1");
        // FPS なのでマウスカーソルを消す。ESC で表示される。
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        m_reload = PlayerManager.Instance.m_reloadImage;
        m_textBox = PlayerManager.Instance.m_textBox1;
        m_text = m_textBox.GetComponent<Text>();
        m_text.text = m_bulletNum + "/" + m_bulletMaxNum;
        m_muzzle = GameObject.FindWithTag("Muzzle");
        m_line = m_muzzle.GetComponent<LineRenderer>();
        m_reload = PlayerManager.Instance.m_reloadImage;
        m_reload?.SetActive(false);
        audio = GetComponent<AudioSource>();
        m_crosshairUi = GameObject.Find("Targetaim").GetComponent<RectTransform>();
        StartCoroutine(Changeattack());
    }
    IEnumerator Changeattack()
    {
        float timer = 0;
        while (timer < 0.5f)
        {
            if (Input.GetButtonDown("Fire1")) GameManager.Instance.m_player.GetComponent<PlayerControll>().DushAttack(-10);
            timer += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
    }

    private void OnEnable()
    {
        m_bulletNum = PlayerPrefs.GetInt("Bullet1");
        m_fireLine = GameObject.Find("FireLine");
        m_muzzle = GameObject.FindWithTag("Muzzle");
        m_particleMuzzle = GameObject.Find("BulletSpawn");
        m_shieldDisplay = GameObject.Find("ShieldImage").GetComponent<ShieldDisplayController>();
        m_line = m_muzzle.GetComponent<LineRenderer>();
    }

    private void Awake()
    {
        m_bulletNum = PlayerPrefs.GetInt("Bullet1");
        m_fireLine = GameObject.Find("FireLine");
        StopCoroutine(nameof(WaitSeconds));
    }

    void Update()
    {
        if (Time.timeScale == 0) return;
        if(!IsReload)m_text.text = m_bulletNum + "/" + m_bulletMaxNum;

        Ray ray = Camera.main.ScreenPointToRay(m_crosshairUi.position);
        transform.LookAt(new Vector3(Camera.main.transform.position.x,Camera.main.transform.position.y,int.MaxValue));
        Vector3 pos = Camera.main.ScreenToWorldPoint(m_crosshairUi.position);
        RaycastHit hit;

        GameObject hitObject = null;    // Ray が当たったオブジェクト

        if (Input.GetButtonDown("Fire1"))
        {
            if (m_bulletNum <= 0 && !IsReload)
            {
                //音の処理
                m_reload?.SetActive(true);
                Reload();
            }
            if (m_shieldDisplay.ShieldValue < 1 && m_bulletNum == 4) StartCoroutine(nameof(Fireline));
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            SoundManager.Instance.StopSE();
            StopCoroutine(nameof(Fireline));
            if (IsCreate && m_shieldDisplay.ShieldValue >= 1)
            {
                var v = GameObject.Find("ShieldPrefab(Clone)");
                if (v != null) return;
                m_shieldSpawn = GameObject.Find("ShieldSpawn").transform;
                var m = Instantiate(m_shield);
                m.transform.position = m_shieldSpawn.position;
                m.transform.SetParent(m_shieldSpawn);
                m.transform.localRotation = Quaternion.identity;
                IsCreate = false;
            }
            else
            {
                if (m_bulletNum >= 1)
                {
                    SoundManager.Instance.PlayShoot();
                    Instantiate(m_bullet,m_particleMuzzle.transform.position,Camera.main.transform.rotation);
                    m_bulletNum -= 1;
                    bool IsHit = Physics.Raycast(ray, out hit, m_shootRange, m_layerMask);

                    if (IsHit)
                    {
                        
                        hitPosition = hit.point;    // Ray が当たった場所
                        hitObject = hit.collider.gameObject;    // Ray が洗ったオブジェクト

                        if (hitObject)
                        {
                            if (hitObject.tag == "Enemy" || hitObject.tag == "Item")
                            {
                                IsSounded = !IsSounded ? true : false;
                                hitObject.GetComponentInParent<IDamage>().AddDamage(m_attackpower);
                                Instantiate(m_effect, hitPosition, Quaternion.identity);
                            }
                            if (!IsHitSound)
                            {
                                PlayHitSound(hitPosition);  // レーザーが当たった場所でヒット音を鳴らす
                                IsHitSound = true;
                            }
                        }
                    }
                }
            }
            
        }

        if (Input.GetButtonDown("Reload") && !IsReload)
        {
            Reload();
        }

    }
    IEnumerator Fireline()
    {
        float time = 0;
        IsCreate = false;
        yield return new WaitForSeconds(1f);
        SoundManager.Instance.PlayCharge();
        while (m_shieldDisplay.ShieldValue < 1)
        {
            time += 0.01f;
            yield return new WaitForSeconds(0.01f);
            m_shieldDisplay.ChangeValues(m_shieldDisplay.ShieldValue + 0.5f * time / m_chargeTime, 0.01f);
            if (m_bulletNum != m_bulletMaxNum && time > 1.5)
            {
                m_shieldDisplay.ChangeValues(0, 0.5f);
                yield break;
            }
        }
        IsCreate = true;
    }

    void OnDestroy()
    {
        PlayerPrefs.SetInt("Bullet1", m_bulletNum);
        PlayerPrefs.Save();
        StopCoroutine(nameof(WaitSeconds));
        m_text.text = m_bulletNum + "/" + m_bulletMaxNum;
    }

    /// <summary>
    /// 射撃音を鳴らす
    /// </summary>
    /// <param name="position">音を鳴らす場所</param>
    void PlayShootSound(AudioClip audioClip)
    {
        audio?.PlayOneShot(m_shootSound);
    }

    /// <summary>
    /// ヒット音を鳴らす
    /// </summary>
    /// <param name="position">音を鳴らす場所</param>
    void PlayHitSound(Vector3 position)
    {
        if (m_hitSound) AudioSource.PlayClipAtPoint(m_hitSound, position, 0.1f);
    }

    /// <summary>
    /// Line Renderer を使ってレーザーを描く
    /// </summary>
    /// <param name="destination">レーザーの終点</param>
    void DrawLaser(Vector3 destination)
    {
        Vector3[] positions = { m_line.transform.position, destination };   // レーザーの始点は常に Muzzle にする
        m_line.positionCount = positions.Length;   // Line を終点と始点のみに制限する
        if (m_bulletNum >= 1) m_line.SetPositions(positions);
    }
    void Reload()
    {
        AudioSource.PlayClipAtPoint(m_reloadSound, this.transform.position);
        m_text.text ="リロード中";
        StopCoroutine(nameof(WaitSeconds));
        StartCoroutine(nameof(WaitSeconds));
    }

    IEnumerator WaitSeconds()
    {
        m_reload?.SetActive(false);
        var i = GameObject.Find("RifleImage").GetComponent<Image>();
        i.fillAmount = 0;
        IsReload = true;
        while(i.fillAmount <= 0.99f)
        {
            i.fillAmount += 0.01f;
            yield return new WaitForSeconds(m_seconds / 100);
        }
        m_bulletNum = m_bulletMaxNum;
        IsReload = false;
    }
}
