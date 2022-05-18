using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class JumpUpController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("上昇量")]
    float m_upAmount = 100f;
    Rigidbody m_rb;

    AttackcolliderController attackcolliderController;
    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponentInParent<Rigidbody>();
        attackcolliderController = GetComponent<AttackcolliderController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(attackcolliderController.AttackPower != 0)m_rb.DOMove(transform.position + transform.up * m_upAmount, 0.5f);
    }
}
