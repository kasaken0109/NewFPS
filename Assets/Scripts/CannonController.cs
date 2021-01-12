using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    [SerializeField] GameObject m_bounceEffect;
    [SerializeField] GameObject m_bulletEffect;
    bool bounceflag = true;
    float m_bulletPower = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (bounceflag == true)
        {
            Instantiate(m_bounceEffect, this.gameObject.transform.position, this.transform.rotation);
            if (collision.gameObject.GetComponent<EnemyContoroller>())
            {
                collision.gameObject.GetComponent<EnemyContoroller>().Hit(m_bulletPower);
            }
            bounceflag = false;
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
            Instantiate(m_bulletEffect, this.gameObject.transform.position, this.transform.rotation);

        }
    }
}
