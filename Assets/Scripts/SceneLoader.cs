using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; } 
    [SerializeField] string m_LoadSceneName = "SceneNameToBeLoaded";
    [SerializeField] float m_fadeSpeed = 1f;
    [SerializeField] Image m_loadPanel = null;
    bool m_isLoading = false;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Load()
    {
        Debug.Log("adsd");
        if (m_isLoading)
        {
            Debug.Log("Called");
            while(m_loadPanel.fillAmount < 0.99f)
            {
                m_loadPanel.fillAmount += 0.02f;
                yield return new WaitForSeconds(0.1f / m_fadeSpeed);
            }
            SceneManager.LoadScene(m_LoadSceneName);
        }
    }


    public void SceneLoad()
    {
        m_isLoading = true;
        StartCoroutine(nameof(Load));
    }

    public void SceneLoad(string sceneName)
    {
        m_isLoading = true;
        m_LoadSceneName = sceneName;
        StartCoroutine(nameof(Load));
    }
}
