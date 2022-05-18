using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BulletType
{
    Lay,
    Physics,
    Skill,
}

public class BulletFire : MonoBehaviour
{
    [SerializeField]
    [Tooltip("弾のエフェクトの発生する地点")]
    Transform m_particleMuzzle = null;

    [SerializeField]
    [Tooltip("パッシブスキルのエフェクトが発生する地点")]
    Transform m_passiveEffectPoint = null;

    [SerializeField]
    [Tooltip("スタンス")]
    Image m_stance = default;

    [SerializeField]
    [Tooltip("スタンス値の消費量予測線")]
    private Image m_line = default;

    [SerializeField]
    private PlayerControll m_player;

    [SerializeField]
    private Text m_bullet;

    [SerializeField]
    private Animator m_anim;

    [SerializeField]
    private PassiveDisplayController m_passiveDisplay = default;

    private Bullet m_equip;

    private float stanceValue = 0.5f;


    Vector3 hitPosition;
    float consumeValue;
    // Start is called before the first frame update
    void Start()
    {
        m_stance.fillAmount = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GameStatus == GameManager.GameState.STOP) return;//一時停止中には射撃不可
        stanceValue = m_stance.fillAmount;

        if(m_equip != null)m_line.fillAmount = m_stance.fillAmount - consumeValue;

        if (Input.GetButtonDown("Fire2"))
        {
            SoundManager.Instance.StopSE();

            //銃弾を生成
            if (stanceValue >= m_equip.ConsumeStanceValue)
            {
                SoundManager.Instance.PlayShoot();
                var instance = Instantiate(m_equip.MyBullet, m_particleMuzzle.position, Camera.main.transform.rotation);
                stanceValue -= m_equip.ConsumeStanceValue;
                m_stance.fillAmount = stanceValue;

                if (m_equip.BulletType == BulletType.Lay) instance.GetComponent<BulletLaserController>().Damage = m_equip.Damage;
                else if (m_equip.BulletType == BulletType.Skill) FindObjectOfType<PlayerManager>().AddDamage(-30);
            }
            //パッシブ用のコストがある場合パッシブを発動
            if(stanceValue >= consumeValue - m_equip.ConsumeStanceValue)
            {
                Debug.Log("Consume");
                CallPassiveInstance(m_equip.passiveSkill_1);
                CallPassiveInstance(m_equip.passiveSkill_2);
            }
        }
    }

    private void CallPassiveInstance(PassiveSkill passiveSkill)
    {
        if (passiveSkill != null)
        {
            SkillBehavior.Instance.CallPassive(ref passiveSkill);
            var obj = Instantiate(passiveSkill.Effect, m_passiveEffectPoint.position, Quaternion.identity);
            var particle = obj.GetComponent<ParticleSystem>().main;
            particle.duration = passiveSkill.EffectableTime;
            stanceValue -= passiveSkill.ConsumeCost;
        }
    }

    public void EquipBullet(Bullet bullet) {
        m_equip = bullet;
        m_bullet.text = bullet.Name;

        //パッシブスキルセット時のコストを計算
        consumeValue = m_equip.ConsumeStanceValue + (m_equip.PassiveSkill1 != null ? m_equip.PassiveSkill1.ConsumeCost : 0)
            + (m_equip.PassiveSkill2 != null ? m_equip.PassiveSkill2.ConsumeCost : 0);

    }
}
