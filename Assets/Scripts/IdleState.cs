using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : StateBase
{
    // Start is called before the first frame update
    [SerializeField]StateBase _attackState;
    [SerializeField] StateBase _moveState;

    protected override void Setup()
    {
        Debug.Log("EnterIdle");

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (_opponentTag == "") return;
        if (collision.gameObject == null) return;
        if (collision.gameObject.CompareTag(_opponentTag))
        {
            Debug.Log(collision.gameObject.tag);
            _actionCtrl.SetCurrent(_attackState);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_opponentTag == "") return;
        if (other.gameObject == null) return;
        if (other.gameObject.CompareTag(_opponentTag))
        {
            //Debug.Log("Ok");
            _actionCtrl.SetCurrent(_moveState);
        }
    }
}
