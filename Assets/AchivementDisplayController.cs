using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using NCMB;

public class AchivementDisplayController : MonoBehaviour
{
    [Tooltip("アチーブメントを表示するテキスト")]
    [SerializeField]
    Text m_text = default;
    List<NCMBObject> m_achievement;

    public void DownloadData()
    {
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("HighScore");

        query.FindAsync((List<NCMBObject> objList, NCMBException e) =>
        {
            m_achievement = objList;
            if (e != null)
            {
                
            }
            else
            {
                m_text.text = "ジャスト回避した回数：" + m_achievement[m_achievement.Count -1]["DodgeCount"];
            }
        });
    }

    public void UpdateDeta()
    {
        NCMBObject obj = new NCMBObject("HighScore");
        obj["DodgeCount"] = AchivementManager.Instance.DodgeCount;

        obj.SaveAsync((NCMBException e) =>
        {
            if (e != null) Debug.LogWarning("Detaがアップロード出来ません！！");
        });
    }

    private void OnDestroy()
    {
        UpdateDeta();
    }
}
