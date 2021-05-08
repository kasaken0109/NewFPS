using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireLine : MonoBehaviour
{
    /// <summary>照準</summary>
    RectTransform m_crosshairUi = null;
    [SerializeField] GameObject m_muzzle = null;
    /// <summary>照準にターゲットが入っていない時の色</summary>
    //[SerializeField] Color m_crosshairColorOnNoTarget = Color.white;
    /// <summary>照準にターゲットが入っている時の色</summary>
    //[SerializeField] Color m_crosshairColorOnTargeted = Color.red;
    /// <summary>LineRenderer 兼 Line の出発点</summary>
    [SerializeField] LineRenderer m_line = null;
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
    [SerializeField] GameObject m_reload = null;
    [SerializeField] Text m_text;
    /// <summary>マガジン内の弾数</summary>
    public int m_bulletNum;
    AudioSource audio;
    [SerializeField] public int m_bulletMaxNum = 4;
    GameObject m_textBox;
    TargetManager targetManager;
    [SerializeField] Text m_debugText = null;
    bool IsSounded = false;
    bool IsHitSound = false;

    void Start()
    {
        // FPS なのでマウスカーソルを消す。ESC で表示される。
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        m_muzzle = GameObject.FindWithTag("Muzzle");
        m_textBox = GameObject.Find("BulletText");
        m_text = m_textBox.GetComponent<Text>();
        m_line = m_muzzle.GetComponent<LineRenderer>();
        m_reload = GameObject.Find("Reload");
        m_reload.SetActive(false);
        audio = GetComponent<AudioSource>();
        m_crosshairUi = GameObject.Find("Targetaim").GetComponent<RectTransform>();
    }

    void Update()
    {
        m_text.text = m_bulletNum + "/" + m_bulletMaxNum;
        Ray ray = Camera.main.ScreenPointToRay(m_crosshairUi.position);

        RaycastHit hit;
        Vector3 hitPosition = m_line.transform.position + m_line.transform.forward * m_shootRange;  // hitPosition は Ray が当たった場所。Line の終点となる。何にも当たっていない時は Muzzle から射程距離だけ前方にする。
        GameObject hitObject = null;    // Ray が当たったオブジェクト
        m_text.text = m_bulletNum + "/" + m_bulletMaxNum;

        if (Physics.Raycast(ray, out hit, m_shootRange, m_layerMask))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red);
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction, Color.white);
        }

        // Ray が何かに当たったか・当たっていないかで処理を分ける        
        if (Input.GetButton("Fire1"))
        {
            DrawLaser(hitPosition); // レーザーの終点は「Ray が当たっている時は当たった場所、当たっていない時は前方・射程距離ぶんの長さ」になる
            if (!IsSounded)
            {
                PlayShootSound();  // レーザーの発射点で射撃音を鳴らす
                IsSounded = true;
            }
            bool IsHit = Physics.Raycast(ray, out hit, m_shootRange, m_layerMask);
            if (IsHit)
            {
                Debug.Log("Hit!!");
                hitPosition = hit.point;    // Ray が当たった場所
                hitObject = hit.collider.gameObject;    // Ray が洗ったオブジェクト
                Debug.Log(hit.collider.name);
                Hit(hit.collider.gameObject);
            }
            if (hitObject)
            {
                Hit(hitObject);
                if (!IsHitSound)
                {
                    PlayHitSound(hitPosition);  // レーザーが当たった場所でヒット音を鳴らす
                    IsHitSound = true;
                }
                
            }
            else
            {
                IsHitSound = false;
            }
        }
        if (Input.GetButtonUp("Fire1"))
        {
            if (m_bulletNum >= 1)
            {
                m_bulletNum -= 1;
            }
        }
        else
        {
            DrawLaser(m_line.transform.position);   // 撃っていない時は、Line の終点と始点を同じ位置にすることで Line を消す
            IsSounded = false;
        }
        if (m_bulletNum <= 0)
        {
            m_reload.SetActive(true);
        }
        if (Input.GetButtonDown("Reload"))
        {
            AudioSource.PlayClipAtPoint(m_reloadSound, this.transform.position);
            Debug.Log("Reload");
            Reload();
            m_reload.SetActive(false);
        }
    }

    void OnDestroy()
    {
        // 消したマウスカーソルを元に戻す。
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    /// <summary>
    /// ショットがオブジェクトに当たった時に呼び出す。
    /// </summary>
    /// <param name="hitObject"></param>
    void Hit(GameObject hitObject)
    {
        if (hitObject.tag == "Target")
        {
            hitObject.GetComponent<TargetManager>().HitObject();
        }
        else if (hitObject.tag == "Enemy")
        {
            hitObject.GetComponent<EnemyNewController>().Hit();
        }
    }

    /// <summary>
    /// 射撃音を鳴らす
    /// </summary>
    /// <param name="position">音を鳴らす場所</param>
    void PlayShootSound()
    {
        if (m_shootSound)
        {
            audio.PlayOneShot(m_shootSound);
        }
    }

    /// <summary>
    /// ヒット音を鳴らす
    /// </summary>
    /// <param name="position">音を鳴らす場所</param>
    void PlayHitSound(Vector3 position)
    {
        if (m_hitSound)
        {
            AudioSource.PlayClipAtPoint(m_hitSound, position);
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
        m_line.SetPositions(positions);
    }
    public void Reload()
    {
        Debug.Log("リロード中");
        m_bulletNum = m_bulletMaxNum;
        //m_reload.Play();
    }
}
