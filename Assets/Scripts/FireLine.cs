using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireLine : MonoBehaviour
{
    /// <summary>照準</summary>
    RectTransform m_crosshairUi = null;
    [SerializeField] GameObject m_muzzle = null;
    [SerializeField] GameObject m_effect = null;
    /// <summary>LineRenderer 兼 Line の出発点</summary>
    [SerializeField] LineRenderer m_line = null;
    /// <summary>リロード時間</summary>
    [SerializeField] float m_seconds = 2f;
    /// <summary>射程距離</summary>
    [SerializeField] float m_shootRange = 15f;
    /// <summary>当たるレイヤー</summary>
    [SerializeField] LayerMask m_layerMask = 0;
    /// <summary>発射した時の音</summary>
    [SerializeField] AudioClip m_shootSound = null;
    /// <summary>命中した時の音</summary>
    [SerializeField] AudioClip m_hitSound = null;
    [SerializeField] AudioClip m_reloadSound = null;
    [SerializeField] AudioClip m_airshoot = null;
    GameObject m_reload = null;
    [SerializeField] Text m_text;
    [SerializeField] int m_attackpower = 10;
    /// <summary>マガジン内の弾数</summary>
    public int m_bulletNum;
    AudioSource audio;
    public int m_bulletMaxNum = 4;
    GameObject m_textBox = null;
    GameObject m_fireLine = null;
    [SerializeField]GameObject m_shield = null;
    Transform m_shieldSpawn = null;
    bool IsSounded = false;
    bool IsHitSound = false;
    bool IsEndHit = false;
    bool CanShoot = true;
    Vector3 hitPosition;
    ParticleSystem particleSystem;

    void Start()
    {
        m_bulletNum = PlayerPrefs.GetInt("Bullet1");
        // FPS なのでマウスカーソルを消す。ESC で表示される。
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        m_muzzle = GameObject.FindWithTag("Muzzle");
        m_textBox = GameObject.Find("BulletText");
        m_text = m_textBox.GetComponent<Text>();
        m_line = m_muzzle.GetComponent<LineRenderer>();
        m_reload = GameObject.Find("Reload");
        m_reload?.SetActive(false);
        audio = GetComponent<AudioSource>();
        m_crosshairUi = GameObject.Find("Targetaim").GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        m_bulletNum = PlayerPrefs.GetInt("Bullet1");
        m_fireLine = GameObject.Find("FireLine");
        m_muzzle = GameObject.FindWithTag("Muzzle");
        m_line = m_muzzle.GetComponent<LineRenderer>();
        particleSystem = m_fireLine.GetComponent<ParticleSystem>();
        particleSystem.Stop();
    }

    private void Awake()
    {
        m_bulletNum = PlayerPrefs.GetInt("Bullet1");
        m_fireLine = GameObject.Find("FireLine");
        particleSystem = m_fireLine.GetComponent<ParticleSystem>();
        particleSystem.Stop();
    }

    void Update()
    {
        m_text.text = m_bulletNum + "/" + m_bulletMaxNum;
        
        Ray ray = Camera.main.ScreenPointToRay(m_crosshairUi.position);
        RaycastHit hit;

        GameObject hitObject = null;    // Ray が当たったオブジェクト

        if (Input.GetButtonDown("Fire1"))
        {
            if (m_bulletNum <= 0)
            {

            }
            StartCoroutine(nameof(Fire));
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(nameof(Fire));
            if (IsSpecial)
            {
                m_shieldSpawn = GameObject.Find("ShieldSpawn").transform;
                var m = Instantiate(m_shield);
                m.transform.position = m_shieldSpawn.position;
                m.transform.SetParent(m_shieldSpawn);
                m.transform.localRotation = Quaternion.identity;
            }
            else
            {
                PlayShootSound();
            }
        }
        if (Input.GetButtonDown("Reload"))
        {
            AudioSource.PlayClipAtPoint(m_reloadSound, this.transform.position);
            Debug.Log("Reload");
            Reload();
        }
    }

    [SerializeField] float m_chargeTime = 3f;
    bool IsSpecial = false;

    IEnumerator Fire()
    {
        IsSpecial = false;
        yield return new WaitForSeconds(1f);
        if(m_bulletNum !=m_bulletMaxNum)
        yield return new WaitForSeconds(m_chargeTime -1);
        IsSpecial = true;
    }

    private RaycastHit DebugDrawLine(ref Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, m_shootRange, m_layerMask))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red);
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction, Color.white);
        }
        return hit;
    }

    void OnDestroy()
    {
        PlayerPrefs.SetInt("Bullet1", m_bulletNum);
        Debug.Log(m_bulletNum);
        PlayerPrefs.Save();
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
        if (m_hitSound)
        {
            AudioSource.PlayClipAtPoint(m_hitSound, position,0.1f);
        }
    }

    /// <summary>
    /// Line Renderer を使ってレーザーを描く
    /// </summary>
    /// <param name="destination">レーザーの終点</param>
    void DrawLaser(Vector3 destination)
    {
        Vector3[] positions = { m_line.transform.position, destination };   // レーザーの始点は常に Muzzle にする
        m_line.positionCount = positions.Length;   // Line を終点と始点のみに制限する
        if (m_bulletNum >= 1)
        {
            m_line.SetPositions(positions);
        }
    }
    public void Reload()
    {
        Debug.Log("リロード中");
        StopCoroutine(nameof(WaitSeconds));
        StartCoroutine("WaitSeconds");
    }

    IEnumerator WaitSeconds()
    {
        yield return new WaitForSeconds(m_seconds);
        m_bulletNum = m_bulletMaxNum;
        //m_reload.Play();
        m_reload?.SetActive(false);
    }
}
