using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostBulletController : MonoBehaviour
{
    [SerializeField] GameObject m_freeze = null;
    [SerializeField] GameObject m_explosion = null;
    [SerializeField] GameObject m_player = null;
    [SerializeField] int m_attackpower = 10;
    bool IsCreateWall;
    Vector3 hitPos;
    Transform hitTransform;
    // Start is called before the first frame update
    void Start()
    {
        m_player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Floor")
        {
            Destroy(this.gameObject);
            hitTransform = m_player.transform;

            foreach (ContactPoint point in collision.contacts)
            {
                hitPos = point.point;
            }
            IsCreateWall = true;
        }
        else if (collision.collider.tag == "Enemy" || collision.collider.tag == "Item")
        {
            collision.gameObject.GetComponentInParent<IDamage>().AddDamage(m_attackpower);
            hitTransform = m_player.transform;
            foreach (ContactPoint point in collision.contacts)
            {
                hitPos = point.point;
            }
            IsCreateWall = false;
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        if (!m_freeze)
        {
            return;
        }
        var m  = Instantiate(IsCreateWall ? m_freeze : m_explosion);
        float yInstance = m_freeze.transform.lossyScale.y / 2;
        m.transform.position = new Vector3(hitPos.x ,0 , hitPos.z);
        m.transform.rotation = hitTransform.rotation;
        SoundManager.Instance.PlayFrostWall();
    }
}
