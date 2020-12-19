using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    [SerializeField] GameObject m_bounceEffect;
    bool bounceflag = true;
    PlayerBulletController m_playerBullet;
    float m_bulletPower = 10f;
    // Start is called before the first frame update
    void Start()
    {
        m_playerBullet = GetComponent<PlayerBulletController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (bounceflag == true)
        {
            if (collision.collider.gameObject.tag == "Wall")
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
            Debug.Log("explosion");
            Destroy(this.gameObject);
            //Instantiate(m_bulletEffect, this.gameObject.transform.position, this.transform.rotation);

        }
    }
}
