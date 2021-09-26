using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    [SerializeField] GameObject animDoll;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void HitObject()
    {
        Debug.Log("呼び出し");
        Instantiate(animDoll, this.transform.position, this.transform.rotation);
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Bullet")
        {
            Instantiate(animDoll, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject);
        }    
    }
}
