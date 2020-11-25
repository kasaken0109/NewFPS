using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyContoroller : MonoBehaviour
{
    [SerializeField] float e_power = 10;
    [SerializeField] float e_hp = 100;
    [SerializeField] GameObject deathBody;
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

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("HIT");
        if (collision.gameObject.GetComponent<PlayerManager>())
        {
            collision.gameObject.GetComponent<PlayerManager>().Damage(e_power);
        }
    }

    public void Hit(float damage){
        GameObject inst;
        e_hp -= damage;
        Debug.Log(e_hp);

        if (e_hp <= 0)
        {
            Destroy(this.gameObject);
            inst = Instantiate<GameObject>(deathBody, transform.position, transform.rotation);
        }
    }
}
