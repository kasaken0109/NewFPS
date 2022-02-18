using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostBulletController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("敵に着弾時に発生する")]
    private GameObject m_freeze = null;

    [SerializeField]
    [Tooltip("地面に着弾時に発生する")]
    private GameObject m_explosion = null;

    [SerializeField]
    private int m_attackpower = 10;

    private GameObject m_player = null;
    private　bool IsCreateWall;

    Vector3 hitPos;
    // Start is called before the first frame update
    void Start()
    {
        m_player = GameManager.Player.gameObject;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Floor")
        {
            Destroy(this.gameObject);

            foreach (ContactPoint point in collision.contacts)
            {
                hitPos = point.point;
            }
            IsCreateWall = true;
        }
        else if (collision.collider.tag == "Enemy" || collision.collider.tag == "Item")
        {
            collision.gameObject.GetComponentInParent<IDamage>().AddDamage(m_attackpower);
            foreach (ContactPoint point in collision.contacts) hitPos = point.point;
            IsCreateWall = false;
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        if (!m_freeze) return;

        var obj  = Instantiate(IsCreateWall ? m_freeze : m_explosion);
        obj.transform.position = new Vector3(hitPos.x , -1.38f, hitPos.z);
        Quaternion look = new Quaternion(0, m_player.transform.rotation.y, 0, 0) * obj.transform.rotation;
        obj.transform.rotation = look;
        SoundManager.Instance.PlayFrostWall();
    }
}
