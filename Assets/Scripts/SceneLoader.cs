using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] string m_LoadSceneName = "SceneNameToBeLoaded";
    [SerializeField] Image m_mask = null;
    bool m_isLoading = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isLoading)
        {
            if (m_mask)
            {
                //m_mask.SetActive(true);
                //while (m_mask.transform.localScale.x <= 10f)
                //{
                //    Transform m_size = m_mask.transform;
                //    m_size.transform.localScale.x *= 1.1f;
                //}
                SceneManager.LoadScene(m_LoadSceneName);
            }
            else
            {
                SceneManager.LoadScene(m_LoadSceneName);
            }
            
        }
    }


    public void SceneLoad()
    {
        m_isLoading = true;
    }

    public void SceneLoad(string sceneName)
    {
        m_isLoading = true;
        m_LoadSceneName = sceneName;
    }
}
