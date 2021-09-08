using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] string m_LoadSceneName = "SceneNameToBeLoaded";
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
            SceneManager.LoadScene(m_LoadSceneName);      
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
