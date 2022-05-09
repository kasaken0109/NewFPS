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
    [Tooltip("強調表示するUIに付属するクラス")]
    private UISelecter[] m_selecters;

    [SerializeField]
    [Tooltip("メニューコントローラー")]
    private MenuController m_menu;

    [SerializeField]
    [Tooltip("射撃を管理するクラス")]
    private BulletFire m_bulletFire = default;

    [SerializeField]
    [Tooltip("弾を設定するリスト")]
    private List<Bullet> m_IDs;

    private Vector3 origin;

    private Vector3 padOrigin;

    private bool isEnter = false;

    private bool useGamePads = false;

    private int equipID = 0;

    private bool IsPreScroll = false;

    public List<Bullet> MyBullet { set { m_IDs = value; } }

    // Start is called before the first frame update
    void Start()
    {
        SelectBullet(equipID);//弾の選択状態の初期化
        m_image.gameObject.SetActive(false);
        IsPreScroll = false;
        origin = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);//スクリーンの中心座標を設定
        padOrigin = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }

    // Update is called once per frame
    void Update()
    {
        var controllerNames = Input.GetJoystickNames();
        useGamePads = controllerNames.Length == 0 ? false : true;

        float hori = Input.GetAxis("Mouse X");
        float vert = Input.GetAxis("Mouse Y");
        Vector3 input = new Vector3(hori, vert, 0);
        padOrigin = input;

        float scrollValue = Input.mouseScrollDelta.y;//Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scrollValue) < 0.1f) IsPreScroll = false;
        else
        {
            if (IsPreScroll) return;
            equipID = scrollValue > 0 ? (equipID == 0 ? 2 : equipID - 1) : (equipID == 2 ? 0 : equipID +1);
            SelectBullet(equipID);
            IsPreScroll = true;
        }
        //if (Input.GetButton("Fire3"))
        //{
        //    if(useGamePads) SelectPadUI(input);
        //    else SelectUI(Input.mousePosition);
        //}
        //m_image.SetActive(Input.GetButton("Fire3"));//マウスホイールを押している間だけUIを表示
        //m_menu.SetCamera(!Input.GetButton("Fire3"));//カメラの機能を切り替える

    }

    public void SelectBullet(int bullet)
    {
        Bullet temp = default;
        foreach (var item in m_IDs)
        {
            if (item.BulletID == bullet) temp = item;//IDが一致したら同一とみなす
        }
        m_bulletFire.EquipBullet(temp);

        //選択されているUI部分を強調表示する
        for (int i = 0; i < m_selecters.Length; i++)
        {
            m_selecters[i].SelectedUI(i == bullet);
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
    /// EventTrigger用　判定エリアに入った時に呼び出される
    /// </summary>
    /// <param name="isArea"></param>
    public void IsInUIArea(bool isArea) => isEnter = isArea;

    private void OnDestroy()
    {
        equipID = 0;
    }
}
