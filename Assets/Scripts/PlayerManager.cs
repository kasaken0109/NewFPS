using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerManager : MonoBehaviour,IDamage
{
    public static PlayerManager Instance { get; private set; }
    // Start is called before the first frame update
    [SerializeField] int m_hp = 100;
    [SerializeField] float m_godTime = 0.4f;
    [SerializeField] float m_changeTime = 2f;
    [SerializeField] GameObject m_charactor;
    [SerializeField] GameObject m_invisible;
    [SerializeField] Text m_hptext = null;
    [SerializeField] Animator m_animator = null;
    [SerializeField] Slider hpslider = null;
    [SerializeField] Material m_change = null;
    [SerializeField] Material m_origin = null;
    /// <summary>
    /// 武器のNo.
    /// </summary>
    private int m_weaponNum = 0;
    private float keyInterval = 0f;
    private WeaponManager m_weaponManager;
    private int m_maxhp;
    bool IsInvisible = false;
    public float m_changeRate = 2f;

    public enum WeaponTypes: int
    {
        RIFLE,
        CANNON,
        NUM,
    }

    public enum StanceTypes
    {
        NORMAL,
        GOD,
    }

    public StanceTypes stanceTypes;

    private string[] m_weaponPath = new string[] {
        "PlayerRifle",
        "PlayerCannon"
    };

    private void Awake()
    {
        Instance = this;
        stanceTypes = StanceTypes.NORMAL;

    }
    void Start()
    {
        m_weaponManager = m_charactor.GetComponent<WeaponManager>();
        m_maxhp = m_hp;
    }

    // Update is called once per frame
    void Update()
    {
        m_hptext.text = "HP:" + m_hp.ToString();
        hpslider.value = (float)m_hp / m_maxhp;
        if(m_invisible.activeSelf) stanceTypes = StanceTypes.GOD;
        else stanceTypes = StanceTypes.NORMAL;
        if (Time.time - keyInterval > 0.5f)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) == true)
            {
                keyInterval = Time.time;
                PrevWeapon();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) == true)
            {
                keyInterval = Time.time;
                NextWeapon();
            }
        }
    }

    public void Damage (int damage)
    {
        m_hp -= damage;
        Debug.Log(m_hp);
    }

    private void PrevWeapon()
    {
        m_weaponNum--;
        if (m_weaponNum < 0)
        {
            m_weaponNum = (int)WeaponTypes.NUM - 1;
        }

        m_weaponManager.EquipWeapon(m_weaponPath[m_weaponNum]);
    }

    private void NextWeapon()
    {
        m_weaponNum++;
        if (m_weaponNum >= (int)WeaponTypes.NUM)
        {
            m_weaponNum = 0;
        }

        m_weaponManager.EquipWeapon(m_weaponPath[m_weaponNum]);
    }

    public void AddDamage(int damage)
    {
        if (IsInvisible)
        {
            StartCoroutine("GodTime");
            return;
        }
        if (m_hp > damage)
        {
            m_hp -= damage;
            m_animator.Play("Damage", 0);
            GetComponent<PlayerControll>().BasicHitAttack();
            DOTween.To(
                () =>hpslider.value, // getter
                x => hpslider.value = x, // setter
                (float)(float)m_hp / m_maxhp, // ターゲットとなる値
                1f  // 時間（秒）
                ).SetEase(Ease.OutCubic);
        }
        else
        {
            m_animator.SetBool("DeadFlag", true);
        }
    }

    public void SetInvisible()
    {
        StartCoroutine("Invisible");
    }

    IEnumerator Invisible()
    {
        IsInvisible = true;
        GetComponentInChildren<Renderer>().material = m_change;
        yield return new WaitForSeconds(m_godTime);
        GetComponentInChildren<Renderer>().material = m_origin;
        IsInvisible = false;
    }

    IEnumerator GodTime()
    {
        m_invisible?.SetActive(true);
        yield return new WaitForSeconds(m_changeTime);
        m_invisible?.SetActive(false);
    }
}
