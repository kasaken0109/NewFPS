using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptable/Create QuestData")]
public class Quest : ScriptableObject
{
    [SerializeField] string m_loadSceneName = null;
    [SerializeField] string m_questName = null;
    [SerializeField] string m_questText = null;
    [SerializeField] Sprite m_image = null;
    // Start is called before the first frame update
    public void LoadQuest()
    {
        SceneLoader.Instance.SceneLoad(m_loadSceneName);
    }
}
