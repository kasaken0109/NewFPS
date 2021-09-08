using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    [SerializeField] GameObject m_effect = null;
    bool IsFirst = true;
    // Start is called before the first frame update
    private void OnEnable()
    {
        IsFirst = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsFirst)
        {
            var r = Instantiate(m_effect, new Vector3( transform.position.x,-0.38f,transform.position.z), Quaternion.identity);
            var m =  Instantiate(m_effect);
            m.transform.position = new Vector3(transform.position.x, 0.12f, transform.position.z);
            Quaternion mq = r.transform.rotation * Quaternion.Euler(0, -28, 0);
            m.transform.rotation = m.transform.rotation * mq;
            IsFirst = false;
        }

    }
}
