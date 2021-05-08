using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNewController : MonoBehaviour
{
    [SerializeField] float e_power = 10;
    [SerializeField] float e_hp = 100;
    [SerializeField] GameObject deathBody;
    [SerializeField] AudioClip e_hit;
    bool IsDoolExisted = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if (true)
        //{

        //}
    }

    public void Hit()
    {
        if (!IsDoolExisted)
        {
            Instantiate<GameObject>(deathBody, transform.position, transform.rotation);
            //this.gameObject.SetActive(false);
            Destroy(this.gameObject);
            IsDoolExisted = true;
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Bullet" && !IsDoolExisted)
        {
            Instantiate<GameObject>(deathBody, transform.position, transform.rotation);
            //this.gameObject.SetActive(false);
            Destroy(this.gameObject);
            IsDoolExisted = true;
        }
    }
}
