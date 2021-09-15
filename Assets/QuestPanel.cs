﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestPanel : MonoBehaviour
{  
    [SerializeField] Image m_questImage = null;
    [SerializeField] Image m_questBackGroundImage = null;
    [SerializeField] Text m_questName = null;
    [SerializeField] Text m_questText = null;
    string m_loadSceneName = null;

    private Quest m_quest = null;
    public Quest SetQuest
    {
        set { m_quest = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (!m_quest) Debug.LogError("QuestDataがありません！");
        m_questImage.sprite = m_quest.QuestImage();
        m_questBackGroundImage.sprite = m_quest.QuestBackGroundImage();
        m_loadSceneName = m_quest.LoadSceneName();
        m_questName.text = m_quest.QuestName();
        m_questText.text = m_quest.QuestText();
    }

    private void Update()
    {
        if (!m_quest) Debug.LogError("QuestDataがありません！");
        m_questImage.sprite = m_quest.QuestImage();
        m_questBackGroundImage.sprite = m_quest.QuestBackGroundImage();
        m_loadSceneName = m_quest.LoadSceneName();
        m_questName.text = m_quest.QuestName();
        m_questText.text = m_quest.QuestText();
    }

    public void LoadQuest()
    {
        SceneLoader.Instance.SceneLoad(m_loadSceneName);
    }
}
