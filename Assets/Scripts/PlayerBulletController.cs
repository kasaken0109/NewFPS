using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletController : MonoBehaviour
{
    // Start is called before the first frame update
    
    /// <summary>弾の飛ぶ速度</summary>
    [SerializeField] float m_bulletSpeed = 10f;
    Rigidbody m_rb;
    [SerializeField] float m_bulletPower = 12f;
    [SerializeField] GameObject m_bounceEffect;
    [SerializeField]GameObject m_bulletEffect;
    GameObject m_player;
    bool bounceflag = true;

    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        //h_vector = v_speed.dir;
        this.m_rb.velocity = Camera.main.transform.forward * m_bulletSpeed;
    }
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (bounceflag == true)
        {
            this.m_rb.velocity *= -1;
            Instantiate(m_bulletEffect, this.gameObject.transform.position, this.transform.rotation);
            if (collision.gameObject.GetComponent<EnemyContoroller>())
            {
                collision.gameObject.GetComponent<EnemyContoroller>().Hit(m_bulletPower);
            }
            Debug.Log("bounced");
        }
        else
        {
            if (collision.gameObject.GetComponent<EnemyContoroller>())
            {
                collision.gameObject.GetComponent<EnemyContoroller>().Hit(m_bulletPower * 2);
            }
            Destroy(this.gameObject);
            Instantiate(m_bulletEffect, this.gameObject.transform.position, this.transform.rotation);
            Debug.Log("explosion");
        }
    }
}
