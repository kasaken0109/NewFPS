using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMove : MonoBehaviour
{
    [SerializeField] Animator m_target;
    // Start is called before the first frame update
    void Start()
    {
        m_target = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag == ("Bullet"))
        {
            m_target.SetTrigger("Hit");
        }  
    }
}
