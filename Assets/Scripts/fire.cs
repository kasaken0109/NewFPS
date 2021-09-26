using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fire : MonoBehaviour
{

    [SerializeField] GameObject m_bulletPrefab = null;
    [SerializeField] GameObject m_bigWallPrefab = null;
    GameObject go;
    /// <summary>弾の発射位置</summary>
    [SerializeField] Transform m_muzzle;
    /// <summary>一画面の最大段数 (0 = 無制限)</summary>
    [SerializeField, Range(0, 10)] int m_bulletLimit = 0;
    GameObject m_reload = null;
    /// <summary>リロード時間</summary>
    [SerializeField] float m_seconds = 2f;
    [SerializeField] float m_fireInterval = 0.15f;
    [SerializeField] AudioClip []m_shootSound = null;
    //[SerializeField] AudioClip m_airSound = null;

    [SerializeField] Animator m_shootAnim = null;

    public int m_bulletNum;
    [SerializeField] Text m_text;
    [SerializeField] public int m_bulletMaxNum = 4;
    GameObject m_textBox;
    Coroutine m_coroutine;
    bool IsSpecial = false;
    bool IsReload = false;
    // Start is called before the first frame update
    void Start()
    {
        m_shootAnim = GetComponent<Animator>();
        m_bulletNum = PlayerPrefs.GetInt("Bullet2");
        GameManager.Instance.m_shootweaponImage.GetComponent<Image>().fillAmount = 1;
        m_reload = PlayerManager.Instance.m_reloadImage;
        if (m_muzzle == null)
        {
            m_muzzle = GameObject.FindGameObjectWithTag("Muzzle").transform;
        }
        //m_reload.SetActive(false);
    }

    private void Awake()
    {
        m_textBox = PlayerManager.Instance.m_textBox2;
        m_text = m_textBox.GetComponent<Text>();
        m_bulletNum = PlayerPrefs.GetInt("Bullet2");
        m_text.text = m_bulletNum +"/" + m_bulletMaxNum;
        
        if (m_bulletNum == 0) m_reload?.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0) return;
        if(!IsReload)m_text.text = m_bulletNum + "/" + m_bulletMaxNum;

        if (Input.GetButtonDown("Fire1"))
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            {
            if (m_bulletNum > 0)
            {
                m_shootAnim.SetTrigger("ShootFlag");
                if (m_coroutine != null)
                {
                    StopCoroutine(m_coroutine);
                }
                if (m_bulletNum >= 4)
                {
                    StartCoroutine(nameof(BigWall));
                }
            }
            else if(!IsReload)
            {
                AudioSource.PlayClipAtPoint(m_shootSound[1], m_muzzle.position,0.1f);
                Debug.Log("リロードしてください");
                m_reload.SetActive(true);

            }
        }
        else if(Input.GetButtonUp("Fire1"))
        {
            SoundManager.Instance.StopSE();
            if (IsSpecial)
            {
                StopCoroutine(nameof(BigWall));
                IsSpecial = false;
            }
            else
            {
                StartCoroutine(nameof(Fire));
                StopCoroutine(nameof(BigWall));
            }
        }
        if (Input.GetButtonDown("Reload") && !IsReload)
        {
            AudioSource.PlayClipAtPoint(m_shootSound[2], this.transform.position);
            Debug.Log("Reload");
            Reload();
        }
    }

    void OnDestroy()
    {
        PlayerPrefs.SetInt("Bullet2", m_bulletNum);
        Debug.Log(m_bulletNum);
        PlayerPrefs.Save();
        StopCoroutine(nameof(WaitSeconds));
        m_text.text = m_bulletNum + "/" + m_bulletMaxNum;
    }

    void PlayShootSound()
    {
        if (m_shootSound[0])
        {
            AudioSource.PlayClipAtPoint(m_shootSound[0], m_muzzle.position,0.1f);
        }
    }


    IEnumerator Fire()
    {
        if (m_bulletPrefab && m_muzzle && m_bulletNum >= 1) // m_bulletPrefab にプレハブが設定されている時 かつ m_muzzle に弾の発射位置が設定されている時
        {
            go = Instantiate(m_bulletPrefab, m_muzzle.position, Camera.main.transform.rotation);  // インスペクターから設定した m_bulletPrefab をインスタンス化す                                                                                         //Debug.Log("Fire");
            m_bulletNum -= 1;
            PlayShootSound();
            yield return new WaitForSeconds(m_fireInterval);
        }
    }

    IEnumerator BigWall()
    {
        SoundManager.Instance.PlayCharge();
        yield return new WaitForSeconds(1.5f);
        IsSpecial = true;
        var player = GameManager.Instance.m_player.gameObject;
        var playerCol = player.GetComponent<Collider>();
        var m = Instantiate(m_bigWallPrefab);
        m_bulletNum = 0;
        m.transform.position = new Vector3(player.transform.position.x, playerCol.bounds.min.y, player.transform.position.z);
        m.transform.rotation = player.transform.rotation;


    }

    void BulletInstance()
    {
        go = Instantiate(m_bulletPrefab, m_muzzle.position, m_bulletPrefab.transform.rotation);  // インスペクターから設定した m_bulletPrefab をインスタンス化する
    }

    void Reload()
    {
        m_text.text = "リロード中";
        StopCoroutine(nameof(WaitSeconds));
        StartCoroutine("WaitSeconds");
    }

    IEnumerator WaitSeconds()
    {
        m_reload?.SetActive(false);
        var i = GameObject.Find("CannonImage").GetComponent<Image>();
        i.fillAmount = 0;
        IsReload = true;
        while (i.fillAmount <= 0.99f)
        {
            i.fillAmount += 0.01f;
            yield return new WaitForSeconds(m_seconds / 100);
        }
        //yield return new WaitForSeconds(m_seconds);
        m_bulletNum = m_bulletMaxNum;
        //m_reload.Play();
        IsReload = false;
    }

}
