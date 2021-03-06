﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackcolliderController : MonoBehaviour
{
    [SerializeField] int m_attackPower = 15;
    [SerializeField] string m_opponentTagName = "Player";
    bool CanHit;
    // Start is called before the first frame update
    private void OnEnable()
    {
        CanHit = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "Enemy")collision.gameObject.GetComponentInParent<IDamage>().AddDamage(m_attackPower);

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.tag == m_opponentTagName && CanHit)
        {
            other.gameObject.GetComponentInParent<IDamage>().AddDamage(m_attackPower);
            CanHit = false;
        }
    }
}
