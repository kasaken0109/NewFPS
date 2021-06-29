﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostWallController : MonoBehaviour
{
    /// <summary> 破壊するときに発生する攻撃エフェクト/// </summary>
    [SerializeField] GameObject m_effect;
    /// <summary> 破壊するときに発生する攻撃コライダー/// </summary>
    [SerializeField] GameObject m_attackCollider = null;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "AttackCollider")
        {
            Instantiate(m_effect, this.transform.position,this.transform.rotation);
            m_attackCollider.SetActive(true);
            StartCoroutine(SetNonActive());
            Destroy(this.gameObject,1);
        }
    }

    private void OnDestroy()
    {
        
    }

    IEnumerator SetNonActive()
    {
        m_attackCollider.SetActive(true);
        yield return new WaitForSeconds(1);
        m_attackCollider.SetActive(false);
    }
}
