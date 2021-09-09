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
    [SerializeField] int m_maxShield = 2;
    [SerializeField] GameObject m_charactor;
    [SerializeField] GameObject m_healEffect;
    [SerializeField] GameObject m_invisible;
    [SerializeField] GameObject m_dead;
    [SerializeField] GameObject[] m_weaponImage;
    [SerializeField] Text m_hptext = null;
    [SerializeField] Animator m_animator = null;
    [SerializeField] Slider hpslider = null;
    [SerializeField] Material m_change = null;
    [SerializeField] Material m_origin = null;
    [SerializeField] PostEffect postEffect = null;
    [SerializeField] FrostEffect m_frost = null;
    [SerializeField] ShieldDisplayController shieldDisplay;
    [SerializeField] bool equipMode = true;
    public GameObject m_reloadImage = null;
    public GameObject m_textBox1 = null;
    public GameObject m_textBox2 = null;
    /// <summary>
    /// 武器のNo.
    /// </summary>
    private int m_weaponNum = 0;
    public int shieldNum;
    private float keyInterval = 0f;
    private WeaponManager m_weaponManager;
    private int m_maxhp;
    bool IsInvisible = false;
    public float m_changeRate = 2f;
    public bool IsAlive = true;
    public enum WeaponTypes: int
    {
        RIFLE,
        CANNON,
        SWORD,
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
        "PlayerCannon",
        "Sword",
    };

    private void Awake()
    {
        Instance = this;
        shieldNum = 0;
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
        //m_hptext.text = "HP:" + m_hp.ToString();
        hpslider.value = (float)m_hp / m_maxhp;
        if (postEffect.enabled)
        {
            stanceTypes = StanceTypes.GOD;
        }
        else
        {
            stanceTypes = StanceTypes.NORMAL;
        }
        if (equipMode)
        {
            if (Time.time - keyInterval > 0.2f)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1) == true)
                {
                    keyInterval = Time.time;
                    FirstWeapon();
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2) == true)
                {
                    keyInterval = Time.time;
                    SecondWeapon();
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3) == true)
                {
                    keyInterval = Time.time;
                    ThirdWeapon();
                }
            }
        }
        else
        {
            if (Time.time - keyInterval > 0.2f)
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
        SetWeaponImage(m_weaponNum);
    }

    private void NextWeapon()
    {
        m_weaponNum++;
        if (m_weaponNum >= (int)WeaponTypes.NUM)
        {
            m_weaponNum = 0;
        }

        m_weaponManager.EquipWeapon(m_weaponPath[m_weaponNum]);
        SetWeaponImage(m_weaponNum);
    }

    private void FirstWeapon()
    {
        m_weaponManager.EquipWeapon(m_weaponPath[0]);
        SetWeaponImage(0);
    }

    private void SecondWeapon()
    {
        m_weaponManager.EquipWeapon(m_weaponPath[1]);
        SetWeaponImage(1);
    }

    private void ThirdWeapon()
    {
        m_weaponManager.EquipWeapon(m_weaponPath[2]);
        SetWeaponImage(2);
    }

    void SetWeaponImage(int weaponNum)
    {
        for (int i = 0; i < m_weaponImage.Length; i++)
        {
            if (i == weaponNum)
            {
                m_weaponImage[i].SetActive(true);
            }
            else
            {
                m_weaponImage[i].SetActive(false);
            }
        }
    }
    public void AddDamage(int damage)
    {
        Debug.Log(m_hp);
        if (IsInvisible)
        {
            if (!IsActiveCoroutine)
            {
                StopCoroutine("GodTime");
                StartCoroutine("GodTime");
            }
            if(damage > 0) return;
        }
        if (m_hp > damage)
        {
            if(damage < 0)
            {
                m_hp -= damage;
                if (m_hp >= m_maxhp) m_hp = m_maxhp;
                Instantiate(m_healEffect, transform.position, Quaternion.identity);
            }
            else
            {
                if (shieldDisplay.ShieldValue == 1) shieldNum = m_maxShield;
                if(shieldNum >= 1)
                {
                    shieldNum--;
                    shieldDisplay.ChangeValues((float)(shieldNum + 1) / (m_maxShield + 1));
                    damage = (int)(damage * 0.5f);
                }
                else
                {
                    if (shieldNum == 0)
                    {
                        shieldDisplay.ChangeValues(0);
                        Destroy(GameObject.Find("ShieldPrefab(Clone)"));
                    }
                    m_animator.Play("Damage", 0);
                    GetComponent<PlayerControll>().BasicHitAttack();
                }
                m_hp -= damage;
            }
            
            DOTween.To(
                () =>hpslider.value, // getter
                x => hpslider.value = x, // setter
                (float)(float)m_hp / m_maxhp, // ターゲットとなる値
                1f  // 時間（秒）
                ).SetEase(Ease.OutCubic);
        }
        else
        {
            var m =Instantiate(m_dead);
            GameManager.Instance.GameStatus = GameManager.GameState.PLAYERLOSE;
            m.transform.position = transform.position;
            gameObject.SetActive(false);
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
    IEnumerator Invisible(float time)
    {
        IsInvisible = true;
        GetComponentInChildren<Renderer>().material = m_change;
        yield return new WaitForSeconds(time);
        GetComponentInChildren<Renderer>().material = m_origin;
        IsInvisible = false;
    }

    bool IsActiveCoroutine = false;
    IEnumerator GodTime()
    {
        StopCoroutine(nameof(Invisible));
        IsActiveCoroutine = true;
        m_frost.FrostAmount = 1f;
        IsInvisible = true;
        GetComponentInChildren<Renderer>().material = m_change;
        while (m_frost.FrostAmount > 0.0001f)
        {
            m_frost.FrostAmount -= 0.003f;
            yield return new WaitForSeconds(m_changeTime /100);

        }
        //m_invisible?.SetActive(false);
        postEffect.enabled = false;
        GetComponentInChildren<Renderer>().material = m_origin;
        IsInvisible = false;
        IsActiveCoroutine = false;
    }
}
