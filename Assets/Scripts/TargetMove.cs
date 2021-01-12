using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMove : MonoBehaviour
{
    Animation m_target;
    float m_animTime = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        m_target = this.gameObject.GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        m_target.Play();
        Destroy(this.gameObject, m_animTime);
    }

    
}
