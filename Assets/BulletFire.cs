using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BulletType
{
    Lay,
    Physics,

}

[System.Serializable]
[CreateAssetMenu(menuName = "Bullets/Create Bullet")]
public class Bullet :ScriptableObject
{
    [SerializeField]
    private GameObject m_bullet;

    [SerializeField]
    private int m_damage = 1;

    [SerializeField]
    private float m_delay = 0.2f;

    [SerializeField]
    [Tooltip("消費するスタンス値")]
    [Range(-1,1)]
    private float m_consumeStanceValue;

    [SerializeField]
    private BulletType m_bulletType = default;

    [SerializeField]
    private int m_bulletID = 0;

    public GameObject MyBullet => m_bullet;

    public int Damage => m_damage;

    public float ConsumeStanceValue => m_consumeStanceValue;

    public BulletType BulletType => m_bulletType;

    public int BulletID => m_bulletID;

    public float Delay => m_delay;
}

public class BulletFire : MonoBehaviour
{
    [SerializeField]
    [Tooltip("弾の種類を管理するリスト")]
    private List<Bullet> m_bullets;

    [SerializeField]
    [Tooltip("照準のUI")]
    RectTransform m_crosshairUi = null;

    [SerializeField]
    [Tooltip("弾のエフェクトの発生する地点")]
    Transform m_particleMuzzle = null;

    [SerializeField]
    [Tooltip("スタンス")]
    Slider m_stance = default;

    [SerializeField]
    [Tooltip("射程距離")]
    private float m_shootRange = 15f;

    [SerializeField]
    [Tooltip("当たるレイヤー")]
    private LayerMask m_layerMask = 0;

    /// <summary>発射した時の音</summary>
    [SerializeField]
    private AudioClip m_shootSound = null;
    /// <summary>命中した時の音</summary>
    [SerializeField]
    private AudioClip m_hitSound = null;

    [SerializeField]
    private GameObject m_effect = null;
    [SerializeField]
    private GameObject m_frostEffect = null;

    [SerializeField]
    private PlayerControll m_player;

    [SerializeField]
    private Animator m_anim;

    private Bullet m_equip;

    private float stanceValue = 0.5f;

    bool IsSounded = false;
    bool IsHitSound = false;
    bool IsCreate = false;


    Vector3 hitPosition;
    // Start is called before the first frame update
    void Start()
    {
        m_stance.value = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0) return;//一時停止中には射撃不可

        Ray ray = Camera.main.ScreenPointToRay(m_crosshairUi.position);
        transform.LookAt(new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, int.MaxValue));
        Vector3 pos = Camera.main.ScreenToWorldPoint(m_crosshairUi.position);
        RaycastHit hit;

        GameObject hitObject = null;    // Ray が当たったオブジェクト

        if (Input.GetButtonDown("Fire2"))
        {
            StartCoroutine(nameof(Fireline));
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            SoundManager.Instance.StopSE();
            StopCoroutine(nameof(Fireline));
            if (stanceValue > m_equip.ConsumeStanceValue)
            {
                SoundManager.Instance.PlayShoot();
                Instantiate(m_equip.MyBullet, m_particleMuzzle.position, Camera.main.transform.rotation);
                stanceValue -= m_equip.ConsumeStanceValue;
                m_stance.value = stanceValue;

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
                            hitObject.GetComponentInParent<IDamage>().AddDamage(m_equip.Damage);
                            Instantiate(m_effect, hitPosition, Quaternion.identity);
                            Instantiate(m_frostEffect, hitPosition, Quaternion.identity, hitObject.transform);
                        }
                        if (!IsHitSound)
                        {
                            PlayHitSound(hitPosition);  // レーザーが当たった場所でヒット音を鳴らす
                            SoundManager.Instance.PlayFrost();
                            IsHitSound = true;
                        }
                    }
                }
            }
            else
            {

            }

        }
    }

    IEnumerator Fireline()
    {
        float time = 0;
        IsCreate = false;
        yield return new WaitForSeconds(1f);
        SoundManager.Instance.PlayCharge();
        yield return new WaitForSeconds(0.5f);
        IsCreate = true;
        m_player.StepForward(-10);
        m_anim.Play("Step");

    }

    public void EquipBullet(Bullet bullet)
    {
        m_equip = bullet;
    }

    /// <summary>
    /// ヒット音を鳴らす
    /// </summary>
    /// <param name="position">音を鳴らす場所</param>
    void PlayHitSound(Vector3 position)
    {
        if (m_hitSound) AudioSource.PlayClipAtPoint(m_hitSound, position, 0.1f);
    }
}
