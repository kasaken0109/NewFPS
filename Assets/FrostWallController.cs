using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostWallController : MonoBehaviour
{
    /// <summary> 破壊するときに発生する攻撃エフェクト/// </summary>
    [SerializeField] GameObject m_effect;
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
            Destroy(this.gameObject);
        }
    }
}
