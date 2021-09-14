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
    public string LoadSceneName() { return m_loadSceneName; }
    public string QuestName() { return m_questName; }
    public string QuestText() { return m_questText; }

    public Sprite QuestImage() { return m_image; }

    // Start is called before the first frame update
}
