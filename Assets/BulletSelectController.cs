using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletSelectController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("バレット選択UI")]
    private Image m_image;

    [SerializeField]
    [Tooltip("")]
    private Text m_bullet;

    [SerializeField]
    private BulletFire m_bulletFire = default;

    [SerializeField]
    private List<Bullet> m_IDs;

    private int bulletIndex = 0;

    private bool isSelected = false;
    // Start is called before the first frame update
    void Start()
    {
        m_image.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        isSelected = Input.GetButton("Fire3");
        m_image.gameObject.SetActive(Input.GetButton("Fire3"));

    }

    public void SelectBullet(int bullet)
    {
        Bullet temp = default;
        foreach (var item in m_IDs)
        {
            if (item.BulletID == bullet) temp = item;
        }
        m_bulletFire.EquipBullet(temp);
    }
}
