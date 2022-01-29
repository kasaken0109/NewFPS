using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNewController : MonoBehaviour
{
    [SerializeField]
    private float m_power = 10;
    [SerializeField]
    private float m_hp = 100;
    [SerializeField]
    private GameObject deathBody;
    [SerializeField]
    private AudioClip m_hit;

    bool IsDoolExisted = false;
    // Start is called before the first frame update
   
    public void Hit()
    {
        if (!IsDoolExisted)
        {
            Instantiate(deathBody, transform.position, transform.rotation);
            Destroy(gameObject);
            IsDoolExisted = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Bullet" && !IsDoolExisted)
        {
            Instantiate(deathBody, transform.position, transform.rotation);
            Destroy(gameObject);
            IsDoolExisted = true;
        }
    }
}
