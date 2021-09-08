using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathController : MonoBehaviour
{
    [SerializeField] float m_speed;
    Rigidbody rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = GameObject.Find("EnemyBoss").transform.forward * m_speed;
    }
}
