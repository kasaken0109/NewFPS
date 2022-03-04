using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletSelectController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("バレット選択UI")]
    private GameObject m_image;

    [SerializeField]
    [Tooltip("")]
    private Text m_bullet;

    [SerializeField]
    [Tooltip("")]
    private UISelecter[] m_selecters;

    [SerializeField]
    private BulletFire m_bulletFire = default;

    [SerializeField]
    private List<Bullet> m_IDs;

    private int bulletIndex = 0;

    private Vector3 origin;

    private bool isEnter = false;

    // Start is called before the first frame update
    void Start()
    {
        SelectBullet(0);
        m_image.gameObject.SetActive(false);
        origin = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnter) SelectUI(Input.mousePosition);//判定エリアに入ったときに選択関数を呼び出し
        //if (Input.GetButton("Fire3"))
        //{
        //    m_image.SetActive(Input.GetButton("Fire3"));
        //}
        //else
        //{
        //    m_image.SetActive(false);
        //}
        m_image.SetActive(Input.GetButton("Fire3"));//マウスホイールを押している間だけUIを表示

    }

    public void SelectBullet(int bullet)
    {
        Bullet temp = default;
        foreach (var item in m_IDs)
        {
            if (item.BulletID == bullet) temp = item;
        }
        m_bulletFire.EquipBullet(temp);
        for (int i = 0; i < m_selecters.Length; i++)
        {
            m_selecters[i].SelectedUI(i == bullet);
        }
    }

    private void SelectUI(Vector3 mousePoint)
    {
        var value = GetAngle(mousePoint);
        if (value > 330f && value <= 360f || value >= 0 && value < 90f) SelectBullet(0);
        else if (value >= 90f && value < 210f) SelectBullet(1);
        else SelectBullet(2);
                
    }

    private float GetAngle(Vector3 mousePoint)
    {
        Vector3 dir = mousePoint - origin;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        return angle > 0f ? angle : angle + 360;
    }

    public void IsInUIArea(bool isArea) => isEnter = isArea;
}
