using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPanel : MonoBehaviour
{
    [SerializeField] Quest m_quest = null;
    string m_loadSceneName = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void LoadQuest()
    {
        SceneLoader.Instance.SceneLoad(m_loadSceneName);
    }
}
