using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLaserController : MonoBehaviour
{
    [Tooltip("照準のUI")]
    private RectTransform m_crosshairUi = null;

    [SerializeField]
    [Tooltip("射程距離")]
    private float m_shootRange = 50f;

    [SerializeField]
    [Tooltip("当たるレイヤー")]
    private LayerMask m_layerMask = 0;

    [SerializeField]
    [Tooltip("命中した時の音")]
    private AudioClip m_hitSound = null;

    [SerializeField]
    [Tooltip("着弾時に発生するエフェクト")]
    private GameObject m_effect = null;

    [SerializeField]
    [Tooltip("")]
    private GameObject m_frostEffect = null;

    private int damage;

    public int Damage { set { damage = value; } }

    private bool IsSounded = false;
    private bool IsHitSound = false;

    private RaycastHit hit;
    private Vector3 hitPosition;
    private bool EndHit = false;
    // Start is called before the first frame update
    void Start()
    {
        m_crosshairUi = GameManager.Instance.CrosshairUI;
        EndHit = false;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(m_crosshairUi.position);

        GameObject hitObject = null;    // Ray が当たったオブジェクト

        if(!EndHit)hit = RayHit(ray, ref hitObject);
    }

    private RaycastHit RayHit(Ray ray, ref GameObject hitObject)
    {
        EndHit = true;
        bool IsHit = Physics.Raycast(ray, out hit, m_shootRange, m_layerMask);

        if (IsHit)
        {

            hitPosition = hit.point;    // Ray が当たった場所
            hitObject = hit.collider.gameObject;    // Ray が洗ったオブジェクト

            if (hitObject)
            {
                if (hitObject.CompareTag("Enemy") || hitObject.CompareTag("Item"))
                {
                    IsSounded = !IsSounded ? true : false;
                    hitObject.GetComponentInParent<IDamage>().AddDamage(damage);
                    Debug.Log(damage);
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
        return hit;
    }

    /// <summary>
    /// ヒット音を鳴らす
    /// </summary>
    /// <param name="position">音を鳴らす場所</param>
    private void PlayHitSound(Vector3 position)
    {
        if (m_hitSound) AudioSource.PlayClipAtPoint(m_hitSound, position, 0.1f);
    }
}
