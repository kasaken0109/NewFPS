using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletSelectController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("強調表示するUIに付属するクラス")]
    private UISelecter[] _selecters;

    [SerializeField]
    [Tooltip("射撃を管理するクラス")]
    private BulletFire m_bulletFire = default;

    [SerializeField]
    [Tooltip("弾を設定するリスト")]
    private List<Bullet> m_IDs;

    [SerializeField]
    private Animator _anim;

    private BulletSelectDisplay bulletDisplay;

    private Vector3 origin;

    private Vector3 padOrigin;

    private int equipID = 0;

    private bool IsPreScroll = false;

    private bool IsPush = false;


    public List<Bullet> MyBullet { set { m_IDs = value; } }

    private int[] bulletIDs;



    // Start is called before the first frame update
    void Start()
    {
        bulletDisplay = GetComponent<BulletSelectDisplay>();
        bulletDisplay.BulletInformationInit(EquipmentManager.Instance.Equipments);
        EquipBullets();
        bulletIDs = new int[3] {0,1,2};
        SelectBullet(equipID);//弾の選択状態の初期化
        bulletDisplay.MoveSelectFrame(equipID);
        IsPreScroll = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire3"))
        {
            IsPush = IsPush ? false : true;
            _anim.SetBool("IsPush", IsPush);
        }
        if (!IsPush) return;
        float scrollValue = Input.mouseScrollDelta.normalized.y;//Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scrollValue) < 0.5f) IsPreScroll = false;
        else
        {
            if (IsPreScroll) return;
            SelectBullet(scrollValue);
            IsPreScroll = true;
        }
    }

    private void SelectBullet(float scrollValue)
    {
        equipID = scrollValue > 0 ? (equipID == 0 ? 2 : equipID - 1) : (equipID == 2 ? 0 : equipID + 1);
        SelectBullet(equipID);
        bulletDisplay.MoveSelectFrame(equipID);
        if (!m_IDs[equipID]) SelectBullet(scrollValue);
    }

    public void SelectBullet(int bullet)
    {
        //Bullet temp = default;
        //foreach (var item in m_IDs)
        //{
        //    if (item.BulletID == bullet) temp = item;//IDが一致したら同一とみなす
        //}
        m_bulletFire.EquipBullet(EquipmentManager.Instance.Equipments[bullet]);

        //選択されているUI部分を強調表示する
        for (int i = 0; i < _selecters.Length; i++)
        {
            _selecters[i].SelectedUI(i == bullet);
        }
    }

    /// <summary>
    /// 角度に応じて弾を選択
    /// </summary>
    /// <param name="mousePoint"></param>
    private void SelectUI(Vector3 mousePoint)
    {
        var value = GetAngle(mousePoint);//角度を取得
        //角度の値に応じて弾をセット
        if (value > 330f && value <= 360f || value >= 0 && value < 90f) SelectBullet(0);
        else if (value >= 90f && value < 210f) SelectBullet(1);
        else SelectBullet(2);
                
    }

    /// <summary>
    /// 角度に応じて弾を選択
    /// </summary>
    /// <param name="mousePoint"></param>
    private void SelectPadUI(Vector3 mousePoint)
    {
        //角度をベクトルから計算
        Vector3 dir = mousePoint - padOrigin;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        //角度の値に応じて弾をセット
        if (angle > 330f && angle <= 360f || angle >= 0 && angle < 90f) SelectBullet(0);
        else if (angle >= 90f && angle < 210f) SelectBullet(1);
        else SelectBullet(2);

    }

    /// <summary>
    /// マウスの位置から角度を計算する
    /// </summary>
    /// <param name="mousePoint">マウスの座標</param>
    /// <returns></returns>
    private float GetAngle(Vector3 mousePoint)
    {
        //角度をベクトルから計算
        Vector3 dir = mousePoint - origin;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        return angle > 0f ? angle : angle + 360;//マイナスの値を360度形式に変換
    }

    /// <summary>
    /// EquipmentManegerに設定されている装備を選択
    /// </summary>
    private void EquipBullets()
    {
        var equipM = EquipmentManager.Instance;
        for (int i = 0;i < m_IDs.Count;i++) m_IDs[i] = equipM.Equipments[i] ? equipM.Equipments[i] : m_IDs[i];
    }

    private void SetBulletIDs(bool isUp)
    {
        var change = isUp ? -1 : 1;
        for (int i = 0; i < bulletIDs.Length; i++)
        {
            bulletIDs[i] = bulletIDs[i] + change;
            if (bulletIDs[i] >= bulletIDs.Length) bulletIDs[i] = 0;
            if (bulletIDs[i] < 0) bulletIDs[i] = bulletIDs.Length - 1;
        }
    }

    private void OnDestroy()
    {
        equipID = 0;
    }
}
