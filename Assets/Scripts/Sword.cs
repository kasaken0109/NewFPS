﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Sword : MonoBehaviour,IWeapon
{
    [SerializeField] float[] m_activeColliderTime;
    [SerializeField] GameObject[] m_activeCollider;
    [SerializeField] AttackcolliderController controller;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GameManager.Instance.m_player.GetComponent<Rigidbody>();
        PlayerManager.Instance.m_reloadImage.SetActive(false);
        StartCoroutine(nameof(Changeattack));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Changeattack()
    {
        float timer = 0;
        while (timer < 0.5f)
        {
            if (Input.GetButtonDown("Fire1")) GameManager.Instance.m_player.GetComponent<PlayerControll>().DushAttack();
            timer += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
    }
    public void NormalAttack()
    {
        StartCoroutine(ColliderGenerater.Instance.GenerateCollider(m_activeCollider[0], 1f));
    }

    public void BasicAttack()
    {
        ColliderGenerater.Instance.StartActiveCollider(m_activeCollider[0], 1f);
    }

    public void SpecialAttack()
    {
        ColliderGenerater.Instance.StartActiveCollider(m_activeCollider[1], 2f);
        //rb.useGravity = true;
        rb.DOMoveY(0,0.5f);
    }

    public void StopEmitting()
    {
        GetComponentInChildren<TrailRenderer>().emitting = false;
    }

    public void StartEmitting()
    {
        GetComponentInChildren<TrailRenderer>().emitting = true;
        rb = GameManager.Instance.m_player.GetComponent<Rigidbody>();
        
        //rb.useGravity = false;
        //rb.velocity = Vector3.zero;
        int dmg = (int)Mathf.Abs(transform.position.y - GameObject.FindGameObjectWithTag("Floor").transform.position.y) * 4;
        int correctDmg = dmg >= 50 ? 50 : dmg;
        controller.AddDamageCount(correctDmg);
        //Debug.Log(correctDmg);
    }
    public void FloatUp()
    {
        rb.DOMoveY(GameManager.Instance.m_player.transform.position.y + 2, 0.3f);
    }
}
